using EcommerceProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceProject.Data.Services
{
    public class OrdersService : IOrdersService
    {
        //Variables
        private readonly EcommerceDbContext _context;

        //Constructor
        public OrdersService(EcommerceDbContext context)
        {
            _context = context;
        }

        //Método para obtener el rol y el id del usuario con las ordenees de compras
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            //Obtener las ordenes 
            var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Movie).Include(n => n.User).ToListAsync();

            //Comprobar que el ID no sea el Admin
            if (userRole != "Admin")
            {
                //Mostrar las ordenes del usuario
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }


        //Método para 
        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            //Obtener la informacion del usuario con el correo
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };

            //Agregarlo y salvar cambios
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            //Recorrer los item de la orden de compra
            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    MovieId = item.Movie.Id,
                    OrderId = order.Id,
                    Price = item.Movie.Precio
                };

                //Agregarlos a los item de la orden
                await _context.OrderItems.AddAsync(orderItem);
            }

            //Salvar los cambios
            await _context.SaveChangesAsync();
        }
    }
}
