using System.ComponentModel.DataAnnotations;

namespace RecipeApp1.Models
{
    public class Recipe
    {
        //[Key]
        //[Required]
        public int Id { get; set; }
       
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Image")]
        public string? Images { get; set; }
        [Display(Name = "Recipe-Making Process")]
        public string? DownloadUrl { get; set; }

        public int CookTime { get; set; }

        public ICollection<IngrRec>? Ingredient { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserRecipe>? UserRecipe { get; set; }
    }
    
}
