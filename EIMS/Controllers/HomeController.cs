using EIMS.Models.DAL;
using EIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using EIMS.DataModel;
using System.IO;
using System.Data;

namespace EIMS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        HomeDAL objDAL = new HomeDAL();
        public ActionResult Index()
        {
            
            return View("Index");
        }
        public ActionResult HomePage()
        {
            if (System.Web.HttpContext.Current.Session["UserName"].ToString() != "")
            {

                if (int.Parse(System.Web.HttpContext.Current.Session["RoleId"].ToString()) == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (int.Parse(System.Web.HttpContext.Current.Session["RoleId"].ToString()) == 2)
                {
                    //var internId = _loginCredentials.UserName.Substring(3);
                    return RedirectToAction("InternDashboard", "Home");
                }

                if (int.Parse(System.Web.HttpContext.Current.Session["RoleId"].ToString()) == 3)
                {
                    //var mentorId = _loginCredentials.UserName;
                    return RedirectToAction("MentorDashboard", "Home");

                }
            }
            return   RedirectToAction("LogOff", "Login"); ;
        }
        public JsonResult GetNotification()
        {
            List<CommentModel> list = new List<CommentModel>();
            //  var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            if (!(string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["UserName"].ToString())))
            {
                string username = System.Web.HttpContext.Current.Session["UserName"].ToString().Substring(3);
                list = NC.GetUnreadComments(username);
            }
            //update session here for get only new added contacts (notification)
            //   Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetNotificationCount(string mentorId)
        {
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetUnreadComments(mentorId);

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ProjectList()
        {
            List<ProjectModel> lstProj = new List<ProjectModel>();
            lstProj = objDAL.getAllProject();
            return Json(lstProj, JsonRequestBehavior.AllowGet);

        }
        public ActionResult InternDashboard()
        {

            LoginModel _loginCredentials = new LoginModel();
            _loginCredentials = (LoginModel)System.Web.HttpContext.Current.Session["LoginCredentials"];
            string Internid = _loginCredentials.UserName.Substring(3);
            return RedirectToAction("getProjTaskDetById", "ProjTask", new { Internid });
            //ViewBag.InternId = Internid;
            //return View("InternDashboard");

        }

        public ActionResult MentorDashboard()
        {
            LoginModel _loginCredentials = new LoginModel();
            _loginCredentials = (LoginModel)System.Web.HttpContext.Current.Session["LoginCredentials"];
            string MentorId = _loginCredentials.UserName.Substring(3);
            MentorDAL objDAL = new MentorDAL();
            DataTable dt = objDAL.GetMentorByID(int.Parse(MentorId.Trim()));
            MentorModel objModel = new MentorModel();
            foreach (DataRow row in dt.Rows)
            {

                objModel.Id = int.Parse(row["Id"].ToString());
                objModel.MentorId = row["MentorId"].ToString();
                objModel.FName = row["FName"].ToString();
                objModel.MName = row["MName"].ToString();
                objModel.LName = row["LName"].ToString();
                objModel.Address = row["Address"].ToString();
                objModel.Email = row["Email"].ToString();
                objModel.Contact = row["Contact"].ToString();


            }
            return View("MentorDashboard", objModel);

        }
        public ActionResult Profile()
        {
            LoginModel _loginCredentials = new LoginModel();
            _loginCredentials = (LoginModel)System.Web.HttpContext.Current.Session["LoginCredentials"];
            string InternId = _loginCredentials.UserName.Substring(3);
            InternDAL objDAL = new InternDAL();
            DataTable dt = objDAL.GetInternByID(int.Parse(InternId.Trim()));
            InternModel objModel = new InternModel();
            foreach (DataRow row in dt.Rows)
            {

                objModel.Id = int.Parse(row["Id"].ToString());
                objModel.InternId = row["InternId"].ToString();
                objModel.FName = row["FName"].ToString();
                objModel.MName = row["MName"].ToString();
                objModel.LName = row["LName"].ToString();
                objModel.Address = row["Address"].ToString();
                objModel.Email = row["Email"].ToString();
                objModel.Contact = row["Contact"].ToString();
                objModel.Qualification = row["Qualification"].ToString();

            }
            return View("Profile", objModel);
        }

        public ActionResult ViewReport()
        {
            ProjTaskDAL objDAL = new ProjTaskDAL();
            myEIMSEntity dbModel = new myEIMSEntity();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "Certificate.rpt"));

            rd.RecordSelectionFormula = "{tblInternInfo.ProjId} = 2";
            // var saveObject = Newtonsoft.Json.JsonConvert.DeserializeObject<DestinationClass>(o);

            //IEnumerable<ProjTaskModel> data = objDAL.GetProjTaskDetById("1002").AsEnumerable().Select(row =>
            //   new ProjTaskModel
            //   {
            //       ProjId = int.Parse(row["ProjId"].ToString()),
            //       ProjectName = row["ProjectName"].ToString(),
            //       WeekId = int.Parse(row["WeekId"].ToString()),
            //       TaskId = int.Parse(row["TaskId"].ToString()),
            //           //TaskName = row["TaskName"].ToString(),
            //           TaskDes = row["TaskDes"].ToString(),
            //       Status = int.Parse(row["Status"].ToString())


            //   }).ToList();
            //rd.SetDataSource(data);
            //rd.ParameterFields("ProjectId");

            Response.Buffer = false;
            Response.ClearHeaders();
            Response.ClearContent();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Certificate.pdf");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Certificate()
        {
            return View("Certificate");
        }
        public JsonResult PrintCertificate( CertificateModel obj)
        {
            InternDAL objIntDAL = new InternDAL();
            DataTable dt = objIntDAL.GetInternByID(int.Parse(obj.InternId.Trim().Substring(3)));
           // List<CertificateModel> lstObj = new List<CertificateModel>();
            foreach (DataRow dr in dt.Rows)
            {
                obj.FName = dr["FName"].ToString();
                obj.LName = dr["LName"].ToString();
                obj.prefCategory = dr["PreferenceCategory"].ToString();
               // lstObj.Add(obj);
            }

            return new JsonResult { Data=obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult AddProject(ProjectModel model)
        {
            ViewBag.Message = objDAL.AddProject(model);
            ModelState.Clear();
            return View("Index");
        }
        public ActionResult AddEditProject(int ProjId)
        {
            DataTable dt = objDAL.GetProjectByID(ProjId);
            ProjectModel objModel = new ProjectModel();
            foreach (DataRow row in dt.Rows)
            {

                objModel.ProjId = int.Parse(row["ProjId"].ToString());
                objModel.ProjectName = row["ProjectName"].ToString();
                objModel.StartDate = DateTime.Parse(row["StartDate"].ToString()).Date;
                objModel.Status = row["Status"].ToString();
                objModel.EndDate = DateTime.Parse(row["EndDate"].ToString()).Date;


            }

            return PartialView("Partial2", objModel);
        }
        public ActionResult Update(ProjectModel model)
        {

            ViewBag.Message = objDAL.Update(model);
            ModelState.Clear();
            return View("Index");
        }
        public ActionResult Delete(int ProjId)
        {
            DataTable dt = objDAL.GetProjectByID(ProjId);
            ProjectModel objModel = new ProjectModel();
            foreach (DataRow row in dt.Rows)
            {

                objModel.ProjId = int.Parse(row["ProjId"].ToString());
                objModel.ProjectName = row["ProjectName"].ToString();
                objModel.StartDate = DateTime.Parse(row["StartDate"].ToString()).Date;
                objModel.EndDate = DateTime.Parse(row["EndDate"].ToString()).Date;


            }

            return PartialView("Partial3", objModel);

        }
        public ActionResult DeleteProject(ProjectModel model)
        {

            ViewBag.Message = objDAL.Delete(model);
            ModelState.Clear();
            return View("Index");
        }
        public ActionResult Project_week_details1(int ProjId, int WeekId)
        {
            ViewBag.ProjectName = objDAL.GetProjectByID(ProjId).AsEnumerable().Select(row =>
       new ProjectModel
       {
           ProjectName = row["ProjectName"].ToString(),
           ProjId = int.Parse(row["ProjId"].ToString())
       });

            ViewBag.WeekNum = objDAL.GetWeekNumber(WeekId, ProjId).AsEnumerable().Select(row =>
           new ProjTaskModel
           {
               WeekId = int.Parse(row["WeekId"].ToString()),
               TaskId = int.Parse(row["TaskId"].ToString()),
               TaskDes = row["TaskDes"].ToString(),
               Status = int.Parse(row["Status"].ToString()),
               InternId = int.Parse(row["InternId"].ToString()),
               ProjId = int.Parse(row["ProjId"].ToString())
           });


            return View("Project_week_details2");
        }

        public ActionResult ProjectInfo(int ProjId)
        {
            ViewBag.ProjectName = objDAL.GetProjectByID(ProjId).AsEnumerable().Select(row =>
       new ProjectModel
       {

           ProjectName = row["ProjectName"].ToString(),
           ProjId=int.Parse(row["ProjId"].ToString())


       });


            ViewBag.InternList = objDAL.GetAssignedInternByProjId(ProjId).AsEnumerable().Select(row =>
       new InternModel
       {
           Id = int.Parse(row["Id"].ToString()),
           FName = row["FName"].ToString(),
           ProjRole = row["ProjRole"].ToString()


       });
            ViewBag.MentorList = objDAL.GetAssignedMentorByProjId(ProjId).AsEnumerable().Select(row =>
       new MentorModel
       {
           Id = int.Parse(row["Id"].ToString()),
           FName = row["FName"].ToString()



       });

            return View("ProjectInfo");

        }
    }
}
