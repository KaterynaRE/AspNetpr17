using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopComputerDomainLibrary
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = default!;

        public int? ParentCategoryId { get; set; }

        public Category? ParentCategory { get; set; }

        public ICollection<Category>? ChildCategories { get; set; } = default!;

        public ICollection<Product>? Products { get; set; } = default!;
    }
}
