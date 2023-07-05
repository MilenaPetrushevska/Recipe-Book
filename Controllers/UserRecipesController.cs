using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeApp1.Areas.Identity.Data;
using RecipeApp1.Data;
using RecipeApp1.Models;

namespace RecipeApp1.Controllers
{
    public class UserRecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<RecipeApp1User> _userManager;
        public UserRecipesController(ApplicationDbContext context, UserManager<RecipeApp1User> usermanager)
        {
            _context = context;
            _userManager= usermanager;
        }
        private Task<RecipeApp1User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: UserRecipes
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserRecipe.Include(u => u.Recipe);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddRecipeBought(int? recipeid)
        {
            if (recipeid == null)
            {
                return NotFound();
            }
            var applicationDbContext = _context.UserRecipe.Where(r => r.RecipeId == recipeid).Include(p => p.Recipe).ThenInclude(p => p.Category);
            var user = await GetCurrentUserAsync();

            if (ModelState.IsValid)
            {
                UserRecipe userrecipe = new UserRecipe();
                userrecipe.RecipeId = (int)recipeid;
                userrecipe.AppUser = user.UserName;
                _context.UserRecipe.Add(userrecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyRecipesList));
            }
            return applicationDbContext != null ?
                          View(await applicationDbContext.ToListAsync()) :
                          Problem("Entity set 'ApplcationDBContext.UserRecipe'  is null.");
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyRecipesList()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.UserRecipe.AsQueryable().Where(r => r.AppUser == user.UserName).Include(r => r.Recipe).ThenInclude(p => p.Category);
            var recipes_ofcurrentuser = _context.Recipe.AsQueryable(); ;
            recipes_ofcurrentuser = applicationDbContext.Select(p => p.Recipe);
            return applicationDbContext != null ?
                          View("~/Views/UserRecipes/RecipesBought.cshtml", await recipes_ofcurrentuser.ToListAsync()) :
                          Problem("Entity set 'ApplictionDbContext.UserRecipe'  is null.");
        }
        // GET: UserRecipes/Details/5
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRecipe == null)
            {
                return NotFound();
            }

            var userRecipe = await _context.UserRecipe
                .Include(u => u.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRecipe == null)
            {
                return NotFound();
            }

            return View(userRecipe);
        }

        // GET: UserRecipes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Title");
            return View();
        }

        // POST: UserRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,AppUser,RecipeId")] UserRecipe userRecipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Title", userRecipe.RecipeId);
            return View(userRecipe);
        }

        [Authorize(Roles = "Admin")]
        // GET: UserRecipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRecipe == null)
            {
                return NotFound();
            }

            var userRecipe = await _context.UserRecipe.FindAsync(id);
            if (userRecipe == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Title", userRecipe.RecipeId);
            return View(userRecipe);
        }

        // POST: UserRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUser,RecipeId")] UserRecipe userRecipe)
        {
            if (id != userRecipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRecipeExists(userRecipe.Id))
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
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Title", userRecipe.RecipeId);
            return View(userRecipe);
        }

        // GET: UserRecipes/Delete/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOwnedRecipes(int? recipeid)
        {
            if (recipeid == null || _context.UserRecipe == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var userRecipe = await _context.UserRecipe.Include(p => p.Recipe).AsQueryable().FirstOrDefaultAsync(m => m.AppUser == user.UserName && m.RecipeId == recipeid);
            if (userRecipe == null)
            {
                return NotFound();
            }

            return View("~/Views/UserRecipes/Delete.cshtml", userRecipe);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRecipe == null)
            {
                return NotFound();
            }

            var userRecipe = await _context.UserRecipe
                .Include(u => u.Recipe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRecipe == null)
            {
                return NotFound();
            }

            return View(userRecipe);
        }

        // POST: UserRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRecipe == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserRecipe'  is null.");
            }
            var userRecipe = await _context.UserRecipe.FindAsync(id);
            if (userRecipe != null)
            {
                _context.UserRecipe.Remove(userRecipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRecipeExists(int id)
        {
          return (_context.UserRecipe?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
