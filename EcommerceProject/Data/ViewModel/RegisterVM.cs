using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceProject.Data.ViewModel
{
    public class RegisterVM
    {
        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "Ingrese el nombre completo")]
        public string FullName { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "Ingrese un correo electrónico")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Ingrese una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirmación de Contraseña")]
        [Required(ErrorMessage = "La Confirmación de Contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
