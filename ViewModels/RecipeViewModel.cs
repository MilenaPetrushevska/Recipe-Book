using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeApp1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace RecipeApp1.ViewModels
{
    public class RecipeViewModel
    {
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Image")]
        public string? Images { get; set; }

        [Display(Name = "Download Url")]
        public IFormFile DownloadUrll { get; set; }

        [Required]
        public int CookTime { get; set; }

        //public IFormFile url { get; set; }
       // public Recipe? Recipe { get; set; }

        public ICollection<IngrRec>? Ingredient { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserRecipe>? UserRecipe { get; set; }
        public IEnumerable<int>? SelectedIngr { get; set; }
        public IEnumerable<SelectListItem>? IngrList { get; set; }
        /*
        [NotMapped]

        [Display(Name = "Average Rating")]
        public double Prosek
        {
            get
            {
                if (Reviews == null)
                    return 0;

                double average = 0;
                int i = 0;
                if (Reviews != null)
                {
                    foreach (var review in Reviews)
                    {
                        average += review.Rating;
                        i++;
                    }

                    return Math.Round(average / i, 2);
                }
                return 0;
            }
        }*/
    }
}
