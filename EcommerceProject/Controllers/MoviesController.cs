using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceProject.Data;
using EcommerceProject.Models;
using EcommerceProject.Data.ViewModel;
using EcommerceProject.Data.Services;
using Microsoft.AspNetCore.Authorization;
using EcommerceProject.Data.Static;

namespace EcommerceProject.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        //Variables
        private readonly IMovieServices _service;

        //Constructor
        public MoviesController(IMovieServices service)
        {
            _service = service;
        }

        //Obtener todas las películas en una lista
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync();
            return View(allMovies);
        }


        //Método que filtra las peliculas en forma de búsqueda en el header
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            //Obtener todas las peliculas
            var allMovies = await _service.GetAllAsync();

            //Comprobar que el string de la búsqueda no este vacio o nulo
            if (!string.IsNullOrEmpty(searchString))
            {
               //Obtener el resultado de la busqueda sea por nombre o descripcion
                var filteredResultNew = allMovies.Where(n => string.Equals(n.Nombre, searchString, StringComparison.CurrentCultureIgnoreCase) || 
                string.Equals(n.Descripcion, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                //Retornar el index con la pelicula buscada
                return View("Index", filteredResultNew);
            }

            //Retornar todas las películas
            return View("Index", allMovies);
        }


        //Obtener una película en especifico
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var movieDetail = await _service.GetMovieByIdAsync(id);

            if (movieDetail == null)
            {
                return View("NotFound");
            }

            return View(movieDetail);
        }


        //Ir a la pagina de crear una película
        public async Task<IActionResult> Create()
        {
            //Obtener todas las listas de cines, productores y actores
            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();

            //Obtener las listas por separado
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "CinemaId", "Nombre");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "ProducerId", "Nombre");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "ActorId", "Nombre");

            return View();
        }


        //Método para agregar una película a la BD
        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            //Comprobar que todo este válido
            if (!ModelState.IsValid)
            {
                //Obtener todas las listas de cines, productores y actores
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();

                //Obtener las listas por separado
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "CinemaId", "Nombre");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "ProducerId", "Nombre");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "ActorId", "Nombre");

                return View(movie);
            }

            //Agregar una nueva película y salvar los cambios y redirrecionar al index
            await _service.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }


        //Ir a la página de editar un actor
        public async Task<IActionResult> Edit(int id)
        {
            //Obtener una película en especifico
            var movieDetails = await _service.GetMovieByIdAsync(id);

            //Si esta vacio retorna no encontrado
            if (movieDetails == null) return View("NotFound");

            //Crear una variable con toda la informacion obtenida
            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Nombre = movieDetails.Nombre,
                Descripcion = movieDetails.Descripcion,
                Precio = movieDetails.Precio,
                FechaInicio = movieDetails.FechaInicio,
                FechaFinal = movieDetails.FechaFinal,
                ImagenURL = movieDetails.ImagenURL,
                CategoriaPelicula = movieDetails.CategoriaPelicula,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
            };

            //Obtener todas las listas de cines, productores y actores
            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();

            //Obtener las listas por separado
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "CinemaId", "Nombre");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "ProducerId", "Nombre");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "ActorId", "Nombre");

            return View(response);
        }


        //Método para actualizar una película a la BD
        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie)
        {
            //Si esta vacio retorna no encontrado
            if (id != movie.Id) return View("NotFound");

            //Comprobar que todo este válido
            if (!ModelState.IsValid)
            {
                //Obtener todas las listas de cines, productores y actores
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();

                //Obtener las listas por separado
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "CinemaId", "Nombre");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "ProducerId", "Nombre");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "ActorId", "Nombre");

                return View(movie);
            }

            //Actualizar una nueva película y salvar los cambios y redirrecionar al index
            await _service.UpdateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }
    }
}