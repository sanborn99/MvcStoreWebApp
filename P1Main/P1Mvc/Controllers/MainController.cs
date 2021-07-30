using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using P1DbContext.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace P1Mvc.Controllers
{
    public class MainController : Controller
    {
        public IBusinessModel _BusinessModel;

        public MainController(IBusinessModel BusinessModel)
        {
            this._BusinessModel = BusinessModel;
        }

        public IActionResult HomePage()
        {


            Location userLocation = JsonConvert.DeserializeObject<Location>(HttpContext.Session.GetString("CurrentSessionLocation"));
            Customer userCustomer = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("CurrentSessionUser"));

            ViewBag.currentLocation = userLocation;
            ViewBag.currentUser = userCustomer;


            return View();
        }

        public IActionResult ShopMenu()
        {

            return View();
        }

        public IActionResult ReturnHome()
        {
            return RedirectToAction("HomePage");
        }

        public ActionResult ChangeLocations()
        {
            Dictionary<int, int> userCart = new Dictionary<int, int>();
            HttpContext.Session.SetString("CurrentSessionUserCart", JsonConvert.SerializeObject(userCart));
            return View(_BusinessModel.GetLocationsList());
        }

        public ActionResult BrowseProducts(string storeCategoryName = "any")              
        {
            Location userLocation = JsonConvert.DeserializeObject<Location>(HttpContext.Session.GetString("CurrentSessionLocation"));

            List<InventoryProduct> productList = new List<InventoryProduct>();

            if(String.Equals(storeCategoryName, "any"))
            {
                productList = _BusinessModel.GetLocationProductList(userLocation.LocationId);
            }
            else
            {
                productList = _BusinessModel.GetLocationProductList(userLocation.LocationId, storeCategoryName);
            }
            
            List<string> categoryList = _BusinessModel.GetCategoryList(userLocation.LocationId);

            ViewBag.categoryList = categoryList;

            return View(productList);
        }

        public ActionResult Details(string selectedProductString)
        {
            InventoryProduct selectedProduct = JsonConvert.DeserializeObject<InventoryProduct>(selectedProductString);
            Dictionary<int, int> userCart = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("CurrentSessionUserCart"));


            int numInCart = 0;
            if (userCart.ContainsKey(selectedProduct.ProductId))
            {
                numInCart = userCart[selectedProduct.ProductId];
            }

            ViewBag.selectedProduct = selectedProduct;
            ViewBag.maxAmount = selectedProduct.NumberProducts - numInCart;
            ViewBag.numInCart = numInCart;

            return View();
        }

        public ActionResult AddToCart(int productId, int quantity)
        {
            Dictionary<int, int> userCart = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("CurrentSessionUserCart"));

            _BusinessModel.AddToCart(userCart, productId, quantity);
            HttpContext.Session.SetString("CurrentSessionUserCart", JsonConvert.SerializeObject(userCart));

            return RedirectToAction("BrowseProducts");
        }

        public ActionResult DisplayCart()
        {
            Dictionary<int, int> userCart = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("CurrentSessionUserCart"));

            Dictionary<Product, int> newCart = _BusinessModel.ConvertDict(userCart);

            ViewBag.orderTotal = _BusinessModel.GetCartTotal(newCart);

            return View(newCart);
        }

        public ActionResult Checkout()
        {
            Dictionary<int, int> userCart = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("CurrentSessionUserCart"));

            Dictionary<Product, int> newCart = _BusinessModel.ConvertDict(userCart);

            ViewBag.orderTotal = _BusinessModel.GetCartTotal(newCart);

            return View(newCart);
        }

        public ActionResult ProcessCheckout()
        {
            Location userLocation           = JsonConvert.DeserializeObject<Location>(HttpContext.Session.GetString("CurrentSessionLocation"));
            Customer userCustomer           = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("CurrentSessionUser"));
            Dictionary<int, int> userCart   = JsonConvert.DeserializeObject<Dictionary<int, int>>(HttpContext.Session.GetString("CurrentSessionUserCart"));

            Dictionary<Product, int> newCart = _BusinessModel.ConvertDict(userCart);

            Order thisOrder = _BusinessModel.Checkout(newCart, userCustomer.CustomerId, userLocation.LocationId);
            ViewBag.thisOrder = thisOrder;

            // Empty the now checked out cart
            userCart = new Dictionary<int, int>();
            Customer currentUser = new Customer();
            Location currentLoc = new Location();

            HttpContext.Session.SetString("CurrentSessionUserCart", JsonConvert.SerializeObject(userCart));
            HttpContext.Session.SetString("CurrentSessionUser", JsonConvert.SerializeObject(currentUser));
            HttpContext.Session.SetString("CurrentSessionLocation", JsonConvert.SerializeObject(currentLoc));

            return View();
        }

    } // End Class
} // End Name