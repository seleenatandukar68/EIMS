using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIMS.Models;
using EIMS.Models.DAL;
using System.Data;

namespace EIMS.Controllers
{
    public class ProjTaskController : Controller
    {
        // GET: ProjTask
        ProjTaskDAL objDAL = new ProjTaskDAL();
        public ActionResult Index()
        {
            return View();
        }
     
        public ActionResult getProjTaskDetById(string Internid)
        {
            InternDAL intObj = new InternDAL();
            // var saveObject = Newtonsoft.Json.JsonConvert.DeserializeObject<DestinationClass>(o);
       
                IEnumerable<ProjTaskModel> data = objDAL.GetProjTaskDetById(Internid).AsEnumerable().Select(row =>
                   new ProjTaskModel
                   {
                       ProjId = int.Parse(row["ProjId"].ToString()),
                       ProjectName = row["ProjectName"].ToString(),
                       WeekId = int.Parse(row["WeekId"].ToString()),
                       TaskId = int.Parse(row["TaskId"].ToString()),
                       //TaskName = row["TaskName"].ToString(),
                       TaskDes = row["TaskDes"].ToString(),
                       Status  = int.Parse(row["Status"].ToString())


                   }).ToList();
            ViewBag.flag = intObj.CheckPreferenceSetting(Internid);
            ViewBag.internId = Internid;
           
            ViewBag.data = intObj.GetAllPreferences().AsEnumerable().Select(row =>
            new SelectListItem
            {
                Selected = false,
                Text = row["PreferenceCategory"].ToString(),
                Value = row["PreId"].ToString()
            }); ;
                return View("InternHome", data);
           
          //  return Json(data, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult getWeeklyTaskById (string ProjId, string Internid, string WeekId)
        {
            InternDAL intObj = new InternDAL();
            // var saveObject = Newtonsoft.Json.JsonConvert.DeserializeObject<DestinationClass>(o);

            IEnumerable<ProjTaskModel> data = objDAL.GetProjTaskDetById(Internid).AsEnumerable().Select(row =>
               new ProjTaskModel
               {
                   ProjId = int.Parse(row["ProjId"].ToString()),
                   ProjectName = row["ProjectName"].ToString(),
                   WeekId = int.Parse(row["WeekId"].ToString()),
                   TaskId = int.Parse(row["TaskId"].ToString()),
                   //TaskName = row["TaskName"].ToString(),
                   TaskDes = row["TaskDes"].ToString(),
                   Status = int.Parse(row["Status"].ToString()),
                   InternId = int.Parse(Internid.Trim()),
                   


               }).Where(x=> x.WeekId == int.Parse(WeekId)).ToList();
            ViewBag.InternId = Internid;
            ViewBag.WeekId = WeekId;
            ViewBag.ProjId = ProjId;
            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["RoleId"]) == 2)
            {
                return View("InternWeeklyTask", data);
            }
            else
            {
                return View("WeeklyEvaluation", data);
            }
        }

        public JsonResult UpdateTaskStatus(List<ProjTaskModel> taskTasks)
        {
            ProjTaskDAL objProjTask = new ProjTaskDAL();
            return Json(objProjTask.UpdateTaskStatus(taskTasks), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddComment(string ProjectId, string Internid, string WeekId)
        {
            CommentDAL objDAL = new CommentDAL();
            CommentModel data = new CommentModel();

            data.weekId = int.Parse(WeekId.Trim());
            data.internId = int.Parse(Internid.Trim());
            data.projId = int.Parse(ProjectId.Trim());
           
            data = objDAL.getCommentByID(data);
            return View("AddComment",data);
           //return View("Try");
        }


    }
}