using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceProject.Data.ViewModel
{
    public class LoginVM
    {
        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "Ingrese un correo electrónico")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Ingrese una contraseña")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
