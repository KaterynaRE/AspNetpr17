using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopComputerDomainLibrary
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = default!;

        public string Description { get; set; } = default!;

        public double Price { get; set; }

        public int BrandId { get; set; }

        public Brand Brand { get; set; } = default!;

        public Category Category { get; set; } = default!;

        public int CategoryId { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; } = default!;
    }
}
