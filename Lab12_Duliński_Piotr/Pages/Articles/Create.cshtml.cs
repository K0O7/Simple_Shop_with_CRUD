using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab12_Duliński_Piotr.Data;
using Lab12_Duliński_Piotr.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Lab12_Duliński_Piotr.Pages.Articles
{
    public class CreateModel : PageModel
    {
        private readonly Lab12_Duliński_Piotr.Data.MyDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public IFormFile FormFile { get; set; }

        public CreateModel(Lab12_Duliński_Piotr.Data.MyDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Categoryname");
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //Article.FormFile = PicPath;
            if (!ModelState.IsValid)
            {
                ViewData["Category"] = new SelectList(_context.Category, "CategoryId", "Categoryname", Article.CategoryId);
                return Page();
            }

            string folder = "";
            if (Article.FormFile == null)
                folder = "/image/noImage.png";
            else
            {
                folder = "upload/" + Guid.NewGuid().ToString() + "_" + Article.FormFile.FileName;
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, folder);
                var fileStream = new FileStream(uploadFolder, FileMode.Create);
                Article.FormFile.CopyTo(fileStream);
                fileStream.Close();
                folder = "/" + folder;
            }
            Article.Image = folder;
            foreach (var cat in _context.Category)
            {
                if (cat.CategoryId == Article.CategoryId)
                    Article.Category = cat;
            }
            var category = await _context.Category.FindAsync(Article.CategoryId);
            try
            {
                category.Counter++;
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

            _context.Article.Add(Article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int categoryId)
        {
            return _context.Category.Any(e => e.CategoryId == categoryId);
        }
    }
}
