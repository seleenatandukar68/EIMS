using EIMS.Models;
using EIMS.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIMS.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult ChangePassword(LoginModel objModel)
        {
            return PartialView("ChangePassword", objModel);

        }

        public JsonResult UpdateUser(LoginModel objModel)
        {
            LoginDAL objDAL = new LoginDAL();
            return Json(objDAL.UpdateUser(objModel), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdatePassword(string username, string email, string role)
        {
            LoginDAL objDAL = new LoginDAL();
            return Json( objDAL.UpdatePassword(username, int.Parse(role.Trim()), email),JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResetPassword()
        {
            return View("ResetPassword");
        }
    }
}