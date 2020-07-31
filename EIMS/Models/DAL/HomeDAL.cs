using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EIMS.Models.DAL
{
    public class HomeDAL
    {
        public List<ProjectModel> getAllProject()
        {
            List<ProjectModel> lstProject = new List<ProjectModel>();

            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("getAllProject", null);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ProjectModel objATT = new ProjectModel();
                objATT.ProjId = int.Parse(row["ProjId"].ToString());
                objATT.ProjectName = row["ProjectName"].ToString();
                objATT.StartDate = DateTime.Parse( row["StartDate"].ToString()).Date;
                objATT.EndDate = DateTime.Parse(row["EndDate"].ToString()).Date;
                objATT.Status= row["Status"].ToString();
                lstProject.Add(objATT);


            }
            return lstProject;

            
        }

        public string AddProject(ProjectModel objModel)
        {
            string msg = "No Record Inserted";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();
                #region To Get MAX ID

                SqlCommand cmdd = new SqlCommand();
                cmdd.Connection = conn;
                cmdd.Transaction = transaction;
                cmdd.CommandText = "GetMaxProjectId";
                cmdd.CommandType = CommandType.StoredProcedure;

                SqlParameter para1 = new SqlParameter();
                para1.ParameterName = "@Id";
                para1.Value = Id;
                para1.SqlDbType = SqlDbType.Int;
                para1.Size = 100;
                para1.Direction = ParameterDirection.InputOutput;
                cmdd.Parameters.Add(para1);


                cmdd.ExecuteNonQuery();

                if (cmdd.Parameters["@Id"].Value.ToString() == "") { cmdd.Parameters["@Id"].Value = 2000; }
                Id = int.Parse(cmdd.Parameters["@Id"].Value.ToString()) + 1;

                #endregion
                string spName = "InsertProject";
                try
                {
                    msg = "Project Id : Project" + Id;
                    SqlParameter[] param = new SqlParameter[5];


                    param[0] = new SqlParameter("@ProjectName", objModel.ProjectName);
                    param[1] = new SqlParameter("@StartDate", objModel.StartDate);
                    param[2] = new SqlParameter("@EndDate", objModel.EndDate);
                    param[3] = new SqlParameter("@Status", objModel.Status);
                    param[4] = new SqlParameter("@ProjId", Id);


                    DAO.executeTranProcedure(spName, param, transaction, conn);
                    transaction.Commit();
                }
                catch (Exception ex)

                {

                    transaction.Rollback();
                    throw new Exception("Error" + ex.Message);
                }
            }

            return msg;
        }
        public DataTable GetAllProjects()
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("getProjects", null);

            return ds.Tables[0];
        }
        public DataTable GetProjectByID(int id)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", id);
            ds = DAO.gettable("getProjectById", param);

            return ds.Tables[0];
        }
        public string Update(ProjectModel model)
        {

            string msg = "No Record Inserted";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            //int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {

                    {
                        string spName = "EditProject";

                        msg = "One Record Updated Successfully";
                        SqlParameter[] param = new SqlParameter[5];


                        param[0] = new SqlParameter("@ProjId", model.ProjId);
                        param[1] = new SqlParameter("@ProjectName", model.ProjectName);
                        param[2] = new SqlParameter("@StartDate", model.StartDate);
                        param[3] = new SqlParameter("@EndDate", model.EndDate);
                        param[4] = new SqlParameter("@Status", model.Status);

                        DAO.executeTranProcedure(spName, param, transaction, conn);
                        transaction.Commit();

                    }

                }

                catch (Exception ex)

                {

                    transaction.Rollback();
                    throw new Exception("Error" + ex.Message);
                }

            }

            return msg;
        }
        public String Delete(ProjectModel model)
        {

            string msg = "No Record Inserted";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            //int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {

                    {
                        string spName = "DeleteProject";

                        msg = "One Record Deleted Successfully";
                        SqlParameter[] param = new SqlParameter[1];

                        param[0] = new SqlParameter("@ProjId", model.ProjId);
                        DAO.executeTranProcedure(spName, param, transaction, conn);
                        transaction.Commit();

                    }

                }

                catch (Exception ex)

                {

                    transaction.Rollback();
                    throw new Exception("Error" + ex.Message);
                }

            }

            return msg;
        }
        public DataTable GetAssignedInternByProjId(int id)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", id);
            ds = DAO.gettable("GetAssignedInternByProjId", param);

            return ds.Tables[0];
        }
        public DataTable GetAssignedMentorByProjId(int id)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", id);
            ds = DAO.gettable("GetAssignedMentorByProjId", param);

            return ds.Tables[0];
        }
        public DataTable GetWeekNumber(int id, int ProjId)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@id", id);
            param[1] = new SqlParameter("@ProjId", ProjId);
            ds = DAO.gettable("getWeekNumber", param);

            return ds.Tables[0];
        }
    }
}