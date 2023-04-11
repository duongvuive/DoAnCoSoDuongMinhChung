using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.SQL;

namespace WebBanHangOnline.Controllers
{
    public class ThongTinKhachHangController : Controller
    {
        // GET: ThongTinKhachHang
       dbWebBanHang db=new dbWebBanHang();
        public ActionResult ProfileKH()
        {
            // Lấy thông tin của người dùng hiện tại
            // Lấy User ID của người đăng nhập
            string userId = User.Identity.GetUserId();

            // Khởi tạo đối tượng UserManager
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Lấy thông tin đầy đủ của User từ UserManager
            var user = userManager.FindById(userId);

            // Kiểm tra người dùng có tồn tại hay không
            if (user == null)
            {
                return HttpNotFound();
            }

            // Tạo view model để chứa thông tin của người dùng
            ProfileViewModel model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }
        public ActionResult ThemThongTinKH()
        {
            List<City> cityList = db.Cities.ToList();
            ViewBag.City = new SelectList(cityList, "CityId", "Name");
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemThongTinKH([Bind(Include = "FullName,Phone,Address,City,District,Ward")] Customer customer)
        {
            string AccountID = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                customer.CustomerID = Guid.NewGuid().ToString();
                customer.Id = AccountID;
                customer.DateCreated = DateTime.Now;
                customer.LastUpdated = DateTime.Now;
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            var cityList = db.Cities.ToList();
            ViewBag.City = new SelectList(cityList, "CityId", "Name");
            return View(customer);
        }
        public ActionResult GetDistrictsByCityId(int cityId)
        {
            List<District> districts = db.Districts.Where(m => m.CityId == cityId).ToList();
            ViewBag.District = new SelectList(districts, "DistrictId", "Name");
            return PartialView("DistrictState");
        }
        public ActionResult GetWardsByDistrictId(int DistrictId)
        {
            List<Ward>  wards= db.Wards.Where(m => m.DistrictId == DistrictId).ToList();
            ViewBag.Ward = new SelectList(wards, "WardId", "Name");
            return PartialView("WardState");
        }
        public ActionResult AccountProfile()
        {
            string id=User.Identity.GetUserId();
            // Khởi tạo đối tượng UserManager
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            // Lấy thông tin đầy đủ của User từ UserManager
            var user = userManager.FindById(id);

            var Customer=db.Customers.FirstOrDefault(m=>m.Id==id);
            
            if (Customer == null)
            {
                return RedirectToAction("ThemThongTinKH", "ThongTinKhachHang");
            }
            var City = db.Cities.FirstOrDefault(m => m.CityId == Customer.City);
            var Ward = db.Wards.FirstOrDefault(m => m.WardId == Customer.Ward);
            var District = db.Districts.FirstOrDefault(m => m.DistrictId == Customer.District);
            //View Model chứa thông tin người dùng
            CustomerProfile customerProfile = new CustomerProfile()
            {
                FullName = Customer.FullName,
                Email    =user.Email,
                Phone    = Customer.Phone,
                Address  = Customer.Address,
                City     = City.Name,
                Ward     = Ward.Name,
                District = District.Name,
            };

            return View(customerProfile);
        }
        public ActionResult ProfileEdit()
        {
            string id = User.Identity.GetUserId();
            
            var Customer = db.Customers.FirstOrDefault(m => m.Id == id);
            List<City> cityList = db.Cities.ToList();
            ViewBag.CityList = new SelectList(cityList,"CityId", "Name", Customer.City);
            ViewBag.DistrictList = new SelectList(db.Districts, "DistrictId", "Name");
            ViewBag.WardList = new SelectList(db.Wards, "WardId", "Name");
            return View(Customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileEdit([Bind(Include = "FullName,Phone,Address,City,District,Ward,LastUpdated")] Customer customer)
        {
            string id=User.Identity.GetUserId();
            var Customer = db.Customers.FirstOrDefault(m => m.Id == id);
            if (Customer != null)
            {
                Customer.FullName = customer.FullName;
                Customer.Phone = customer.Phone;
                Customer.Address = customer.Address;
                Customer.City = customer.City;
                Customer.Ward = customer.Ward;
                Customer.District = customer.District;
                Customer.LastUpdated = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(customer);
        }
    }
}