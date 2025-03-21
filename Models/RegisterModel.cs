using System.ComponentModel.DataAnnotations;
namespace BuyLocalApp.Models;
public class RegisterModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }
    }