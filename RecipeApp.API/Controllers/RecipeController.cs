using Microsoft.AspNetCore.Mvc;
using RecipeApp.API.Models;
using RecipeApp.API.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RecipeApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        /// <summary>
        /// Gets all recipes from db
        /// </summary>
        /// <returns>List of IDValue pairs for front end list view</returns>
        [HttpGet]
        [Route("getallrecipes")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Recipe>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<IdValuePair>>> GetAllRecipesAsync()
        {
            return await _recipeService.GetAllRecipesAsync();
        }

        /// <summary>
        /// Gets a recipe by RecipeID prop
        /// </summary>
        /// <param name="id">RecipeID</param>
        /// <returns>Full Recipe object</returns>
        [HttpGet("{id}")]
        [Route("getrecipebyid/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Recipe), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Recipe>> GetRecipeByIdAsync([FromQuery] int id)
        {
            if (id <= 0)
                return BadRequest();

            var model = await _recipeService.GetRecipeDetailByIdAsync(id);

            if (model == null)
                return NotFound(new { Message = $"Item with RecipeID {id} not found." });

            return model;
        }

        /// <summary>
        /// Creates new recipe obj in db
        /// </summary>
        /// <param name="model">Recipe model</param>
        /// <returns>New recipe db obj with newly assigned Identity and a 204 status</returns>
        [HttpPost]
        [Route("addrecipe")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> AddRecipeAsync([FromBody] Recipe model)
        {
            if(model == null)
                return BadRequest();

            model = await _recipeService.AddRecipeAsync(model);

            return CreatedAtAction(nameof(GetRecipeByIdAsync), new { id = model.RecipeID }, model);
        }

        /// <summary>
        /// Updates recipe obj in db
        /// </summary>
        /// <param name="model">Recipe model</param>
        /// <returns>Returns updated obj with 204 status</returns>
        [HttpPut]
        [Route("updaterecipe")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateRecipeAsync([FromBody] Recipe model)
        {
            if (model == null)
                return BadRequest();

            var result = await _recipeService.UpdateRecipeAsync(model);

            if(result <= 0)
                return NotFound(new { Message = $"Recipe with ID {model.RecipeID} not found." });

            return CreatedAtAction(nameof(GetRecipeByIdAsync), new { id = model.RecipeID }, model);
        }

        /// <summary>
        /// Deletes recipe obj from db
        /// </summary>
        /// <param name="id">RecipeID</param>
        /// <returns>No content if no error occurs and 204 status</returns>
        [HttpDelete("{id}")]
        [Route("deleterecipe/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteRecipeAsync([FromQuery] int id)
        {
            if (id <= 0)
                return NotFound(new { Message = $"Item with RecipeID {id} not found." });

            var result = await _recipeService.DeleteRecipeAsync(id);

            if (result <= 0)
                return NotFound(new { Message = $"Item with RecipeID {id} not found." });

            return NoContent();
        }

    }
}
