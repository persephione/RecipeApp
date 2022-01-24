using Microsoft.Extensions.Configuration;
using RecipeApp.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RecipeApp.WebMVC.Infrastructure;
using Newtonsoft.Json;

namespace RecipeApp.WebMVC.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _apiClient;
        private readonly string _apiRecipeUrl;

        public RecipeService(HttpClient httpClient, IConfiguration config)
        {
            _apiClient = httpClient;
            _config = config;

            var baseUrl = _config.GetValue<string>("RecipeUrl");
            _apiRecipeUrl = $"{baseUrl}/api/recipe";
        }

        public async Task<List<IdValuePair>> GetRecipes()
        {
            var url = API.GetRecipes(_apiRecipeUrl);
            
            var response = await _apiClient.GetAsync(url);
           
            var responseString = await response.Content.ReadAsStringAsync();

            return string.IsNullOrEmpty(responseString) 
                ? null
                : JsonConvert.DeserializeObject<List<IdValuePair>>(responseString);
        }

        public async Task<Recipe> GetRecipeById(int recipeId)
        {
            var url = API.GetRecipeById(_apiRecipeUrl, recipeId);
            
            var response = await _apiClient.GetAsync(url);
            
            var responseString = await response.Content.ReadAsStringAsync();
            
            return string.IsNullOrEmpty(responseString) 
                ? null 
                : JsonConvert.DeserializeObject<Recipe>(responseString);
        }

        public async Task<bool> AddRecipe(Recipe recipe)
        {
            try
            {
                string jsonResult = string.Empty;

                var uri = API.AddRecipe(_apiRecipeUrl);

                var recipeContent = new StringContent(JsonConvert.SerializeObject(recipe), System.Text.Encoding.UTF8, "application/json");

                var responseMessage = await _apiClient.PostAsync(uri, recipeContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync();

                    var result = string.IsNullOrEmpty(jsonResult) ?
                        new Recipe() { RecipeID = 0 } :
                        JsonConvert.DeserializeObject<Recipe>(jsonResult);

                    if (result.RecipeID <= 0)
                        return false;

                    return true;
                }
                else
                {
                    // status code 400 bad request
                    // todo: log error
                    return false;
                }
            }
            catch (Exception ex)
            {
                // todo: log error
                return false;
            }
        }

        public async Task<bool> UpdateRecipe(Recipe recipe)
        {
            try
            {
                string jsonResult = string.Empty;

                var url = API.UpdateRecipe(_apiRecipeUrl);

                var recipeContent = new StringContent(JsonConvert.SerializeObject(recipe), System.Text.Encoding.UTF8, "application/json");

                var responseMessage = await _apiClient.PutAsync(url, recipeContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // todo: log error
                    return false;
                }
            }
            catch (Exception ex)
            {
                // todo: log error
                return false;
            }
        }

    }
}
