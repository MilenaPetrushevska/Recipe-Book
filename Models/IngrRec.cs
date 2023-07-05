using System.ComponentModel.DataAnnotations;

namespace RecipeApp1.Models
{
    public class IngrRec
    {
        //[Key]
        //[Required]
        public int Id { get; set; }

        //[Required]
        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }

        //[Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
