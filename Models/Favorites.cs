using E_Ticaret.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    public class Favorites
    {

        private List<Favorite> _favorites = new List<Favorite>();

        public List<Favorite> Favori
        {
            get { return _favorites; }
        }

        public void AddProduct(Product product)
        {
            var line = Favori.FirstOrDefault(i => i.Product.Id == product.Id);

            if (line == null)
            {
                _favorites.Add(new Favorite() { Product = product, AddDate = DateTime.Now });
            }

        }




        public void DeleteProduct(Product product)
        {
            _favorites.RemoveAll(i => i.Product.Id == product.Id);
        }



        public void Clear()
        {
            _favorites.Clear();
        }


    }


    public class Favorite
    {
        public Product Product { get; set; }
        public DateTime AddDate { get; set; }
    }
}