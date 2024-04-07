using Microsoft.EntityFrameworkCore;
using OnlineWeb.Models;
using System.ComponentModel.DataAnnotations; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlineWeb.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

     public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Order> Orders { get; set;}

    public DbSet<Feedback> Feedbacks { get; set; }


}

