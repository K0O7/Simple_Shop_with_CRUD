using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab12_Duliński_Piotr.Data;
using Lab12_Duliński_Piotr.ViewModels;

namespace Lab12_Duliński_Piotr.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly Lab12_Duliński_Piotr.Data.MyDbContext _context;

        public CreateModel(Lab12_Duliński_Piotr.Data.MyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Category.Counter = 0;
            _context.Category.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
