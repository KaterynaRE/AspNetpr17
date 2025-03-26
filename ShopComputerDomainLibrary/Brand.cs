using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopComputerDomainLibrary
{
    public class Brand
    {
        public int Id { get; set; }

        public string BrandName { get; set; } = default!;

        public string Country { get; set; } = default!;

        public ICollection<Product>? Products { get; set; } = default!;
    }
}
