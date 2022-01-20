using RecipeApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeApp.API.Services
{
    public interface IRecipeService
    {
        Task<List<IdValuePair>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeDetailByIdAsync(int id);
        Task<Recipe> AddRecipeAsync(Recipe model);
        Task<int> UpdateRecipeAsync(Recipe model);
        Task<int> DeleteRecipeAsync(int id);
    }
}
