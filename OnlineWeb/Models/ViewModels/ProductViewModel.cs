using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineWeb.Models.ViewModels // Adjust the namespace to match your project's structure
{
    public class ProductViewModel
    {
        
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Product name is required")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public float Price { get; set; }

        // Assuming style and category are optional; adjust based on your requirements
        [Display(Name = "Style")]
        public string Style { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();


        [Display(Name = "Product Images")]
        public List<IFormFile> ProductImageFiles { get; set; }

        // Additional fields as needed based on your form
    }
}
