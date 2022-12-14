using EcommerceProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceProject.Models
{
    //Clase que va a obtener agregar una nueva película
    public class NewMovieVM
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de la película")]
        [Required(ErrorMessage = "Ingrese el nombre")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre tiene que tener un máximo de 50 caracteres y un mínimo de 2")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción de la película")]
        [Required(ErrorMessage = "Ingrese la descripción")]
        [StringLength(400, MinimumLength = 2, ErrorMessage = "La descripción tiene que tener un máximo de 400 caracteres y un mínimo de 2")]
        public string Descripcion { get; set; }

        [Display(Name = "Precio en colones (₡)")]
        [Required(ErrorMessage = "Ingrese un precio")]
        public double Precio { get; set; }

        [Display(Name = "Imagen de Película")]
        [Required(ErrorMessage = "Ingrese una imagen")]
        public string ImagenURL { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "Ingrese una fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha de Fin")]
        [Required(ErrorMessage = "Ingrese una fecha de fin")]
        public DateTime FechaFinal { get; set; }

        [Display(Name = "Categoría de la película")]
        [Required(ErrorMessage = "Selecciona una categoría")]
        public CategoriaPelicula CategoriaPelicula { get; set; }

        //Relaciones con las demás tablas
        [Display(Name = "Actor(es)")]
        [Required(ErrorMessage = "Selecciona los actores")]
        public List<int> ActorIds { get; set; }

        [Display(Name = "Cinema")]
        [Required(ErrorMessage = "Selecciona un cine")]
        public int CinemaId { get; set; }

        [Display(Name = "Productor")]
        [Required(ErrorMessage = "Selecciona un productor")]
        public int ProducerId { get; set; }

    }
}
