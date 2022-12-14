using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    /*
     Clase que contiene las propiedades del Actor de las diferentes peliculas
     */
    public class Actor
    {
        //Atributos
        [Key]
        public int ActorId { get; set; }

        [Display(Name = "Perfil del Actor")]
        [Required (ErrorMessage = "La imagen del actor es requerida")]
        public string Imagen { get; set; }

        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "Ingrese el nombre completo")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre tiene que tener un máximo de 50 caracteres y un mínimo de 2")]
        public string Nombre { get; set; }
        
        [Display(Name = "Biografía")]
        [Required(ErrorMessage = "Ingrese la biografía del actor")]
        [StringLength(400, MinimumLength = 50, ErrorMessage = "La biografía tiene que tener un máximo de 400 caracteres y un mínimo de 50")]
        public string Biografia { get; set; }

        //Relacion con las demás tablas
        public List<Actor_Movie> Actors_Movies { get; set; }
    }
}
