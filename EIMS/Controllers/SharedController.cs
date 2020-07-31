using EIMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EIMS.Controllers
{
   
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult Index()
        {
            return View();
        }

        public bool ValidateSession()
        {
            string x = Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]);
            if (x == "")
            {
                return false;
            }
            return true;
        }
        public int GetMentortoNotify(int InterId)
        {
            int mentorId=0;
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@InternId", InterId);
            ds = DAO.gettable("GetMentortoNotify", param);
            if(ds.Tables[0].Rows.Count > 0)
            {
                mentorId= int.Parse(ds.Tables[0].Rows[0]["MentorId"].ToString());
            }

            return mentorId;

        }
    }
}