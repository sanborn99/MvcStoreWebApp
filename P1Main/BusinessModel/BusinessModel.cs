using System;
using P1DbContext.Models;
using System.Linq;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class BusinessModel : IBusinessModel
    {
        private Customer currentUser;// { get; set; }// = new Customer();
        private Location currentLocation;// { get; set; }// = new Location();

        public P1DbClass context;

        /// <summary>
        /// Constructor for business class that takes a Db context
        /// </summary>
        /// <param name="context">Db context</param>
        public BusinessModel(P1DbClass context)
        {
            this.context = context;
        }

        /// <summary>
        /// Returns true of there is a valid user based on username and password
        /// </summary>
        /// <param name="inputUsername">username input</param>
        /// <param name="inputPassword">password input</param>
        /// <returns>true if there is a valid user</returns>
        public bool Login(string inputUsername, string inputPassword)
        {

            Customer loginUser;
            //using (P1DbClass context = new P1DbClass())
            //{
            loginUser = context.Customers.Where(x => x.UserName == inputUsername && x.Password == inputPassword).FirstOrDefault();
            //}


            if (loginUser == null)
            {
                return false;
            }
            else
            {
                currentUser = loginUser;
                return true;
            }
        }

        /// <summary>
        /// Creates a new account with the provided Customer object
        /// </summary>
        /// <param name="newUser">new Customer object to be added</param>
        public bool CreateAccount(Customer newUser)
        {
            try
            {
                context.Add(newUser);     // add the new object to the database
                context.SaveChanges();      // save and update changes

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the current Customer that describes the current User
        /// </summary>
        /// <returns>Current Customer being used by model</returns>
        public Customer GetCurrentUser()
        {
            return currentUser;
        }

        /// <summary>
        /// Returns a Location Object based on the given Location Id
        /// </summary>
        /// <param name="locationId">Takes in a Location Id integer</param>
        /// <returns>Returns the Location Object</returns>
        public Location GetLocation(int locationId)
        {
            currentLocation = context.Locations.Where(x => x.LocationId == locationId).FirstOrDefault();
            return currentLocation;
        }

        /// <summary>
        /// Queries Database for a List of all Location Objects
        /// </summary>
        /// <returns>List of all Location Objects</returns>
        public List<Location> GetLocationsList()
        {
            var locList = context.Locations.ToList();
            return locList;
        }

        /// <summary>
        /// Queries Database for a List of all Order Objects
        /// </summary>
        /// <returns>List of all Order Objects</returns>
        public List<Order> GetOrderList()
        {
            var orderList = context.Orders.ToList();
            return orderList;
        }

        /// <summary>
        /// Queries Database for a List of all Order Objects matching input Location Id
        /// </summary>
        /// <param name="locationId">input Location Id to filter by</param>
        /// <returns>Filtered List if Order objects</returns>
        public List<Order> GetOrderList(int locationId)
        {
            var orderList = context.Orders.Where(x => x.LocationId == locationId).ToList();
            return orderList;
        }

        /// <summary>
        /// Queries Database for a List of all Order Objects matching input Customer Id
        /// </summary>
        /// <param name="customerId">input Customer Id to filter by</param>
        /// <returns>Filtered List if Order objects</returns>
        public List<Order> GetCustomerOrderList(int customerId)
        {
            var orderList = context.Orders.Where(x => x.CustomerId == customerId).ToList();
            return orderList;
        }

        /// <summary>
        /// Returns List of Customers matching parameters or list of all customers on empty parameters
        /// </summary>
        /// <param name="fName">First name contains</param>
        /// <param name="lName">Last name contains</param>
        /// <returns>List of Customer Objects that contain input requirements</returns>
        public List<Customer> GetCustomerList(string fName = "", string lName = "")
        {
            var customerList = context.Customers.Where(x => x.FirstName.Contains(fName) && x.LastName.Contains(lName)).ToList();

            return customerList;
        }

        /// <summary>
        /// Returns a List of Ordered Products filtered by input Order Id
        /// </summary>
        /// <param name="selectedOrderId">Order Id to filter by</param>
        /// <returns>List of Ordered Product Objects</returns>
        public List<OrderedProduct> GetOrderedProductList(int selectedOrderId)
        {
            var orderedProductList = context.OrderedProducts.Where(x => x.OrderId == selectedOrderId).ToList();

            return orderedProductList;
        }

        /// <summary>
        /// Returns List of Strings matching all unique product category types at a location
        /// </summary>
        /// <param name="locationId">Location Id to search at</param>
        /// <returns>List of strings of unique category names</returns>
        public List<string> GetCategoryList(int locationId)
        {
            var joinResults = context.Inventories.Join(
                context.Products,
                invent => invent.ProductId,
                prod => prod.ProductId,
                (invent, prod) => new
                {
                    ProductLocationId = invent.LocationId,
                    ProductCategory = prod.Category
                }

            );

            var categoryList = joinResults.Where(x => x.ProductLocationId == locationId).Distinct().ToList();

            List<string> categoryStringList = new List<string>();

            foreach(var obj in categoryList)
            {
                categoryStringList.Add(obj.ProductCategory);
            }


            return categoryStringList;
        }

        /// <summary>
        /// Gets list of Products and their quantities at a given location
        /// </summary>
        /// <param name="locationId">location Id to search at</param>
        /// <returns>List of Inventory Product Objects</returns>
        public List<InventoryProduct> GetLocationProductList(int locationId)
        {


            var joinResults = context.Inventories.Join(
                context.Products,
                invent => invent.ProductId,
                prod => prod.ProductId,
                (invent, prod) => new InventoryProduct(
                    prod.ProductId,
                    prod.ProductName,
                    prod.Price,
                    prod.Description,
                    prod.Category,
                    invent.LocationId,
                    invent.NumberProducts)
            ).AsEnumerable();

            List<InventoryProduct> productList = joinResults.Where(x => x.LocationId == locationId).ToList();

            return productList;
        }

        /// <summary>
        /// Gets list of Products and their quantities at a given location Filtered by category
        /// </summary>
        /// <param name="locationId">Location Id to search by</param>
        /// <param name="GetLocationProductList">Category Name to filter by</param>
        /// <returns>List of filtered Inventory Product Objects</returns>
        public List<InventoryProduct> GetLocationProductList(int locationId, string GetLocationProductList)
        {

            var joinResults = context.Inventories.Join(
                context.Products,
                invent => invent.ProductId,
                prod => prod.ProductId,
                (invent, prod) => new InventoryProduct(
                    prod.ProductId,
                    prod.ProductName,
                    prod.Price,
                    prod.Description,
                    prod.Category,
                    invent.LocationId,
                    invent.NumberProducts)
            ).AsEnumerable();

            List<InventoryProduct> productList = joinResults.Where(x => x.LocationId == locationId && x.Category == GetLocationProductList).ToList();

            return productList;
        }

        /// <summary>
        /// Returns an Order object based on the Order Id input
        /// </summary>
        /// <param name="selectedOrderId">Order Id to filter by</param>
        /// <returns>Order object</returns>
        public Order GetOrderDetails(int selectedOrderId)
        {
            var order = context.Orders.Where(x => x.OrderId == selectedOrderId).FirstOrDefault();
            return order;
        }

        /// <summary>
        /// Adds input product and quantity to users cart and returns the updated cart
        /// </summary>
        /// <param name="userCart">Shopping cart to update</param>
        /// <param name="productId">Product Id to add to cart</param>
        /// <param name="numAdded">Quantity to add</param>
        /// <returns>Updated Cart</returns>
        public Dictionary<int, int> AddToCart(Dictionary<int, int> userCart, int productId, int numAdded)
        {
            

            if (numAdded == 0) return userCart;

            if (userCart.ContainsKey(productId))
            {
                userCart[productId] += numAdded;
            }
            else
            {
                userCart.Add(productId, numAdded);
            }


            return userCart;

        }

        /// <summary>
        /// Converts a Product Id : Quantity cart into a Product : Quantity cart to provide more information for displaying to user
        /// </summary>
        /// <param name="userCart">Cart to convert</param>
        /// <returns>Converted Cart in Product, Quantity form</returns>
        public Dictionary<Product, int> ConvertDict(Dictionary<int, int> userCart)
        {
            Dictionary<Product, int> newCart = new Dictionary<Product, int>();

            foreach(var item in userCart)
            {
                Product getItem = context.Products.Where(x => x.ProductId == item.Key).FirstOrDefault();

                newCart.Add(getItem, item.Value);
            }

            return newCart;

        }

        /// <summary>
        /// Calculates the total value of the items in shopping cart
        /// </summary>
        /// <param name="cart">Cart to sum</param>
        /// <returns>Decimal value of carts contents</returns>
        public decimal GetCartTotal(Dictionary<Product, int> cart)
        {
            decimal sum = 0;

            foreach (var obj in cart)
            {
                sum += (obj.Key.Price * obj.Value);
            }

            return sum;
        }

        /// <summary>
        /// Creates a new order for the current user at the current location and updates the database with a new
        /// order and ordered products. Decrements current stores inventory by quantity of the ordered products.
        /// </summary>
        /// <param name="cart">Current shopping cart of desired products</param>
        /// <param name="customerId">Current Customer Id</param>
        /// <param name="locationId">Current shopping Location Id</param>
        /// <returns>The newly generated Order Object for the finalized order</returns>
        public Order Checkout(Dictionary<Product, int> cart, int customerId, int locationId)
        {
            Order thisOrder = new Order();
            thisOrder.OrderTime = DateTime.Now;
            thisOrder.CustomerId = customerId;
            thisOrder.LocationId = locationId;

            try
            {
                // Add the new order object to the Database
                context.Add(thisOrder);
                context.SaveChanges();
            }
            catch{};


            int newOrderId = context.Orders.Max(x => x.OrderId);
            Order newOrder = context.Orders.Where(x => x.OrderId == newOrderId).FirstOrDefault();


            foreach (var item in cart)
            {
                foreach (var obj in context.Inventories)
                {
                    if (obj.LocationId == locationId && obj.ProductId == item.Key.ProductId)
                    {
                        obj.NumberProducts -= item.Value;
                    }
                }
                // Add an ordered product object for each product in the shopping cart
                var newOrderedProduct = new OrderedProduct();
                newOrderedProduct.OrderId = newOrder.OrderId;
                newOrderedProduct.ProductId = item.Key.ProductId;
                newOrderedProduct.NumberOrdered = item.Value;
                context.Add(newOrderedProduct);
            }
            // Save Database Changes and clear user's shopping cart
            try
            {
                context.SaveChanges();
            }
            catch {};

            return newOrder;
        }

        
    }
}
