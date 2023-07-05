using System.ComponentModel.DataAnnotations;

namespace RecipeApp1.Models
{
    public class Ingredient
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<IngrRec>? Recipe { get; set; }
    }
}
