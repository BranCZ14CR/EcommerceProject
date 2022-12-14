using EcommerceProject.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Data.ViewComponents
{

    public class ShoppingCartSummary: ViewComponent
    {
        //Atributos
        private readonly ShoppingCart _shoppingCart;

        //Constuctor
        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }


        //Método para obtener el numero de items que tiene un carrito
        public IViewComponentResult Invoke()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            return View(items.Count);
        }
    }
}
