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

        [HttpGet]
        [Route("getallrecipes")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Recipe>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<IdValuePair>>> GetAllRecipesAsync()
        {
            return await _recipeService.GetAllRecipesAsync();

            // todo: if null then return something or it'll throw an error
        }

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

            if (model != null)
            {
                return model;
            }

            return NotFound();
        }

        [HttpPost]
        [Route("addrecipe")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> AddRecipeAsync([FromBody] Recipe model)
        {
            if(model == null)
                return BadRequest();

            var result = await _recipeService.AddRecipeAsync(model);

            return CreatedAtAction(nameof(GetRecipeByIdAsync), new { id = model.RecipeID }, model);
        }

        [HttpPut]
        [Route("updaterecipe")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateRecipeAsync([FromBody] Recipe model)
        {
            if (model == null)
                return NotFound(new { Message = $"Recipe with ID {model.RecipeID} not found." });

            var result = await _recipeService.UpdateRecipeAsync(model);

            if(result <= 0)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetRecipeByIdAsync), new { id = model.RecipeID }, model);
        }

        [HttpDelete("{id}")]
        [Route("deleterecipe/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteRecipeAsync([FromQuery] int id)
        {
            if (id <= 0)
                return NotFound();

            var result = await _recipeService.DeleteRecipeAsync(id);

            if (result <= 0)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
