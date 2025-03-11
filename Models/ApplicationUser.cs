using Microsoft.AspNetCore.Identity;

namespace BuyLocalApp.Models;

public class ApplicationUser : IdentityUser
{
    private int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}