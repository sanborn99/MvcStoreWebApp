using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P1DbContext.Models;


namespace BusinessLayer
{
    public interface IBusinessModel
    {

        public bool Login(string inputUsername, string inputPassword);

        public bool CreateAccount(Customer newUser);
        public Customer GetCurrentUser();
        public Location GetLocation(int locationId);

        public List<Location> GetLocationsList();

        public List<string> GetCategoryList(int locationId);

        public List<Order> GetOrderList();

        public List<Order> GetOrderList(int locationId);

        public List<OrderedProduct> GetOrderedProductList(int selectedOrderId);

        public List<Order> GetCustomerOrderList(int customerId);

        public List<Customer> GetCustomerList(string fName, string lName);

        public List<InventoryProduct> GetLocationProductList(int locationId);

        public List<InventoryProduct> GetLocationProductList(int locationId, string GetLocationProductList);

        public Order GetOrderDetails(int selectedOrderId);

        public Dictionary<int, int> AddToCart(Dictionary<int, int> userCart, int productId, int numAdded);

        public Dictionary<Product, int> ConvertDict(Dictionary<int, int> userCart);

        public decimal GetCartTotal(Dictionary<Product, int> cart);

        public Order Checkout(Dictionary<Product, int> cart, int customerId, int locationId);
    }
}
