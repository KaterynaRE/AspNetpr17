namespace ShopComputer.Models.ViewModels.Cart
{
    public class CartIndexVM
    {
        public ShopComputerDomainLibrary.Cart Cart { get; set; } = default!;
        public string? ReturnUrl { get; set; }
    }
}
