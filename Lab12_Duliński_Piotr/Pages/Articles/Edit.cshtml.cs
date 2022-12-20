using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab12_Duliński_Piotr.Data;
using Lab12_Duliński_Piotr.ViewModels;

namespace Lab12_Duliński_Piotr.Pages.Articles
{
    public class EditModel : PageModel
    {
        private readonly Lab12_Duliński_Piotr.Data.MyDbContext _context;

        public EditModel(Lab12_Duliński_Piotr.Data.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Article Article { get; set; }
        [BindProperty]
        public int oldCategoryId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article
                .Include(a => a.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (Article == null)
            {
                return NotFound();
            }

            //oldCategoryId = Article.CategoryId;
            ViewData["oldCategory"] = Article.CategoryId;
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Categoryname");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                ViewData["Category"] = new SelectList(_context.Category, "CategoryId", "Categoryname", Article.CategoryId);
                return Page();
            }

            foreach (var cat in _context.Category)
            {
                if (cat.CategoryId == Article.CategoryId)
                    Article.Category = cat;
            }
            try
            {

                var category = await _context.Category.FindAsync(Article.CategoryId);
                var oldCategory = await _context.Category.FindAsync(oldCategoryId);
                try
                {
                    oldCategory.Counter--;
                    category.Counter++;
                    _context.Update(category);
                    _context.Update(oldCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId) || !CategoryExists(oldCategory.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                _context.Update(Article);
                _context.Attach(Article).State = EntityState.Modified;
                _context.Entry(Article).Property(x => x.Image).IsModified = false;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int categoryId)
        {
            return _context.Category.Any(e => e.CategoryId == categoryId);
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
