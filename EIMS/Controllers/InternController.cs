using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIMS.Models.DAL;
using System.Data;
using EIMS.Controllers;
using System.IO;
using System.Data.SqlClient;

namespace EIMS.Models
{

    public class InternController : Controller
    {
        InternDAL objDAL = new InternDAL();
       
        HomeDAL objDAL1 = new HomeDAL();

        
        // GET: Intern
        public ActionResult Index()
        {
            return View("Intern");
        }
        [Rule]
        public ActionResult AddIntern(InternModel model)
        {
            InternModel objRes = new InternModel();
            objRes = objDAL.AddIntern(model);
            foreach (HttpPostedFileBase file in objRes.files)
            {
                FileUpload(file, objRes.Id);
            }
            ViewBag.Message = "Intern ID : INT" + objRes.Id;
            ModelState.Clear();

            return View("Intern");
        }
        [HttpPost]
        public void FileUpload(HttpPostedFileBase files, int id)
        {

          String FileExt = Path.GetExtension(files.FileName).ToUpper();

            if (FileExt == ".PDF")
            {
                Stream str = files.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                FileDetailsModel Fd = new Models.FileDetailsModel();
                Fd.FileName = files.FileName;
                Fd.FileContent = FileDet;
                SaveFileDetails(Fd, id);

            }
            else
            {

                ViewBag.FileStatus = "Invalid file format.";


            }

        }
        private void SaveFileDetails(FileDetailsModel objDet, int id)
        {
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();

                SqlParameter[] param = new SqlParameter[3];

                param[0] = new SqlParameter("@InternId", id);
                param[1] = new SqlParameter("@FileName", objDet.FileName);
                param[2] = new SqlParameter("@FileContent", objDet.FileContent);


                DAO.executeTranProcedure("AddFileDetails", param, transaction, conn);
                transaction.Commit();
            }


        }
        [Rule]
        public ActionResult ViewInternList(string msg)
        {

            IEnumerable<InternModel> data = objDAL.GetAllIntern().AsEnumerable().Select(row =>
             new InternModel
             {
                 Id = int.Parse(row["Id"].ToString()),
                 InternId = row["InternId"].ToString(),
                 FName = row["FName"].ToString(),
                 MName = row["MName"].ToString(),
                 LName = row["LName"].ToString(),
                 Address = row["Address"].ToString(),
                 Email = row["Email"].ToString(),
                 Contact = row["Contact"].ToString(),
                 Qualification = row["Qualification"].ToString()

             });
            if (msg != "")
            {
                ViewBag.Message = msg;
            }
            return View("InternList", data);
        }

        public ActionResult ViewIntern(int id, string name)
        {
            DataTable dt = objDAL.GetInternByID(id);
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
            return View("ViewIntern", objModel);

        }
        #region Methods related to file handling 
        [HttpGet]
        public FileResult DownLoadFile(int id, int internId)
        {
            List<FileDetailsModel> ObjFiles = GetFileList(internId);

            var FileById = (from FC in ObjFiles
                            where FC.Id.Equals(id)
                            select new { FC.FileName, FC.FileContent }).ToList().FirstOrDefault();

            return File(FileById.FileContent, "application/pdf", FileById.FileName);

        }
        [HttpGet]
        public PartialViewResult FileDetails(int id)
        {
            List<FileDetailsModel> DetList = GetFileList(id);

            return PartialView("FileDetails", DetList);


        }

        private List<FileDetailsModel> GetFileList(int id)
        {
            List<FileDetailsModel> DetList = new List<FileDetailsModel>();
            SqlConnection conn = DAO.getConnection();

            using (conn)
            {
                DAO.getConnection();
                DataSet ds = new DataSet();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@InternId", id);
                ds = DAO.gettable("GetFileDetails", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FileDetailsModel obj = new FileDetailsModel();
                    obj.Id = int.Parse(dr["Id"].ToString());
                    obj.FileName = dr["FileName"].ToString();
                    obj.FileContent = (byte[])dr["FileContent"];
                    obj.InternId = int.Parse(dr["InternId"].ToString());
                    DetList.Add(obj);
                }





            }
            return DetList;
        }

        #endregion

