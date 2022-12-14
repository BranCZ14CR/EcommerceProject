using Microsoft.EntityFrameworkCore;
using EcommerceProject.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EcommerceProject.Data
{
    public class EcommerceDbContext : IdentityDbContext<ApplicationUser>
    {
        //Constructor de la clase
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Definir las entidades
            modelBuilder.Entity<Actor_Movie>().HasKey(am => new
            {
                am.ActorId,
                am.MovieId
            });

            //Creación de la relacion de uno a muchos de la Base de Datos con las clases de Actores y Peliculas
            modelBuilder.Entity<Actor_Movie>().HasOne(m => m.Movie).WithMany(am => am.Actors_Movies).HasForeignKey(m => m.MovieId);
            modelBuilder.Entity<Actor_Movie>().HasOne(m => m.Actor).WithMany(am => am.Actors_Movies).HasForeignKey(m => m.ActorId);

            base.OnModelCreating(modelBuilder);
        }

        //Agregar las tablas con las clases creadas
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor_Movie> Actors_Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Producer> Producers { get; set; }


        //Tablas de la Orden de Compra y el Carrito de Compras
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
