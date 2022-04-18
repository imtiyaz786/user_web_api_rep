using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace USERWebApi.Models
{
    public class RegistrationModel:IdentityUser
    {
        [Required]
        public string? Category { get; set; }
    }
}
