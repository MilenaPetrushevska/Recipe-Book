using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApp1.Areas.Identity.Data;
using RecipeApp1.Data;

namespace RecipeApp1.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<RecipeApp1User>>();
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            RecipeApp1User user = await UserManager.FindByEmailAsync("admin@recipe.com");
            if (user == null)
            {
                var User = new RecipeApp1User();
                User.Email = "admin@recipe.com";
                User.UserName = "admin@recipe.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            var roleCheck1 = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck1) { roleResult = await RoleManager.CreateAsync(new IdentityRole("User")); }
            RecipeApp1User user1 = await UserManager.FindByEmailAsync("defuser@recipe.com");
            if (user1 == null)
            {
                var User = new RecipeApp1User();
                User.Email = "user@recipe.com";
                User.UserName = "user@recipe.com";
                string userPWD = "Userr123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(User, "User");
                }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<ApplicationDbContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                if (context == null || context.Category == null || context.Ingredient == null || context.IngrRec == null || context.Recipe == null || context.Review == null)
                {
                    throw new ArgumentNullException("Null ApplicationDbContext");
                }
                if (context.Category.Any() || context.Ingredient.Any() || context.IngrRec.Any() || context.Recipe.Any() || context.Review.Any())
                {
                    return; // DB has been seeded
                }
                
                var categories = new List<Category>
                {
                    new Category { Name = "Desserts" },
                    new Category { Name = "Main Meal" },
                    new Category { Name = "Appetizers" },
                    new Category { Name = "Salads" },
                    new Category { Name = "Breakfast" }
                };
                context.Category.AddRange(categories);
                context.SaveChanges();

                // Create ingredients
                var ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Flour", Description = "All-purpose flour" },
                    new Ingredient { Name = "Sugar", Description = "Granulated sugar" },
                    new Ingredient { Name = "Eggs", Description = "Large eggs" },
                    new Ingredient { Name = "Milk", Description = "Whole milk" },
                    new Ingredient { Name = "Salt", Description = "Table salt" },
                    new Ingredient { Name = "Butter", Description = "Unsalted butter" }
                };
                context.Ingredient.AddRange(ingredients);
                context.SaveChanges();

                // Create recipes
                var recipes = new List<Recipe>
                {
                    new Recipe { CategoryId = categories[0].Id, Title = "Chocolate Cake", CookTime = 60, Description = "A rich and moist chocolate cake.",DownloadUrl="Preheat the oven to 350°F (175°C). Grease and flour two 9-inch round cake pans.\n In a large mixing bowl, combine the flour, baking powder, baking soda, cocoa powder, and sugar.\r\nAdd the eggs, milk, vegetable oil, and vanilla extract to the dry ingredients. Mix until well combined.\r\nGradually add the boiling water to the batter, mixing well. The batter will be thin.\r\nPour the batter evenly into the prepared cake pans.\r\nBake for 30-35 minutes or until a toothpick inserted into the center comes out clean.\r\nRemove from the oven and let the cakes cool in the pans for 10 minutes. Then transfer them to a wire rack to cool completely.\r\nFrost the cake with your favorite chocolate frosting or ganache. Serve and enjoy!", Images = "https://sallysbakingaddiction.com/wp-content/uploads/2013/04/triple-chocolate-cake-4.jpg" },
                    new Recipe { CategoryId = categories[1].Id, Title = "Spaghetti Bolognese", CookTime = 30, Description = "Classic Italian pasta dish with meat sauce.",DownloadUrl="Cook the spaghetti according to the package instructions. Drain and set aside.\r\nIn a large skillet, cook the ground beef over medium heat until browned. Add the onion, garlic, and carrot, and cook until the vegetables are softened.\r\nStir in the crushed tomatoes, tomato paste, dried oregano, salt, and pepper. Simmer for about 20 minutes, allowing the flavors to blend.\r\nServe the Bolognese sauce over the cooked spaghetti. Sprinkle with grated Parmesan cheese, if desired.", Images = "https://www.recipetineats.com/wp-content/uploads/2018/07/Spaghetti-Bolognese.jpg?w=500&h=500&crop=1" },
                    new Recipe { CategoryId = categories[2].Id, Title = "Guacamole", CookTime = 15, Description = "Fresh and flavorful avocado dip.",DownloadUrl="Cut the avocados in half, remove the pits, and scoop out the flesh into a bowl.\r\nMash the avocado with a fork until it reaches your desired consistency (chunky or smooth).\r\nAdd the diced onion, tomato, jalapeño pepper (if using), lime juice, and chopped cilantro. Mix well.\r\nSeason with salt and pepper to taste. Adjust the flavors by adding more lime juice or cilantro, if desired.\r\nServe the guacamole with tortilla chips, tacos, or as a topping for burgers or sandwiches.", Images = "https://i2.wp.com/www.downshiftology.com/wp-content/uploads/2019/04/Guacamole-1-2.jpg" }
                };

                context.Recipe.AddRange(recipes);
                context.SaveChanges();

                // Create ingredient-recipe associations
                var ingredientRecipes = new List<IngrRec>
                {
                    new IngrRec { IngredientId = ingredients[0].Id, RecipeId = recipes[0].Id },
                    new IngrRec { IngredientId = ingredients[1].Id, RecipeId = recipes[0].Id },
                    new IngrRec { IngredientId = ingredients[2].Id, RecipeId = recipes[1].Id },
                    new IngrRec { IngredientId = ingredients[3].Id, RecipeId = recipes[1].Id },
                    new IngrRec { IngredientId = ingredients[4].Id, RecipeId = recipes[2].Id },
                    new IngrRec { IngredientId = ingredients[5].Id, RecipeId = recipes[2].Id }
                };
                context.IngrRec.AddRange(ingredientRecipes);
                context.SaveChanges();

                // Create reviews
                var reviews = new List<Review>
                {
                    new Review { RecipeId = recipes[0].Id, AppUser = "Milena", Comment = "Delicious cake!", Rating = 5 },
                    new Review { RecipeId = recipes[1].Id, AppUser = "Sara", Comment = "Great pasta dish.", Rating = 4 },
                    new Review { RecipeId = recipes[2].Id, AppUser = "Mila", Comment = "Best guacamole ever.", Rating = 5 }
                };
                context.Review.AddRange(reviews);
                context.SaveChanges();
            }
        }
    }
}
