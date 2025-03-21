using Microsoft.AspNetCore.Identity;

namespace BuyLocalApp.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}