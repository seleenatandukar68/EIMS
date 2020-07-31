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
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult SaveComment(CommentModel objModel)
        {
            CommentDAL objDAL = new CommentDAL();
            
            return Json(objDAL.AddComment(objModel), JsonRequestBehavior.AllowGet);


        }

        public void getComNotDetails (int id)
        {
            CommentDAL objDAL = new CommentDAL();
            objDAL.getComNotDetails(id);

        }
        public ActionResult getAllComNot(string mentorId)
        {
    
            
            return View("AllNotification");
        }

        public JsonResult GetCommentById(CommentModel obj)
        {
            CommentDAL objDAL = new CommentDAL();
            return Json(objDAL.getCommentByID(obj), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateIsRead(string cmtId)
        {
            CommentDAL objDAL = new CommentDAL();
            return Json(objDAL.UpdateIsRead(cmtId), JsonRequestBehavior.AllowGet);          

        }

        public JsonResult SaveFeedback(CommentModel obj)
        {
            CommentDAL objDAL = new CommentDAL();
            return Json(objDAL.SaveFeedback(obj), JsonRequestBehavior.AllowGet);
        }
    }
}