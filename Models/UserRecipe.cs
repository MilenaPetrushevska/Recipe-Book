using System.ComponentModel.DataAnnotations;

namespace RecipeApp1.Models
{
    public class UserRecipe
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string AppUser { get; set; }
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
