using System;
using System.Collections.Generic;


namespace P1DbContext.Models
{
    public class InventoryProduct
    {
        public InventoryProduct()
        {

        }

        public InventoryProduct(int ProductId, string ProductName, decimal Price, string Description, string Category, int LocationId, int NumberProducts)
        {
            this.ProductId      = ProductId;
            this.ProductName    = ProductName;
            this.Price          = Price;
            this.Description    = Description;
            this.Category       = Category;
            this.LocationId     = LocationId;
            this.NumberProducts = NumberProducts;

        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public int LocationId { get; set; }
        public int NumberProducts { get; set; }
    }
}
