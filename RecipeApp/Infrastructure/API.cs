namespace RecipeApp.WebMVC.Infrastructure
{
    public static class API
    {
        public static string GetRecipes(string baseUrl) => $"{baseUrl}/getallrecipes";
        public static string GetRecipeById(string baseUrl, int RecipeId) => $"{baseUrl}/getrecipebyid?id={RecipeId}";
        public static string AddRecipe(string baseUrl) => $"{baseUrl}/addrecipe";
        public static string UpdateRecipe(string baseUrl) => $"{baseUrl}/updaterecipe";
    }
}
