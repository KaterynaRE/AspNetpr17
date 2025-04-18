﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopComputerDomainLibrary
{
    public class CartItem
    {
        public Product Product { get; set; } = default!;

        public int CountProduct { get; set; }

        public double TotalPrice => Product.Price * CountProduct;
    }
}
