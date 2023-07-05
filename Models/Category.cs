using System.ComponentModel.DataAnnotations;

namespace RecipeApp1.Models
{
    public class Category
    {

       // [Key]
       // [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<Recipe>? Recipe { get; set; }
    }
}
