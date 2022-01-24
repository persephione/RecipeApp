using RecipeApp.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RecipeApp.API.Services
{
    public class RecipeService : IRecipeService
    {
        private static string _connStr;
        private static SqlConnection _connection;

        public RecipeService(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connStr);
        }

        public async Task<List<IdValuePair>> GetAllRecipesAsync()
        {
            var list = new List<IdValuePair>();
            var dataSet = new DataSet();

            try
            {
                await _connection.OpenAsync().ContinueWith((task) => {
                    var command = new SqlCommand("sp_GetRecipes", _connection);
                    command.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapter(command);

                    adapter.Fill(dataSet);

                    for (int i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
                    {
                        var id = dataSet.Tables[0].Rows[i][0] as int?;
                        if (id != null)
                        {
                            list.Add(new IdValuePair
                            {
                                Id = (int)dataSet.Tables[0].Rows[i][0],
                                Value = dataSet.Tables[0].Rows[i][1].ToString()
                            });
                        }
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            catch (Exception ex)
            {
                // todo: log exception
                return null;
            }

            return list; 
        }

        public async Task<Recipe> GetRecipeDetailByIdAsync(int id)
        {
            var model = new Recipe(); 
            var dataSet = new DataSet();

            try
            {
                await _connection.OpenAsync().ContinueWith((task) =>
                {
                    var command = new SqlCommand("sp_GetRecipeDetailById", _connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@RecipeId", SqlDbType.Int).Value = id;
                    var adapter = new SqlDataAdapter(command);

                    adapter.Fill(dataSet);
                    
                    var column = dataSet.Tables[0].Rows[0][0].ToString();

                    if (!string.IsNullOrEmpty(column))
                    {
                        model.RecipeID = id;
                        model.RecipeName = dataSet.Tables[0].Rows[0][0].ToString(); 
                        model.Ingredients = dataSet.Tables[0].Rows[0][1].ToString();
                        model.Instructions = dataSet.Tables[0].Rows[0][2].ToString();
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            catch (Exception ex)
            {
                // todo: log exception
                return null;
            }

            return model;
        }

        public async Task<Recipe> AddRecipeAsync(Recipe model)
        {
            try
            {
                await _connection.OpenAsync().ContinueWith((task) => {
                    var command = new SqlCommand("sp_InsertRecipe", _connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@RecipeName", SqlDbType.VarChar, 100, "RecipeName")).Value = model.RecipeName;
                    command.Parameters.Add(new SqlParameter("@Ingredients", SqlDbType.VarChar, 500, "Ingredients")).Value = model.Ingredients;
                    command.Parameters.Add(new SqlParameter("@Instructions", SqlDbType.VarChar, 8000, "Instructions")).Value = model.Instructions;
                    command.Parameters.Add("@RecipeID", SqlDbType.Int, 0, "RecipeID").Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    model.RecipeID = (int)command.Parameters["@RecipeID"].Value;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            catch (Exception ex)
            {
                // todo: log exception
                return null;
            }

            return model;
        }

        public async Task<int> UpdateRecipeAsync(Recipe model)
        {
            var result = 0;

            try
            {
                await _connection.OpenAsync().ContinueWith((task) => {
                    var command = new SqlCommand("sp_UpdateRecipe", _connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@RecipeID", SqlDbType.Int, 0, "RecipeID")).Value = model.RecipeID;
                    command.Parameters.Add(new SqlParameter("@RecipeName", SqlDbType.VarChar, 100, "RecipeName")).Value = model.RecipeName;
                    command.Parameters.Add(new SqlParameter("@Ingredients", SqlDbType.VarChar, 500, "Ingredients")).Value = model.Ingredients;
                    command.Parameters.Add(new SqlParameter("@Instructions", SqlDbType.VarChar, 8000, "Instructions")).Value = model.Instructions;
                    result = command.ExecuteNonQuery();
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            catch (Exception ex)
            {
                // todo: log exception
                return -1;
            }

            return result;
        }

        public async Task<int> DeleteRecipeAsync(int id)
        {
            var result = 0;

            try
            {
                await _connection.OpenAsync().ContinueWith((task) => {
                    var command = new SqlCommand("sp_DeleteRecipe", _connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@RecipeID", SqlDbType.Int, 0, "RecipeID")).Value = id;
                    result = command.ExecuteNonQuery();
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
            catch (Exception ex)
            {
                // todo: log exception
                return -1;
            }

            return result;
        }
    }
}
