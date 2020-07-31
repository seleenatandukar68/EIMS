using EIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EIMS.DataModel;
using EIMS.Models.DAL;
using EIMS.App_Start;

namespace EIMS.Controllers
{
 
    public class LoginController : Controller
    {
        // GET: Login
        [LoginRule]
        public ActionResult Index()

        {
            return View("Login");
        }
        public ActionResult Login(LoginModel model)
        {
            // if (ModelState.IsValid) //validating the user inputs  
            LoginDAL objDAL = new LoginDAL();
            string name = model.UserName;

            string password = model.Password;
            bool isExist = false;
            using (myDBEntities _entity = new myDBEntities())  // out Entity name is "SampleMenuMasterDBEntites"  
            {
                isExist = objDAL.verifyLogin(model);
                if (isExist)
                {
                    LoginModel _loginCredentials = _entity.tblUsers.Where(x => x.Username.Trim().ToLower() == name.Trim().ToLower()  ).Select(x => new LoginModel
                    {
                        UserName = x.Username,
                        RoleName = x.tblRole.Role,
                        UserRoleId = x.RoleId,
                        UserId = x.UserId
                    }).FirstOrDefault();  // Get the login user details and bind it to LoginModels class  
                    List<MenuModel> _menus = _entity.tblSubMenus.Where(x => x.RoleId == _loginCredentials.UserRoleId).Select(x => new MenuModel
                    {
                        MainMenuId = x.tblMainMenu.MenuId,
                        MainMenuName = x.tblMainMenu.MainMenu,
                        SubMenuId = x.SubId,
                        SubMenuName = x.SubMenu,
                        ControllerName = x.Controller,
                        ActionName = x.Action,
                        RoleId = x.RoleId,
                        RoleName = x.tblRole.Role
                    }).ToList(); //Get the Menu details from entity and bind it in MenuModels list.  
                    FormsAuthentication.SetAuthCookie(_loginCredentials.UserName, false); // set the formauthentication cookie  
                    HttpContext context = System.Web.HttpContext.Current;
                    context.Session["LoginCredentials"] = _loginCredentials; // Bind the _logincredentials details to "LoginCredentials" session  
                    context.Session["MenuMaster"] = _menus; //Bind the _menus list to MenuMaster session  
                    context.Session["UserName"] = _loginCredentials.UserName;
                    context.Session["RoleId"] = _loginCredentials.UserRoleId;
                    if (_loginCredentials.UserRoleId == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    if (_loginCredentials.UserRoleId == 2)
                    {
                        var internId = _loginCredentials.UserName.Substring(3);
                        return RedirectToAction("InternDashboard", "Home");
                    }

                    if (_loginCredentials.UserRoleId == 3)
                    {
                        var mentorId = _loginCredentials.UserName;
                        return RedirectToAction("MentorDashboard", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Please enter the valid credentials!...";
                    return View("Login");
                }
            }

            return View();
        }

        public ActionResult LogOff()

        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session["LoginCredentials"] = null;
            System.Web.HttpContext.Current.Session["MenuMaster"] = null;
            System.Web.HttpContext.Current.Session["UserName"] = null;
            System.Web.HttpContext.Current.Session["RoleId"] = null;
            var httpSession = System.Web.HttpContext.Current.Session;
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            System.Web.HttpContext.Current.Session.RemoveAll();
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();

            
         

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));

            Response.Cache.SetNoStore();


            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}