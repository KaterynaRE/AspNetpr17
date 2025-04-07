using ShopComputerDomainLibrary;

namespace ShopComputer.Models.ViewModels.Products
{
    public class IndexProductVM
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
