using System.ComponentModel.DataAnnotations;

namespace RecipeApp1.Models
{
    public class Review
    {
        //[Key]
        //[Required]
        public int Id { get; set; }

        //[Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        //[Required]
        [MaxLength(30)]
        public string AppUser { get; set; }

        //[Required]
        public string Comment { get; set; }
        //[Required]
        public int Rating { get; set; }

    }
}
