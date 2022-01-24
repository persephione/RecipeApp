using Microsoft.AspNetCore.Mvc;
using RecipeApp.WebMVC.Services;
using RecipeApp.WebMVC.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RecipeApp.WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecipeService _recipeService;

        public HomeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _recipeService.GetRecipes();

            // todo: add message for null

            return View(list);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Recipe model)
        {
            var created = await _recipeService.AddRecipe(model);

            if(!created)
            {
                // todo: show error message
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                // todo: show error message for bad request
            }

            var model = await _recipeService.GetRecipeById(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Recipe model)
        {
            var updated = await _recipeService.UpdateRecipe(model);

            if (!updated)
            {
                // todo: show error message
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
