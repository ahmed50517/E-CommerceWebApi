﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double Price { get; set; }

        public bool IsAvailable { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        
        public Category Category { get; set; }

       
        
    }
}
