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
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ThongTinKhachHangController : Controller
    {
        // GET: ThongTinKhachHang
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

    }
}