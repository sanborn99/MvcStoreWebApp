using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using P1DbContext.Models;

namespace P1Mvc.Controllers
{
    public class ExtrasController : Controller
    {
        public IBusinessModel _BusinessModel;

        public ExtrasController(IBusinessModel BusinessModel)
        {
            this._BusinessModel = BusinessModel;
        }


        public IActionResult ExtrasMenu()
        {
            return View();
        }

        public IActionResult LocationHistory(int locationId = -1)
        {
            List<Location> locationList = _BusinessModel.GetLocationsList();
            List<Order> orderList = new List<Order>();

            if (locationId == -1)
            {
                orderList = _BusinessModel.GetOrderList();
            }
            else
            {
                orderList = _BusinessModel.GetOrderList(locationId);
            }

            ViewBag.locationList = locationList;
            return View(orderList);
        }

        public IActionResult CustomerSearch(string fName = "", string lName = "")
        {
            if(fName == null) fName = string.Empty;

            if(lName == null) lName = string.Empty;

            List<Customer> customerList = _BusinessModel.GetCustomerList(fName, lName);

            return View(customerList);
        }

        public IActionResult CustomerOrderDetails(int selectedCustomerId)
        {

            List<Order> orderList = _BusinessModel.GetCustomerOrderList(selectedCustomerId);

            return View(orderList);
        }

        public IActionResult OrderDetails(int selectedOrderId)
        {
            ViewBag.orderDetails = _BusinessModel.GetOrderDetails(selectedOrderId);
            List<OrderedProduct> orderedProductList = _BusinessModel.GetOrderedProductList(selectedOrderId);

            Dictionary<int, int> orderedProductInfo = new Dictionary<int, int>();
            
            foreach(var obj in orderedProductList)
            {
                orderedProductInfo.Add(obj.ProductId, obj.NumberOrdered);
            }

            Dictionary<Product, int> newCart = _BusinessModel.ConvertDict(orderedProductInfo);

            ViewBag.orderTotal = _BusinessModel.GetCartTotal(newCart);
            return View(newCart);
        }

    }
}
