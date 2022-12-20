using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab12_Duliński_Piotr.Data;
using Lab12_Duliński_Piotr.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab12_Duliński_Piotr.Pages.Shop
{
    public class IndexModel : PageModel
    {
        private readonly Lab12_Duliński_Piotr.Data.MyDbContext _context;

        public IndexModel(Lab12_Duliński_Piotr.Data.MyDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["Category"] = new SelectList(_context.Category, "CategoryId", "Categoryname");
            Article = await _context.Article.Include(a => a.Category).ToListAsync();
        }

        [BindProperty]
        public string searchString { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewData["Category"] = new SelectList(_context.Category, "CategoryId", "Categoryname", Int32.Parse(searchString));
                Article = await _context.Article
                    .Where(a => a.CategoryId.ToString().Equals(searchString))
                    .Include(a => a.Category).ToListAsync();
                return Page();
                //return View(await myDbContextFiltered.ToListAsync());
            }
            else
            {
                Article = await _context.Article.Include(a => a.Category).ToListAsync();
                return Page();
            }
        }
    }
}
