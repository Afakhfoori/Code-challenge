using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace BunningsCataloge.Models
{
    public class ImportFileModel
    {
        [Required]
        public IFormFile SuppliersA { get; set; }
        [Required]
        public IFormFile SuppliersB { get; set; }
        [Required]
        public IFormFile CatalogA { get; set; }
        [Required]
        public IFormFile CatalogB { get; set; }
        [Required]
        public IFormFile BarcodeA { get; set; }
        [Required]
        public IFormFile BarcodeB { get; set; }

    }
}
