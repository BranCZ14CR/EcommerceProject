using EcommerceProject.Data.Cart;

namespace EcommerceProject.Data.ViewModel
{
    //Clase que obtiene el carrito de compras y el total de las compras
    public class ShoppingCartVM
    {
        public ShoppingCart ShoppingCart { get; set; }
        public double ShoppingCartTotal { get; set; }
    }
}
