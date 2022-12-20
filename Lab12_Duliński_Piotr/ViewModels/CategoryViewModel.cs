using Lab12_Duliński_Piotr.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab12_Duliński_Piotr.ViewModels
{
    public class Category
    {
        [Key]
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "too long name, do not exceed {1}")]
        public string Categoryname { get; set; }
        public int Counter { get; set; }
    }
}
