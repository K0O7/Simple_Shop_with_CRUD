using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab12_Duliński_Piotr.Data;
using Lab12_Duliński_Piotr.ViewModels;

namespace Lab12_Duliński_Piotr.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly Lab12_Duliński_Piotr.Data.MyDbContext _context;

        public DetailsModel(Lab12_Duliński_Piotr.Data.MyDbContext context)
        {
            _context = context;
        }

        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
