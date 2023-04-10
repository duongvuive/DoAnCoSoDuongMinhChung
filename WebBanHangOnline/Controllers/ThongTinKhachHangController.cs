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
            ViewBag.Ward = new SelectList(wards, "DistrictId", "Name");
            return PartialView("WardState");
        }

    }
}