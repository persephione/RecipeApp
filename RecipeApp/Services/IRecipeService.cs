using RecipeApp.WebMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeApp.WebMVC.Services
{
    public interface IRecipeService
    {
        Task<List<IdValuePair>> GetRecipes();
        Task<Recipe> GetRecipeById(int recipeId);
        Task<bool> AddRecipe(Recipe recipe);
        Task<bool> UpdateRecipe(Recipe recipe);
    }
}
