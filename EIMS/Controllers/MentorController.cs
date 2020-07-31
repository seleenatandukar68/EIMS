using EIMS.Models;
using EIMS.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIMS.Controllers
{
    public class MentorController : Controller
    {
        // Kishor
        MentorDAL objDAL = new MentorDAL();
        HomeDAL objDAL1 = new HomeDAL();
        // GET: Mentor
        public ActionResult Index()
        {
            return View("add");
        }
        [Rule]
        public ActionResult AssignProject()
        {
            return View("AssignProject");
        }

        public ActionResult AddTask()
        {
          //  ProjectTaskModel objModel = new ProjectTaskModel();
            InternDAL intObj = new InternDAL();
            ProjTaskDAL projObj = new ProjTaskDAL();
            ViewBag.projectList = projObj.GetProjectByMentorId(System.Web.HttpContext.Current.Session["UserName"].ToString().Substring(3)).AsEnumerable().Select(row =>
            new SelectListItem {
                Selected = false,
                Text = row.ProjectName,
                Value = row.ProjId.ToString()
            });
            ViewBag.data = intObj.GetAllPreferences().AsEnumerable().Select(row =>
                new SelectListItem
                {
                    Selected = false,
                    Text = row["PreferenceCategory"].ToString(),
                    Value = row["PreId"].ToString()
                }); 
            return View("AddTask");
        }

        public JsonResult SaveTask(ProjectTaskModel objAtt)
        {
            ProjTaskDAL objDAL = new ProjTaskDAL();
            
            return Json(objDAL.SaveAssignedTask(objAtt), JsonRequestBehavior.AllowGet);

        }
        public JsonResult PublishTask(ProjectTaskModel objAtt)
        {
            ProjTaskDAL objDAL = new ProjTaskDAL();

            return Json(objDAL.PublishTask(objAtt), JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteTask(string taskid)
        {
            ProjTaskDAL objDAL = new ProjTaskDAL();

            return Json(objDAL.DeleteTask(taskid), JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAssignedTask(ProjectTaskModel objAtt)
        {
            ProjTaskDAL objDAL = new ProjTaskDAL();

            return Json(objDAL.GetAssignedTask(objAtt), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EvaluateInterns(string mentorId)
        {
            InternDAL intObj = new InternDAL();
            ProjTaskDAL projObj = new ProjTaskDAL();
            ViewBag.projectList = projObj.GetProjectByMentorId(System.Web.HttpContext.Current.Session["UserName"].ToString().Substring(3)).AsEnumerable().Select(row =>
            new SelectListItem
            {
                Selected = false,
                Text = row.ProjectName,
                Value = row.ProjId.ToString()
            });
            ViewBag.data = intObj.GetAllPreferences().AsEnumerable().Select(row =>
                new SelectListItem
                {
                    Selected = false,
                    Text = row["PreferenceCategory"].ToString(),
                    Value = row["PreId"].ToString()
                });

            return View("EvaluateInterns");
        }

        //Kishor
        [Rule]
        public ActionResult AddMentor(MentorModel model)
        {

            ViewBag.Message = objDAL.AddMentor(model);
            ModelState.Clear();
            return View("add");
        }
        public ActionResult view(string msg)
        {

            IEnumerable<MentorModel> data = objDAL.GetAllMentor().AsEnumerable().Select(row =>
             new MentorModel
             {
                 Id = int.Parse(row["Id"].ToString()),
                 MentorId = row["MentorId"].ToString(),
                 FName = row["FName"].ToString(),
                 MName = row["MName"].ToString(),
                 LName = row["LName"].ToString(),
                 Address = row["Address"].ToString(),
                 Email = row["Email"].ToString(),
                 Contact = row["Contact"].ToString()

             });
            if (msg != "")
            {
                ViewBag.Message = msg;
            }
            return View("ViewMentors", data);
        }


        public ActionResult ViewMentor(int id, string name)
        {
            DataTable dt = objDAL.GetMentorByID(id);
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
            return View("ViewMentor", objModel);

        }
        public ActionResult ViewPartialMentor(int id)
        {
            DataTable dt = objDAL.GetMentorByID(id);
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
            return PartialView("ViewPartialMentor", objModel);
        }
        [Rule]
        public ActionResult EditDelete(MentorModel model, FormCollection form)
        {
            string action = form["btn"].ToString();
            ViewBag.Message = objDAL.EditDelete(model, action);
            ModelState.Clear();
            return view(ViewBag.Message);
        }
        public ActionResult EditMentorProfile (MentorModel model, FormCollection form)
        {
            string action = form["btn"].ToString();
            ViewBag.Message = objDAL.EditDelete(model, action);
            ModelState.Clear();
            return RedirectToAction("MentorDashboard", "Home");
        }


        public ActionResult assign()
        {

            ViewBag.projList = objDAL.GetMentorByProjId().AsEnumerable().Select(row =>
        new MentorModel
        {
            Id = int.Parse(row["Id"].ToString()),
            FName = row["FName"].ToString(),
            MName = row["MName"].ToString(),
            LName = row["LName"].ToString()

        });
            IEnumerable<ProjectModel> List = objDAL1.GetAllProjects().AsEnumerable().Select(row =>
            new ProjectModel
            {
                ProjId = int.Parse(row["ProjId"].ToString()),
                ProjectName = row["ProjectName"].ToString(),


            });
            ViewBag.ProjIdList = new SelectList(List, "ProjId", "ProjectName");

            return View("assign_Mentor");
        }

        public ActionResult Assign_Mentor(int Id)
        {
            ViewBag.projList = objDAL.GetMentorByProjId().AsEnumerable().Select(row =>
          new MentorModel
          {
              Id = int.Parse(row["Id"].ToString()),
              FName = row["FName"].ToString(),
              MName = row["MName"].ToString(),
              LName = row["LName"].ToString()

          });
            DataTable dt = objDAL.GetMentorByIDFromMentorModel(Id);
            MentorModel objModel = new MentorModel();

            foreach (DataRow row in dt.Rows)
            {
                objModel.Id = int.Parse(row["Id"].ToString());
                objModel.FName = row["FName"].ToString();
                objModel.MName = row["MName"].ToString();
                objModel.LName = row["LName"].ToString();

            }
            IEnumerable<ProjectModel> List = objDAL1.GetAllProjects().AsEnumerable().Select(row =>
            new ProjectModel
            {
                ProjId = int.Parse(row["ProjId"].ToString()),
                ProjectName = row["ProjectName"].ToString(),


            });
            ViewBag.ProjIdList = new SelectList(List, "ProjId", "ProjectName");


            return View("assign_Mentor", objModel);

        }

        public ActionResult AddProject_Mentor(MentorModel model)
        {
            ViewBag.projList = objDAL.GetMentorByProjId().AsEnumerable().Select(row =>
       new MentorModel
       {
           Id = int.Parse(row["Id"].ToString()),
           FName = row["FName"].ToString(),
           MName = row["MName"].ToString(),
           LName = row["LName"].ToString()

       });
            IEnumerable<ProjectModel> List = objDAL1.GetAllProjects().AsEnumerable().Select(row =>
            new ProjectModel
            {
                ProjId = int.Parse(row["ProjId"].ToString()),
                ProjectName = row["ProjectName"].ToString(),


            });
            ViewBag.ProjIdList = new SelectList(List, "ProjId", "ProjId");

            ViewBag.Message = objDAL.MentorAssign(model);
            ModelState.Clear();

            return View("AddProject_Mentor");
        }

        public ActionResult viewAssignedMentor()
        {
            return View("viewAssignedMentor");


        }
        public JsonResult MentorList()
        {
            List<MentorModel> AssignedMentor = new List<MentorModel>();
            AssignedMentor = objDAL.getAllAssignedMentorInfo();
            return Json(AssignedMentor, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DisplayMentorList()
        {
            List<MentorModel> MentorList = new List<MentorModel>();
            MentorList = objDAL.getAllMentorInfo();
            return Json(MentorList, JsonRequestBehavior.AllowGet);
        }
    }
}