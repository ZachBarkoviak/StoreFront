using StoreFront.DATA.EF.Models;

namespace GadgetStore.UI.MVC.Models
{
    public class CartItemViewModel
    {
        // Shopping cart - Step 2
        // created this class...

        public int Qty { get; set; }

        public Product CartProd { get; set; }

        public CartItemViewModel() { }

        public CartItemViewModel(int qty, Product product)
        {
            Qty = qty;
            CartProd = product;
        }



    }
}
