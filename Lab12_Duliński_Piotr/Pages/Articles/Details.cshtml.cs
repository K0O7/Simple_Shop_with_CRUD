using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab12_Duliński_Piotr.Data;
using Lab12_Duliński_Piotr.ViewModels;

namespace Lab12_Duliński_Piotr.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly Lab12_Duliński_Piotr.Data.MyDbContext _context;

        public DetailsModel(Lab12_Duliński_Piotr.Data.MyDbContext context)
        {
            _context = context;
        }

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
    }
}
