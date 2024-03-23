using Microsoft.AspNetCore.Mvc;
using OnlineWeb.Data;
using OnlineWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using OnlineWeb.Models.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


namespace OnlineWeb.Controllers  // Replace "YourApplicationNamespace" with your project's actual namespace
{
    public class ProductController : Controller
    {
        // Action method for displaying the Add Product page

         private readonly ApplicationDbContext _context;
         private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(ApplicationDbContext context,IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
    }

[HttpGet]
public IActionResult AddProduct()
{
    // Initialize your ViewModel if needed
    var model = new ProductViewModel();
    return View(model);
}


[HttpPost]
public async Task<IActionResult> AddProduct(ProductViewModel model)
{
    if (ModelState.IsValid)
    {
        // Map the ViewModel to your domain model and save to database
        var product = new Product
        {
            ProductName = model.ProductName,
            Price = model.Price,
            Style = model.Style,
            Category = model.Category,
            Description = model.Description,
            ProductImageFiles = new List<ProductImage>()
        };
       Console.WriteLine("Log message: ok.");
        if (model.ProductImageFiles != null && model.ProductImageFiles.Count > 0)
        {
           Console.WriteLine("Log message: image.");
            foreach (var file in model.ProductImageFiles)
            {
                // Ensure the uploads directory exists
                var uploadsDir = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                var filePath = Path.Combine(uploadsDir, file.FileName);
                Console.WriteLine("Uploaded image saved to: " + filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                product.ProductImageFiles.Add(new ProductImage { ImageUrl = "/uploads/" + file.FileName, Product = product });
            }
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        Console.WriteLine(product.Id);
     
        // Redirect to the Shop or ProductDetail view as needed
         return RedirectToAction("Shop");
    }

    // If we got this far, something failed; redisplay the form
    
     return View(model);
}
    [HttpGet("Product/ProductDetails/{id}")]
    public async Task<IActionResult> ProductDetails(int id)
{
    var product = await _context.Products
        .Include(p => p.ProductImageFiles) // Assuming you want to include related images
        .FirstOrDefaultAsync(p => p.Id == id);

    if (product == null)
    {
        return NotFound();
    }

    var viewModel = new ProductViewModel
    {
        Id = product.Id,
        ProductName = product.ProductName,
        Price = product.Price,
        Description = product.Description,
        Style = product.Style,
        Category = product.Category,
        ImageUrls = product.ProductImageFiles.Select(img => img.ImageUrl).ToList()
    };

    return View(viewModel); // Pass the viewModel here
}


       [HttpGet]
   public async Task<IActionResult> Shop(int page = 1)
{
    const int pageSize = 8; // Items per page
    var totalItems = await _context.Products.CountAsync(); // Total number of items
    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Total pages

    var products = await _context.Products
        .OrderBy(p => p.Id) // Or any other ordering
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductViewModel
        {
            Id = p.Id,
            ProductName = p.ProductName,
            Price = p.Price,
            Style = p.Style,
            Category = p.Category,
            Description = p.Description,
            ImageUrls = p.ProductImageFiles.Select(img => img.ImageUrl).ToList()
        })
        .ToListAsync();

    ViewBag.TotalPages = totalPages;
    ViewBag.CurrentPage = page;

    return View("~/Views/Shop/Shop.cshtml", products);
}

  // GET: Product/Edit/5

    public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var product = await _context.Products.FindAsync(id);
    if (product == null)
    {
        return NotFound();
    }

    var viewModel = new ProductViewModel
    {
        Id = product.Id,
        ProductName = product.ProductName,
        Price = product.Price,
        Style = product.Style,
        Category = product.Category,
        Description = product.Description,
        ImageUrls = product.ProductImageFiles.Select(img => img.ImageUrl).ToList()
    };

    return View("EditProduct", viewModel); // Reuse the AddProduct view for editing
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(ProductViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        Console.WriteLine($"Processing {viewModel.ProductImageFiles.Count} files");

        var product = await _context.Products.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == viewModel.Id);
        if (product == null)
        {
            return NotFound();
        }

        // Update product properties as before
        product.ProductName = viewModel.ProductName;
        product.ProductName = viewModel.ProductName;
        product.Price = viewModel.Price;
        product.Style = viewModel.Style;
        product.Category = viewModel.Category;
        product.Description = viewModel.Description;

        if (viewModel.ProductImageFiles != null && viewModel.ProductImageFiles.Count > 0)
        {
            // Assuming a method to handle file saving and URL generation:

              Console.WriteLine($"Uploaded image count: {viewModel.ProductImageFiles.Count}");
            List<ProductImage> uploadedImages = await SaveUploadedFiles(viewModel.ProductImageFiles);

            foreach (var image in uploadedImages)
        {
            Console.WriteLine($"Uploaded image URL: {image.ImageUrl}");
        }
        
            product.ProductImageFiles = uploadedImages; // Replace existing images with the new ones
        }

        

        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(e => e.Id == viewModel.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(ProductDetails), new { id = viewModel.Id });
    }

    return View(viewModel);
}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Products.FindAsync(id);
            if (item != null)
            {
                _context.Products.Remove(item);
                await _context.SaveChangesAsync();
                // Redirect to a different action/controller as needed
               return RedirectToAction("Shop", "Product");
            }
            return NotFound();
        }

    public async Task<IActionResult> ShopCategory(string category)
    {
        IEnumerable<ProductViewModel> model;

        if (!string.IsNullOrEmpty(category))
        {
            
            var lowerCategory = category.ToLower();
            model = await _context.Products
                            .Where(p => p.Category.ToLower() == lowerCategory) 
                            .Select(p => new ProductViewModel
                            {
                                Id = p.Id,
                                ProductName = p.ProductName,
                                Price = p.Price,
                                Style = p.Style,
                                Category = p.Category,
                                Description = p.Description,
                                // Assuming you have a way to convert product images to URLs
                                ImageUrls = p.ProductImageFiles.Select(img => img.ImageUrl).ToList()
                            })
                            .ToListAsync();
        }
        else
        {
            model = await _context.Products
                            .Select(p => new ProductViewModel
                            {
                                // Similar mapping as above
                            })
                            .ToListAsync();
        }

        return View(model); // Pass the model to the view
    }
private async Task<List<ProductImage>> SaveUploadedFiles(List<IFormFile> files)
{
    var uploadedImages = new List<ProductImage>();
    string uploadsDir = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
    foreach (var file in files)
    {
        Console.WriteLine("Processing file: " + file.FileName);
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        string filePath = Path.Combine(uploadsDir, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        
        string imageUrl = "/uploads/" + uniqueFileName; // Adjust as necessary
        uploadedImages.Add(new ProductImage { ImageUrl = imageUrl });
    }

    return uploadedImages;
}

    }

}
