using E_Ticaret.Entity;
using E_Ticaret.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        DataContext db = new DataContext();
        // GET: Order
        public ActionResult Index(string key, int page = 1)
        {
            var orders = db.Orders.Select(i => new AdminOrderModel()
            {
                Id = i.Id,
                Orderdate = i.Orderdate,
                OrderNumber= i.OrderNumber,
                OrderState = i.OrderState,
                Total = i.Total,
                UserName = i.UserName,
                Count = i.OrderLines.Count()
            }).OrderByDescending(i => i.Orderdate).AsQueryable();

            if (string.IsNullOrEmpty(key) == false)
            {
                orders = orders.Where(i => i.UserName.Contains(key) || i.OrderNumber.Contains(key));
            }

            return View(orders.ToList().ToPagedList(page, 6));
        }

        public ActionResult Details(int id)
        {
            var order = db.Orders
              .Where(i => i.Id == id)
              .Select(i => new OrderDetailsModel()
              {
                  UserName = i.UserName,
                  OrderId = i.Id,
                  Adres = i.Adres,
                  AdresBasligi = i.AdresBasligi,
                  Mahalle = i.Mahalle,
                  Orderdate = i.Orderdate,
                  Total = i.Total,
                  OrderNumber = i.OrderNumber,
                  OrderState = i.OrderState,
                  PostaKodu = i.PostaKodu,
                  Sehir = i.Sehir,
                  Semt = i.Semt,
                  OrderLines = i.OrderLines.Select(a => new OrderLineModel()
                  {
                      Image = a.Product.Image,
                      Price = a.Product.Price,
                      ProductId = a.ProductId,
                      ProdutName = a.Product.Name,
                      Quantity = a.Quantity
                  }).ToList()
              }).FirstOrDefault();


            return View(order);
        }

        public ActionResult UpdateOrderState(int OrderId,EnumOrderState OrderState)
        {
            var state = db.Orders.FirstOrDefault(i => i.Id == OrderId);
           if(state != null)
            {
                state.OrderState = OrderState;
                db.SaveChanges();

                TempData["message"] = "Bilgileriniz Kayıt Edildi.";

                return RedirectToAction("Details", new { id = OrderId });
            }

            return RedirectToAction("Index");
        }



    }
}