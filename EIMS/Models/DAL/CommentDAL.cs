using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace EIMS.Models.DAL
{
    public class CommentDAL
    {
        public DataTable GetComment(string mentorid)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@mentorid", mentorid);
            ds = DAO.gettable("getComment", param);

            return ds.Tables[0];
        }

        public DataTable GetUnreadComment(string mentorId)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@MentorId", int.Parse(mentorId.Trim()) );
            ds = DAO.gettable("getUnreadComment", param);

            return ds.Tables[0];
        }
        public bool AddComment(CommentModel obj)
        {
            string spName = "AddComment";
            if (obj.status == "E")
            {
                spName= "UpdateComment";

            }
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[5];

            param[0] = new SqlParameter("@Comment", obj.Comment);
            param[1] = new SqlParameter("@InternId", obj.internId);
            param[2] = new SqlParameter("@DateTime", DateTime.Now);
            param[3] = new SqlParameter("@ProjectId", obj.projId);
            param[4] = new SqlParameter("@WeekId", obj.weekId);
            DAO.executeProcedure(spName, param);

            return true;
        }

        public CommentModel getCommentByID(CommentModel obj)
        {
            string comment = "";
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@InternId", obj.internId);
            param[1] = new SqlParameter("@ProjectId", obj.projId);
            param[2] = new SqlParameter("@WeekId", obj.weekId);
            ds = DAO.gettable("getCommentByID", param);
            if (ds.Tables[0].Rows.Count > 0) { 
            foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    obj.Comment = dr["Comment"].ToString();
                    obj.addedOn = Convert.ToDateTime(dr["AddedOn"].ToString()).ToString("MM / dd / yyyy HH: mm:ss") ;
                    obj.cmtId = int.Parse(dr["Id"].ToString());
                    obj.projId = int.Parse(dr["ProjectId"].ToString());
                    obj.feedback = dr["Feedback"].ToString();
                }
                obj.status = "E";
                }
            return obj;
        }
        public void getComNotDetails(int cmtId) {
            string comment = "";
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            
            param[0] = new SqlParameter("@CmtId", cmtId);
            ds = DAO.gettable("getComNotDetails", param);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //obj.Comment = dr["Comment"].ToString();

                }
               
            }
        }

        public bool UpdateIsRead(string cmtId)
        {
            string spName = "UpdateIsRead";
         
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@cmtId", int.Parse(cmtId));
            
            DAO.executeProcedure(spName, param);
            return true;

            
        }

        public bool SaveFeedback(CommentModel obj)
        {
            string spName = "SaveFeedback";

            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@InternId", obj.internId);
            param[1] = new SqlParameter("@ProjectId", obj.projId);
            param[2] = new SqlParameter("@WeekId", obj.weekId);
            param[3] = new SqlParameter("@feedback", obj.feedback);

            DAO.executeProcedure(spName, param);
            return true;


        }
    }
}