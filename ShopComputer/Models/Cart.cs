using Microsoft.AspNetCore.Http.HttpResults;
using ShopComputer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopComputerDomainLibrary
{
    public class Cart
    {
        string key = "cart";
        List<CartItem> items = default!;
        private readonly IHttpContextAccessor httpContextAccessor;

        public List<CartItem> CartItems => items;

        public int ItemsCount => CartItems.Count;

        //public Cart(IHttpContextAccessor httpContextAccessor)
        //{
        //    this.httpContextAccessor = httpContextAccessor;
        //}

        public Cart(IHttpContextAccessor httpContextAccessor, List<CartItem> items)
        {
            this.items = items;
            this.httpContextAccessor = httpContextAccessor;
        }

        //public void SetItems(List<CartItem> items)
        //{
        //    this.items = items;
        //}

        public void Add(CartItem item)
        {
            CartItem? cartItem = items.FirstOrDefault(t => t.Product.Id == item.Product.Id);
            if (cartItem == null)
                items.Add(item);
            else
                cartItem.CountProduct = cartItem.CountProduct + 1;
            if (httpContextAccessor.HttpContext is not null)
                httpContextAccessor.HttpContext.Session.Set(key, CartItems);
        }

        public void DecreaseOne(CartItem item)
        {
            CartItem? cartItem = items.FirstOrDefault(c=>c.Product.Id == item.Product.Id);
            if (cartItem != null)
            {
                cartItem.CountProduct = cartItem.CountProduct - 1;
            }
            if (item.CountProduct <= 0)
            {
                CartItems.Remove(item);
            }
            if (httpContextAccessor.HttpContext is not null)
                httpContextAccessor.HttpContext.Session.Set(key, CartItems);
        }

        public bool Remove(CartItem item)
        {
            bool result = items.Remove(item);
            if (httpContextAccessor.HttpContext is not null)
                httpContextAccessor.HttpContext.Session.Set(key, CartItems);
            return result;
        }


        public bool Remove(int id)
        {
            CartItem? cartItem = items.FirstOrDefault(t => t.Product.Id == id);
            if (cartItem != null)
                return Remove(cartItem);
            return false;
        }

        public void Clear()
        {
            items = new List<CartItem>();
        }

        public double GetTotalPrice()
        {
            return items.Sum(t => t.TotalPrice);
        }
    }
}
