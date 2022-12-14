using EcommerceProject.Models;
using System.Collections.Generic;

namespace EcommerceProject.Data.ViewModel
{
    //Clase que obtiene las listas de los productores, cines y actores

    public class NewMoviewDropDownsVM
    {
        public NewMoviewDropDownsVM()
        {
            Producers = new List<Producer>();
            Cinemas = new List<Cinema>();
            Actors = new List<Actor>();
        }

        public List<Producer> Producers { get; set; }

        public List<Cinema> Cinemas { get; set; }

        public List<Actor> Actors { get; set; }

    }
}
