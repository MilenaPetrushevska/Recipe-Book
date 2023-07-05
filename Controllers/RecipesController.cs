using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeApp1.Data;
using RecipeApp1.Models;
using RecipeApp1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RecipeApp1.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
       

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        // GET: Recipes
        public async Task<IActionResult> Index(string category)
        {
            IQueryable<Recipe> recipes = _context.Recipe.Include(r => r.Category).Include(r => r.Reviews);

            if (!string.IsNullOrEmpty(category))
            {
                recipes = recipes.Where(r => r.Category.Name.Equals(category));
            }

            var recipeList = await recipes.ToListAsync();
            return View(recipeList);
        }
        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(b => b.Category).Include(b => b.Reviews).Include(p => p.UserRecipe)
                .Include(b => b.Ingredient).ThenInclude(b => b.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var ingr = _context.Ingredient.AsEnumerable();
            ingr = ingr.OrderBy(s => s.Name);

            IngrRecEditViewModel viewmodel = new IngrRecEditViewModel
            {
                IngrList = new MultiSelectList(ingr, "Id", "Name")
            };
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View(viewmodel);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Description,Images,DownloadUrl,CookTime")] Recipe recipe)
        {
            if (ModelState.IsValid )
            {
                _context.Add(recipe);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

                
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", recipe.CategoryId);
            return View();
        }

        // GET: Recipes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = _context.Recipe.Where(b => b.Id == id).Include(b => b.Ingredient).First();
            if (recipe == null)
            {
                return NotFound();
            }
            var ingr = _context.Ingredient.AsEnumerable();
            ingr = ingr.OrderBy(s => s.Name);

            IngrRecEditViewModel viewmodel = new IngrRecEditViewModel
            {
                Recipe =    recipe,
                IngrList = new MultiSelectList(ingr, "Id", "Name"),
                SelectedIngr = recipe.Ingredient.Select(s => s.IngredientId)
            };
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", recipe.CategoryId);
            return View(viewmodel);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IngrRecEditViewModel viewmodel)
        {
            if (id != viewmodel.Recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Recipe);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> newIngrList = viewmodel.SelectedIngr;
                    IEnumerable<int> prevIngrList = _context.IngrRec.Where(s => s.RecipeId == id).Select(s => s.IngredientId);
                    IQueryable<IngrRec> toBeRemovedIngr = _context.IngrRec.Where(s => s.RecipeId == id);
                    if (newIngrList != null)
                    {
                        toBeRemovedIngr = toBeRemovedIngr.Where(s => !newIngrList.Contains(s.IngredientId));
                        foreach (int ingrId in newIngrList)
                        {
                            if (!prevIngrList.Any(s => s == ingrId))
                            {
                                _context.IngrRec.Add(new IngrRec { IngredientId = ingrId, RecipeId = id });
                            }
                        }
                    }
                    _context.IngrRec.RemoveRange(toBeRemovedIngr);
                    
                    _context.Update(viewmodel.Recipe);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                    if (!RecipeExists(viewmodel.Recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", viewmodel.Recipe.CategoryId);
            return View(viewmodel);
        }

        // GET: Recipes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipe == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Recipe'  is null.");
            }
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
          return (_context.Recipe?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
