using E_Ticaret.Entity;
using E_Ticaret.Identity;
using E_Ticaret.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Ticaret.Controllers
{
    public class AccountController : Controller
    {

        private DataContext db = new DataContext();

        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;


        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new Identity.IdentityDbContext());
            userManager = new UserManager<ApplicationUser>(userStore);

            var roleStore = new RoleStore<ApplicationRole>(new Identity.IdentityDbContext());
            roleManager = new RoleManager<ApplicationRole>(roleStore);
        }



        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var orders = db.Orders
                .Where(i => i.UserName == username)
                .Select(i => new UserOrderModel()
                {
                    Id = i.Id,
                    OrderNumber = i.OrderNumber,
                    Orderdate = i.Orderdate,
                    OrderState = i.OrderState,
                    Total = i.Total
                }).OrderByDescending(i => i.Orderdate).ToList();


            if (userManager.FindByName(User.Identity.Name).Image != null)
            {
                ViewBag.user = User.Identity.Name + ".jpg";
            }
            else
            {
                ViewBag.user = "pp.jpg";
            }

            return View(orders);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var order = db.Orders
                .Where(i => i.Id == id)
                .Select(i => new OrderDetailsModel()
                {
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


        // GET: Account
        public ActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {

                //Kayıt İşlemleri
                ApplicationUser user = new ApplicationUser();

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.UserName = model.Username;
                //Foto işlemleri
                var path = Server.MapPath("~/Upload/Users");
                var filename = Path.ChangeExtension(user.UserName + "", ".jpg");
                var fullpath = Path.Combine(path, filename);

                file.SaveAs(fullpath);

                model.UserImage = filename.ToString();
                user.Image = model.UserImage;


                IdentityResult result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    //Kullanıcı oluştu ve rol atanabilir.
                    if (roleManager.RoleExists("user"))
                    {

                        userManager.AddToRole(user.Id, "user");
                    }

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("RegisterUserError", "Kullanıcı oluşturulamadı.");
                }


            }


            return View(model);
        }


        // GET: Account
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                //Login işlemleri

                var user = userManager.Find(model.Username, model.Password);

                if (user != null)
                {
                    //Varolan kullanıcı sisteme dahil et  
                    // AppCookie oluşturup sisteme bırak.

                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identitycalims = userManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = model.RememberMe;
                    authManager.SignIn(authProperties, identitycalims);

                    var role = user.Roles.ToString();

                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction("Index", "Home");


                }
                else
                {
                    ModelState.AddModelError("LoginUserError", "Giriş Başarısız.");
                }


            }


            return View(model);
        }

        // GET: Account
        public ActionResult LogOut()
        {


            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            //Session.Abandon();


            return RedirectToAction("Index", "Home");
        }






    }
}