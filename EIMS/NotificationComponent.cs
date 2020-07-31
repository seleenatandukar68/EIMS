using EIMS.DataModel;
using EIMS.Models;
using EIMS.Models.DAL;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EIMS
{
    public class NotificationComponent
    {
        //Here we will add a function for register notification (will add sql dependency)
        //public void RegisterNotification(DateTime currentTime)
        //{
        //    string conStr = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        //    string sqlCommand = @"SELECT [Comment],[InternId] from [dbo].[tblComment] where [AddedOn] > @AddedOn";
        //    you can notice here I have added table name like this[dbo].[Contacts] with[dbo], its mendatory when you use Sql Dependency
        //    using (SqlConnection con = new SqlConnection(conStr))
        //    {
        //        SqlCommand cmd = new SqlCommand(sqlCommand, con);
        //        cmd.Parameters.AddWithValue("@AddedOn", currentTime);
        //        if (con.State != System.Data.ConnectionState.Open)
        //        {
        //            con.Open();
        //        }
        //        cmd.Notification = null;
        //        SqlDependency sqlDep = new SqlDependency(cmd);
        //        sqlDep.OnChange += new OnChangeEventHandler(sqlDep_OnChange);
        //        we must have to execute the command here
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            nothing need to add here now
        //        }
        //    }
        //}

        //void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        //{

        //    or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
        //    if (e.Info == SqlNotificationInfo.Insert)
        //        {
        //            SqlDependency sqlDep = sender as SqlDependency;
        //            sqlDep.OnChange -= sqlDep_OnChange;

        //            from here we will send notification message to client
        //            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<CommentHub>();
        //            notificationHub.Clients.All.notify("added");
        //            re - register notification
        //        RegisterNotification(DateTime.Now);
        //        }
        //}

        public List<CommentModel> GetComments(string username)
        {
            List<CommentModel> lst = new List<CommentModel>();
            CommentDAL objDAL = new CommentDAL();
            DataTable data = objDAL.GetComment(username);
            foreach (DataRow dr in data.Rows)
            {
                CommentModel obj = new CommentModel();
                obj.internId = int.Parse(dr["InternId"].ToString());
                obj.Comment = dr["Comment"].ToString();
                obj.weekId = int.Parse(dr["WeekId"].ToString());
                obj.cmtId = int.Parse(dr["Id"].ToString());
                obj.projId = int.Parse(dr["ProjectId"].ToString());
                lst.Add(obj);
            }
           return lst;
        }
        public List<CommentModel> GetUnreadComments(string mentorId)
        {
            List<CommentModel> lst = new List<CommentModel>();
            CommentDAL objDAL = new CommentDAL();
            DataTable data = objDAL.GetUnreadComment(mentorId);
            foreach (DataRow dr in data.Rows)
            {
                CommentModel obj = new CommentModel();
                obj.internId = int.Parse(dr["InternId"].ToString());
                obj.Comment = dr["Comment"].ToString();
                obj.weekId = int.Parse(dr["WeekId"].ToString());
                obj.projId = int.Parse(dr["ProjectId"].ToString());
                lst.Add(obj);
            }
            return lst;
        }
    }
}