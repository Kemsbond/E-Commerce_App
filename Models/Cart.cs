using E_Ticaret.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    public class Cart
    {
        private List<CartLine> _cardLines = new List<CartLine>();

        public List<CartLine> Cartlines
        {
            get { return _cardLines; }
        }

        public void AddProduct(Product product,int quantity)
        {
            var line = Cartlines.FirstOrDefault(i => i.Product.Id == product.Id);
            if(line != null)
            {
                line.Quantity += quantity;
            }
            else
            {
                _cardLines.Add(new CartLine() {Product = product,Quantity = quantity });
            }
        }

        public void DeleteOneProduct(Product product, int quantity)
        {
            var line = Cartlines.FirstOrDefault(i => i.Product.Id == product.Id);
            if (line != null)
            {
                if ((line.Quantity - 1) == 0)
                {
                    _cardLines.RemoveAll(i => i.Product.Id == product.Id);
                }
                line.Quantity -= quantity;
            }
           
        }


        public void DeleteProduct(Product product)
        {
            _cardLines.RemoveAll(i => i.Product.Id == product.Id);
        }

        public double Total()
        {
            return _cardLines.Sum(i => i.Product.Price * i.Quantity); 
        }

        public void Clear()
        {
            _cardLines.Clear();
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

}