using E_Ticaret.Entity;
using E_Ticaret.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{
    public class FavoritesController : Controller
    {
        DataContext db = new DataContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View(GetFavorites());
        }

        public ActionResult AddToFavorites(int Id)
        {


            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if (product != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    GetFavorites().AddProduct(product);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromFavorites(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if (product != null)
            {
                GetFavorites().DeleteProduct(product);
            }

            return RedirectToAction("Index");
        }


        public Favorites GetFavorites()
        {
            
            var favori = (Favorites)Session["Favorites"];

            if (favori == null)
            {
                favori = new Favorites();
                Session["Favorites"] = favori;
            }


            if (favori != null && !User.Identity.IsAuthenticated)
            {
                favori.Clear();

            }

            return favori;
        }




    }
}