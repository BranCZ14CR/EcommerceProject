using EcommerceProject.Data.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using EcommerceProject.Models;
using EcommerceProject.Data.Base;

namespace EcommerceProject.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMovieServices
    {
        //Variables
        private readonly EcommerceDbContext _context;

        //Constructor
        public MoviesService(EcommerceDbContext context) : base(context)
        {
            _context = context;
        }

        
        //Método para añadir una nueva pelicula
        public async Task AddNewMovieAsync(NewMovieVM data)
        {
            //Crear una variable con todos los parámetros de la película
            var newMovie = new Movie()
            {
                Nombre = data.Nombre,
                Descripcion = data.Descripcion,
                Precio = data.Precio,
                ImagenURL = data.ImagenURL,
                CinemaId = data.CinemaId,
                FechaInicio = data.FechaInicio,
                FechaFinal = data.FechaFinal,
                CategoriaPelicula = data.CategoriaPelicula,
                ProducerId = data.ProducerId
            };

            //Agregar la película y salvar los cambios
            await _context.Movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();

            //Agregar los actores a la película
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                };
                await _context.Actors_Movies.AddAsync(newActorMovie);
            }

            //Salvar los cambios
            await _context.SaveChangesAsync();
        }

        
        //Obtener una película por su ID 
        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            //Obtener una variable que contenga el las peliculas incluyendo el cine, productor y sus actores
            var movieDetails = await _context.Movies
                .Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.Actors_Movies).ThenInclude(a => a.Actor)
                .FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }


        //Obtener los valores de todos los productores, actores y cines que se crean para agregarlos a la película
        public async Task<NewMoviewDropDownsVM> GetNewMovieDropdownsValues()
        {
            var response = new NewMoviewDropDownsVM()
            {
                Actors = await _context.Actors.OrderBy(n => n.Nombre).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(n => n.Nombre).ToListAsync(),
                Producers = await _context.Producers.OrderBy(n => n.Nombre).ToListAsync()
            };

            return response;
        }


        //Métodos para actualizar la pelicula
        public async Task UpdateMovieAsync(NewMovieVM data)
        {
            //Obtener el ID de la película
            var dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.Id == data.Id);

            //Comprobar que no este nulo el ID de la Película
            if (dbMovie != null)
            {
                //Obteenr los datos 
                dbMovie.Nombre = data.Nombre;
                dbMovie.Descripcion = data.Descripcion;
                dbMovie.Precio = data.Precio;
                dbMovie.ImagenURL = data.ImagenURL;
                dbMovie.CinemaId = data.CinemaId;
                dbMovie.FechaInicio = data.FechaInicio;
                dbMovie.FechaFinal = data.FechaFinal;
                dbMovie.CategoriaPelicula = data.CategoriaPelicula;
                dbMovie.ProducerId = data.ProducerId;

                //Salvar los cambios
                await _context.SaveChangesAsync();
            }

            //Remover los actores existentes
            var existingActorsDb = _context.Actors_Movies.Where(n => n.MovieId == data.Id).ToList();
            _context.Actors_Movies.RemoveRange(existingActorsDb);
            await _context.SaveChangesAsync();

            //Añadir los actores nuevos de la película
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = data.Id,
                    ActorId = actorId
                };
                await _context.Actors_Movies.AddAsync(newActorMovie);
            }

            //Salvar los cambios
            await _context.SaveChangesAsync();
        }   
    }
}
