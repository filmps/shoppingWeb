using Microsoft.EntityFrameworkCore;
using OnlineWeb.Models;
using System.ComponentModel.DataAnnotations; 

namespace OnlineWeb.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

     public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Feedback> Feedbacks { get; set; }


}

