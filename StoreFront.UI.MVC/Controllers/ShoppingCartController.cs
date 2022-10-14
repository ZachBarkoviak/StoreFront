using Microsoft.AspNetCore.Mvc;

using StoreFront.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using StoreFront.UI.MVC.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Steps to Implement Session Based Shopping Cart
        /*
         * 1) Register Session in program.cs (builder.Services.AddSession... && app.UseSession())
         * 2) Create the CartItemViewModel class in [ProjName].UI.MVC/Models folder
         * 3) Add the 'Add To Cart' button in the Index and/or Details view of your Products
         * 4) Create the ShoppingCartController (empty controller -> named ShoppingCartController)
         *      - add using statements
         *          - using GadgetStore.DATA.EF.Models;
         *          - using Microsoft.AspNetCore.Identity;
         *          - using GadgetStore.UI.MVC.Models;
         *          - using Newtonsoft.Json;
         *      - Add props for the GadgetStoreContext && UserManager
         *      - Create a constructor for the controller - assign values to context && usermanager
         *      - Code the AddToCart() action
         *      - Code the Index() action
         *      - Code the Index View
         *          - Start with the basic table structure
         *          - Show the items that are easily accessible (like the properties from the model)
         *          - Calculate/show the lineTotal
         *          - Add the RemoveFromCart <a>
         *      - Code the RemoveFromCart() action
         *          - verify the button for RemoveFromCart in the Index view is coded with the controller/action/id
         *      - Add UpdateCart <form> to the Index View
         *      - Code the UpdateCart() action
         *      - Add Submit Order button to Index View
         *      - Code SubmitOrder() action
         * */
        #endregion

        private readonly FrontierConsolidatedStoreContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(FrontierConsolidatedStoreContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //Retrieve the contents from the Session shopping cart (stored as JSON) and convert them to C# using Newtonsoft.Json
            //after converting to C#, we can pass the collection back to a strongly-typed view

            //retrieve the cart
            var sessionCart = HttpContext.Session.GetString("cart");

            //create the shell for the local (C#) shopping cart
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //if the session cart is null, or if there are 0 items in the session cart, return a message to notify the user that cart is empty
            if (sessionCart == null || sessionCart.Count() == 0)
            {
                ViewBag.Message = "There are no items in your cart.";

                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                ViewBag.Message = null;

                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            //no matter what, return the collection to the View
            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            // Get the cart ready
            //we will have 2 instances of the cart - a local variable for the shopping cart and the session variable.
            //the local variable will be dealing with the C# instances of cart items -> the session variable will be dealing with the
            //JSON instances of the cart items

            //Local cart instance
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //retrieve the session instance of the cart to see if it exists yet
            var sessionCart = HttpContext.Session.GetString("cart");

            if (sessionCart == null)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            // Retrieve the product for the cart from the DB
            Product product = _context.Products.Find(id);

            // Create the CartItemViewModel for the product being added
            CartItemViewModel civm = new CartItemViewModel(1, product);

            // If the product is already in the cart, increase the qty by 1
            // Otherwise, add the new item into the local shopping cart
            if (shoppingCart.ContainsKey(product.ProductId))
            {
                //update qty
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, civm);
            }

            //update the session version of the cart
            // take the local copy, serialize as JSON
            // then store that JSON value in session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);




            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            // retrieve our cart from session
            string sessionCart = HttpContext.Session.GetString("cart");


            // deserialize the JSON
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);


            // remove the cart item from the C# collection
            shoppingCart.Remove(id);


            // update session again
            if (shoppingCart.Count() == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }



            // send the user back to the shopping cart index
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            //get cart and deserialize from JSON to C#
            string sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //update the qty for the productId provided in the params of this action
            shoppingCart[productId].Qty = qty;

            //update session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        //This method must be async in order to invoke the UserManager's async methods in this action.
        public async Task<IActionResult> SubmitOrder()
        {
            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            UserDetail ud = _context.UserDetails.Find(userId);

            //Create Order Object
            Order o = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                ShipToName = ud.FirstName + " " + ud.LastName,
                ShipToPlanetId = ud.PlanetId
            };

            // Add the Order to _context
            _context.Orders.Add(o);

            // Retrieve the session cart and convert to C#
            string sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Create OrderProduct for every item in our cart
            foreach (var item in shoppingCart)
            {
                OrderProduct op = new OrderProduct()
                {                    
                    ProductId = item.Value.CartProd.ProductId,
                    OrderId = o.OrderId,
                    Quantity = (short)item.Value.Qty,
                    ProductPrice = item.Value.CartProd.ProductPrice
                };

                //ONLY need to add items to an existing entity (here -> the order 'o') if the items are a related record (like the OrderProduct here)
                o.OrderProducts.Add(op);

            }

            //save changes to DB
            _context.SaveChanges();
            HttpContext.Session.Remove("cart");

            return RedirectToAction("Index", "Orders");
        }

    }
}
