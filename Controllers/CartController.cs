using E_Ticaret.Entity;
using E_Ticaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{   
    public class CartController : Controller
    {

        DataContext db = new DataContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View(GetCart());
        }

        public ActionResult AddToCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if(product != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    GetCart().AddProduct(product, 1);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if (product != null)
            {
                GetCart().DeleteProduct(product);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromOneCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if (product != null)
            {
                GetCart().DeleteOneProduct(product,1);
            }

            return RedirectToAction("Index");
        }



        public Cart GetCart()
        {
            var cart = (Cart)Session["Cart"];


            if(cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            if (cart != null)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    cart.Clear();
                }
              
            }
            return cart;
        }

        public PartialViewResult Summary()
        {
            return PartialView(GetCart());
        }

        public ActionResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ActionResult Checkout(ShippingDetails entity)
        {
            var cart = GetCart();

            if(cart.Cartlines.Count() == 0)
            {
                ModelState.AddModelError("UrunYokError", "Sepetinizde ürün bulunmamaktadır.");
            }
            if (ModelState.IsValid)
            {
                SaveOrder(cart,entity);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(entity);
            }
               
        }

        private void SaveOrder(Cart cart, ShippingDetails entity)
        {
            var order = new Order();

            order.OrderNumber = "S"+(new Random()).Next(11111,99999).ToString();
            order.Total = cart.Total();
            order.Orderdate = DateTime.Now;
            order.OrderState = EnumOrderState.Bekleniyor;

            order.UserName = User.Identity.Name;
            order.AdresBasligi = entity.AdresBasligi;
            order.Adres = entity.Adres;
            order.Mahalle = entity.Mahalle;
            order.Sehir = entity.Sehir;
            order.Semt = entity.Semt;
            order.PostaKodu = entity.PostaKodu;

            order.OrderLines = new List<OrderLine>();

            foreach(var pr in cart.Cartlines)
            {
                var orderline = new OrderLine();
                orderline.Quantity = pr.Quantity;
                orderline.Price = pr.Quantity * pr.Product.Price;
                orderline.ProductId = pr.Product.Id ;

                order.OrderLines.Add(orderline);
            }
            db.Orders.Add(order);
            db.SaveChanges();
           
        }
    }
}