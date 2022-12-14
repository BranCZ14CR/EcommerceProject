using EcommerceProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceProject.Data.Cart
{
    public class ShoppingCart
    {
        //Atributos
        public EcommerceDbContext _context { get; set; }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }


        //Constructor de la clase
        public ShoppingCart(EcommerceDbContext context)
        {
            _context = context;
        }


        //Método para obtener una session del carrito de compras para tener toda la información
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            //Instanciar una Session de HTTP de la base de datos
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<EcommerceDbContext>();

            //Crear la variable de session 
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            //Retornar el carrito de compras
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }


        //Método para agregar una película al carrito de compras
        public void AddItemToCart(Movie movie)
        {
            //Obtener el ID de la pelicula para comprobar si existe o no en el carrito
            var ShoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            //Comprobar que no este vacio el item del carrito
            if (ShoppingCartItem == null)
            {
                //En caso de que sea nulo entonces se agrega la pelicula al carrito de compra
                ShoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };

                //Se agrega a los items del carrito de compra
                _context.ShoppingCartItems.Add(ShoppingCartItem);
            }
            else
            {
                //En caso de que exista la pelicula se aumenta la cantidad
                ShoppingCartItem.Amount++;
            }

            //Salvar los cambios
            _context.SaveChanges();
        }


        //Método para remover una pelicula del carrito de compras
        public void RemoveItemFromCart(Movie movie)
        {
            //Obtener el ID de la pelicula para comprobar si existe o no en el carrito
            var ShoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            //Comprobar que si esta vacio el item del carrito
            if (ShoppingCartItem != null)
            {
                //Verificar si el item es mayor a uno
                if (ShoppingCartItem.Amount > 1)
                {
                    //Reduce una cantidad
                    ShoppingCartItem.Amount--;
                }
                else
                {
                    //En caso de que no sea mayor a 1 se quite todo el item
                    _context.ShoppingCartItems.Remove(ShoppingCartItem);
                }
            }

            //Salvar los cambios
            _context.SaveChanges();
        }


        //Método para obtener una lista de todos los item del carrito
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList());
        }


        //Método para obtener el total del precio del carrito
        public double GetShoppingCartTotal()
        {
            return _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Select(n => n.Movie.Precio * n.Amount).Sum();
        }


        //Método para limpiar el carrito de compras
        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
