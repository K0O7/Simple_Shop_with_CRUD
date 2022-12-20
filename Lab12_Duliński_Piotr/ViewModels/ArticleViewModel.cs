using Lab12_Duliński_Piotr.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab12_Duliński_Piotr.ViewModels
{
    
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength (2, ErrorMessage ="too short name")]
        [Display(Name ="Product name")]
        [MaxLength (20, ErrorMessage ="too long name, do not exceed {1}")]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public int Price { get; set; }
        public string Image { get; set; }

        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile FormFile { get; set; }
    }
}
