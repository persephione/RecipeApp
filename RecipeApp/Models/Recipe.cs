using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RecipeApp.WebMVC.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(255)]
        [Display(Name = "Name of Recipe")]
        public string RecipeName { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(1000)]
        public string Ingredients { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(4000)]
        public string Instructions { get; set; }
    }
}
