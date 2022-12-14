using EcommerceProject.Data.Cart;
using EcommerceProject.Data.Services;
using EcommerceProject.Data.Static;
using EcommerceProject.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommerceProject.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class OrdersController : Controller
    {
        //Variables
        private readonly IMovieServices _movieService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersServices;


        //Constructor
        public OrdersController(IMovieServices movieServices, ShoppingCart shoppingCart, IOrdersService ordersServices)
        {
            _movieService = movieServices;
            _shoppingCart = shoppingCart;
            _ordersServices = ordersServices;
        }

        //Método del Index
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersServices.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }



        //Método para obtener el carrito de compra
        public IActionResult ShoppingCart()
        {
            //Obtener los items del carrito de compras
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            //Instanciar un nuevo objeto que obtiene el total del carrito y todo el carrito
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
            };

            return View(response);
        }


        //Método para agregar un item al carrito de compras
        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            //Obtener una pelicula por identificación
            var item = await _movieService.GetMovieByIdAsync(id);

            //Comprobar que no este vacio y se agrega el item
            if(item != null)
            {
                _shoppingCart.AddItemToCart(item);            
            }

            //Redirreccionar para obtener el carrito de compra
            return RedirectToAction(nameof(ShoppingCart));
        }
        

        //Método para eliminar un item al carrito de compras
        public async Task<IActionResult> RemoveItemFromCart(int id)
        {
            //Obtener una pelicula por identificación
            var item = await _movieService.GetMovieByIdAsync(id);

            //Comprobar que no este vacio y se elimina el item
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }

            //Redirreccionar para obtener el carrito de compra
            return RedirectToAction(nameof(ShoppingCart));
        }

        //Método para completar la orden
        public async Task<IActionResult> CompleteOrder()
        {
            //Obtener todos los items del carrito
            var items = _shoppingCart.GetShoppingCartItems();

            //Obtener los datos del usuario
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersServices.StoreOrderAsync(items, UserId, userEmailAddress);

            //Limpiar el carrito de compra
            await _shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompleted");
        }
    }
}
