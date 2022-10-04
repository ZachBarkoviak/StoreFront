﻿using System;
using System.Collections.Generic;

namespace StoreFront.UI.MVC.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public int ManufacturerId { get; set; }
        public bool StockStatus { get; set; }
        public int StockQty { get; set; }
        public int? UnitsOrdered { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductImage { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;
        public virtual Manufacturer Manufacturer { get; set; } = null!;
    }
}