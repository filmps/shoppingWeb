// User.cs
using System;
using System.ComponentModel.DataAnnotations;
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string UserAddress { get; set; }
    public string UserPhone { get; set; }
    // public List<Order> Order { get; set; } = new List<Order>();

}