        [Rule]
        public ActionResult EditDelete(InternModel model, FormCollection form)
        {
            string action = form["btn"].ToString();
            ViewBag.Message = objDAL.EditDelete(model, action);
            ModelState.Clear();
            return ViewInternList(ViewBag.Message);
        }
        public ActionResult EditProfile(InternModel model, FormCollection form)
        {
            string action = form["btn"].ToString();
            ViewBag.Message = objDAL.EditDelete(model, action);
            ModelState.Clear();
            return RedirectToAction("Profile", "Home");
        }
        public JsonResult UpdatePreference(string prefId, string internId, string projRole)
        {

            JsonResult data = new JsonResult();

            //ModelState.Clear();
            return Json(objDAL.UpdatePreference(prefId, internId, projRole), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInternByMentorAndPrefAndProjId(string mentorId, string prefId, string projId)
        {
            InternDAL objDAL = new InternDAL();
            //  List<InternModel> lstIntern = objDAL.getAllInternByMentorIdAndProjId(mentorId);
            if (string.IsNullOrEmpty(prefId))
            {
                return Json(objDAL.getAllInternByMentorIdAndProjId(mentorId, projId), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(objDAL.getAllInternByMentorIdAndProjId(mentorId, projId).Where(x => x.PreId == int.Parse(prefId)).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        //Kishor
        public ActionResult Assign()
        {

            ViewBag.projList = objDAL.GetInternByProjId().AsEnumerable().Select(row =>
        new InternModel
        {
            Id = int.Parse(row["Id"].ToString()),
            FName = row["FName"].ToString(),
            ProjRole = row["ProjRole"].ToString()


        });
            IEnumerable<ProjectModel> List = objDAL1.GetAllProjects().AsEnumerable().Select(row =>
            new ProjectModel
            {
                ProjId = int.Parse(row["ProjId"].ToString()),
                ProjectName = row["ProjectName"].ToString(),


            });
            ViewBag.ProjIdList = new SelectList(List, "ProjId", "ProjectName");

            return View("assign_Intern");


        }

        public ActionResult Assign_Project(int InternId)
        {
            ViewBag.projList = objDAL.GetInternByProjId().AsEnumerable().Select(row =>
       new InternModel
       {
           Id = int.Parse(row["Id"].ToString()),
           FName = row["FName"].ToString(),
           ProjRole = row["ProjRole"].ToString()


       });

            DataTable dt = objDAL.GetInternByIDFromtblInternInfo(InternId);
            InternModel objModel = new InternModel();

            foreach (DataRow row in dt.Rows)
            {
                objModel.Id = int.Parse(row["Id"].ToString());
                objModel.FName = row["FName"].ToString();
                objModel.ProjRole = row["ProjRole"].ToString();
            }
            IEnumerable<ProjectModel> List = objDAL1.GetAllProjects().AsEnumerable().Select(row =>
         new ProjectModel
         {
             ProjId = int.Parse(row["ProjId"].ToString()),
             ProjectName = row["ProjectName"].ToString(),


         });
            ViewBag.ProjIdList = new SelectList(List, "ProjId", "ProjectName");


            return View("Assign_Intern", objModel);

        }

        public ActionResult AddProject_Intern(InternModel model)
        {

          
            ViewBag.projList = objDAL.GetInternByProjId().AsEnumerable().Select(row =>
      new InternModel
      {
          Id = int.Parse(row["Id"].ToString()),
          FName = row["FName"].ToString(),
          ProjRole = row["ProjRole"].ToString()


      });


            IEnumerable<ProjectModel> List = objDAL1.GetAllProjects().AsEnumerable().Select(row =>
         new ProjectModel
         {
             ProjId = int.Parse(row["ProjId"].ToString()),
             ProjectName = row["ProjectName"].ToString(),


         });
            ViewBag.ProjIdList = new SelectList(List, "ProjId", "ProjectName");


            ViewBag.Message = objDAL.InternAssign(model);
            ModelState.Clear();

            return View("AddProject_Intern");
        }


        public ActionResult viewAssignedIntern()
        {

            return View("viewAssignedIntern");


        }
        public JsonResult InternList()
        {
            List<InternModel> InternList = new List<InternModel>();
            InternList = objDAL.getAllAssignedInternInfo();
            return Json(InternList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisplayInternList()
        {
            List<InternModel> InternList = new List<InternModel>();
            InternList = objDAL.getAllInternInfo();
            return Json(InternList, JsonRequestBehavior.AllowGet);
        }
    }
}