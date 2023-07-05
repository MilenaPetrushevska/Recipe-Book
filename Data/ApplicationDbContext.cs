using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeApp1.Areas.Identity.Data;
using RecipeApp1.Models;

namespace RecipeApp1.Data
{
    public class ApplicationDbContext : IdentityDbContext<RecipeApp1User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RecipeApp1.Models.Category>? Category { get; set; }
        public DbSet<RecipeApp1.Models.Recipe>? Recipe { get; set; }
        public DbSet<RecipeApp1.Models.Ingredient>? Ingredient { get; set; }
        public DbSet<RecipeApp1.Models.Review>? Review { get; set; }
        public DbSet<RecipeApp1.Models.UserRecipe>? UserRecipe { get; set; }
        public DbSet<RecipeApp1.Models.IngrRec>? IngrRec { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}