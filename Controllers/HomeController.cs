using E_Ticaret.Entity;
using E_Ticaret.Models;
using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace E_Ticaret.Controllers
{
    public class HomeController : Controller
    {
        DataContext db = new DataContext();
        public PartialViewResult GetComments()
        {
            var id = Url.RequestContext.RouteData.Values["id"];
            var comments = db.Comments.AsEnumerable().Where(x => x.ProductId == int.Parse(id.ToString()) & x.isApproved == true ).ToList();

            ViewBag.yorum = comments.Count();

            return PartialView(comments);
        }

        public ActionResult DeleteComment(int id)
        {
            Comments comments = db.Comments.Find(id);
            if (User.Identity.IsAuthenticated)
            {
                db.Comments.Remove(comments);
                db.SaveChanges();

            }
            else
            {
                return RedirectToAction("/Index");
            }

            return RedirectToAction("/Index");
        }


        public ActionResult AddComment()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "Id,Head,Description")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                var id = int.Parse(Url.RequestContext.RouteData.Values["id"].ToString());
                comments.ProductId = int.Parse(Url.RequestContext.RouteData.Values["id"].ToString());
                comments.UserName = User.Identity.Name;
                comments.isApproved = false;
                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Details/"+id);
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", comments.ProductId);
            return View(comments);
        }


        // GET: Home
        public ActionResult Index(string key)
        {

            var urunler = db.Products
                .Where(i => i.IsApproved && i.IsHome)
                .Select(i => new ProductModel()
                {
                    Id = i.Id,
                    Name = i.Name.Length > 25 ? i.Name.Substring(0, 25) + "..." : i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    Stock = i.Stock,
                    ProductCode = i.ProductCode,
                    Image = i.Image ?? "place.jpg",
                    CategoryId = i.CategoryId,
                    BDescription = i.BDescription
                }).AsQueryable();

            ViewBag.aranan = key;

            if (string.IsNullOrEmpty(key) == false)
            {
                urunler = urunler.Where(i => i.Name.Contains(key) || i.Description.Contains(key) || i.BDescription.Contains(key));
            }

            return View(urunler.ToList().OrderByDescending(i => i.Id));
        }

        public ActionResult Details(int? id, string key)
        {

            var detay = db.Products.Where(i => i.Id == id).AsQueryable();

            if (string.IsNullOrEmpty(key) == false)
            {
                detay = detay.Where(i => i.Name.Contains(key) || i.Description.Contains(key) || i.BDescription.Contains(key));
            }


            return View(detay.FirstOrDefault());
        }

        public ActionResult List(int? id, string key,int page =1)
        {
            var urunler = db.Products
               .Where(i => i.IsApproved )
               .Select(i => new ProductModel()
               {
                   Id = i.Id,
                   Name = i.Name.Length > 25 ? i.Name.Substring(0, 25) + "..." : i.Name,
                   Description = i.Description,
                   Price = i.Price,
                   Stock = i.Stock,
                   ProductCode = i.ProductCode,
                   Image = i.Image ?? "place.jpg",
                   CategoryId = i.CategoryId,
                   BDescription = i.BDescription
               }).AsQueryable();

            ViewBag.aranan = key;

            if (string.IsNullOrEmpty(key) == false)
            {
                urunler = urunler.Where(i => i.Name.Contains(key) || i.Description.Contains(key) || i.BDescription.Contains(key));
            }


            if (id != null)
            {
                urunler = urunler.Where(i => i.CategoryId == id);
            }


            return View(urunler.ToList().OrderByDescending(i => i.Id).ToPagedList(page, 16));
        }

        public PartialViewResult HotDeals()
        {
           
            var hots = db.Products.OrderByDescending(x=> x.Price).Take(5);
            return PartialView(hots);

        }

        public PartialViewResult GetCategories()
        {
            return PartialView(db.Categories.ToList());
        }



    }
}