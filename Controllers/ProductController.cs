using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using E_Ticaret.Entity;
using E_Ticaret.Models;
using System.IO;

namespace E_Ticaret.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private DataContext db = new DataContext();


        public ActionResult AdminHome(string key , int  page =1 )
        {   
            var orders = db.Orders.Select(i => new AdminOrderModel()
            {
                Id = i.Id,
                Orderdate = i.Orderdate,
                OrderNumber = i.OrderNumber,
                OrderState = i.OrderState,
                Total = i.Total,
                UserName = i.UserName,
                Count = i.OrderLines.Count()
            }).OrderByDescending(i => i.Orderdate).AsQueryable();

            if (string.IsNullOrEmpty(key) == false)
            {
                orders = orders.Where(i => i.OrderNumber.Contains(key) || i.UserName.Contains(key) || i.OrderState.ToString().Contains(key));
            }


            return View(orders.ToList().ToPagedList(page, 8));
        }

        // GET: Product
        public ActionResult Index(string key, int page=1)
        {
            var products = db.Products.Include(p => p.Category);

            if (string.IsNullOrEmpty(key) == false)
            {
                products = products.Where(i => i.Name.Contains(key) || i.Description.Contains(key) || i.BDescription.Contains(key));
            }

            return View(products.ToList().OrderByDescending(i => i.Id).ToPagedList(page,6));
        }


        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }







        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,Stock,IsApproved,ProductCode,IsHome,CategoryId")] Product product, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {

                var path = Server.MapPath("~/Upload");
                var filename = Path.ChangeExtension(product.ProductCode + "", ".jpg");


                if (file == null)
                {
                    filename = "place.jpg";
                }
                var fullpath = Path.Combine(path, filename);

                file.SaveAs(fullpath);

                product.Image = filename.ToString();
                product.ProductCode = "#" + product.ProductCode +"";
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,Stock,IsApproved,IsHome,Image,CategoryId,BDescription")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
