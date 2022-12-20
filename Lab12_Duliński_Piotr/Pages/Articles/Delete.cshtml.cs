using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab12_Duliński_Piotr.Data;
using Lab12_Duliński_Piotr.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace Lab12_Duliński_Piotr.Pages.Articles
{
    public class DeleteModel : PageModel
    {
        private readonly Lab12_Duliński_Piotr.Data.MyDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        public DeleteModel(Lab12_Duliński_Piotr.Data.MyDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public Article Article { get; set; }

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article.FindAsync(id);

            if (Article != null)
            {
                if (Article.Image != "/image/noImage.png")
                    System.IO.File.Delete(_hostingEnvironment.WebRootPath + Article.Image);

                var category = await _context.Category.FindAsync(Article.CategoryId);
                try
                {
                    category.Counter--;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _context.Article.Remove(Article);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int categoryId)
        {
            return _context.Category.Any(e => e.CategoryId == categoryId);
        }
    }
}
