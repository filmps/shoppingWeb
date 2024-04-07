using Microsoft.AspNetCore.Identity;

namespace OnlineWeb.Models
{
    public class Order
{
    public int OrderId { get;set;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? CompanyName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public bool CreateAccount { get; set; }
    public bool ShipDifferentAddress { get; set; }
    
     public string? UserId { get; set; }
     public int CartId { get; set;}
     public IdentityUser? User { get; set; }

      public  Cart? Cart { get; set; }
}

}