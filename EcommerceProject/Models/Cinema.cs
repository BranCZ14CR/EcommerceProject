using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    /*
        Clase de los cines que pueden existir
     */
    public class Cinema
    {
        //Atributos
        [Key]
        public int CinemaId { get; set; }

        [Display(Name = "Logo")]
        [Required(ErrorMessage = "El Logo del Cinema es requerida")]
        public string Logo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Ingrese el nombre del Cinema")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del cinema tiene que tener un máximo de 50 caracteres y un mínimo de 2")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Ingrese la descripción del Cinema")]
        [StringLength(400, MinimumLength = 50, ErrorMessage = "La descripción tiene que tener un máximo de 400 caracteres y un mínimo de 50")]
        public string Descripcion { get; set; } 

        //Relaciones con las demás tablas
        public List<Movie> Movies { get; set; }
    }
}
