using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeApp1.Models;

namespace RecipeApp1.ViewModels
{
    public class IngrRecEditViewModel
    {
        //public IFormFile url { get; set; }
        public Recipe Recipe { get; set; }

        public IEnumerable<int>? SelectedIngr { get; set; }
        public IEnumerable<SelectListItem>? IngrList { get; set; }
    }
}
