using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }
        
    }
}
