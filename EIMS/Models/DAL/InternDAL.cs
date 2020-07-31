using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EIMS.Models.DAL
{
    public class InternDAL
    {
        public InternModel AddIntern(InternModel objModel)
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
                cmdd.CommandText = "GetMaxInternId";
                cmdd.CommandType = CommandType.StoredProcedure;

                SqlParameter para1 = new SqlParameter();
                para1.ParameterName = "@Id";
                para1.Value = Id;
                para1.SqlDbType = SqlDbType.Int;
                para1.Size = 100;
                para1.Direction = ParameterDirection.InputOutput;
                cmdd.Parameters.Add(para1);


                cmdd.ExecuteNonQuery();

                if (cmdd.Parameters["@Id"].Value.ToString() == "") { cmdd.Parameters["@Id"].Value = 1000; }
                Id = int.Parse(cmdd.Parameters["@Id"].Value.ToString()) + 1;

                #endregion
                string spName = "InsertIntern";
                try
                {
                    msg = "Intern ID : INT" + Id;
                    objModel.Id = Id;
                    SqlParameter[] param = new SqlParameter[9];

                    param[0] = new SqlParameter("@InternId", "INT" + Id);
                    param[1] = new SqlParameter("@FName", objModel.FName);
                    param[2] = new SqlParameter("@MName", objModel.MName);
                    param[3] = new SqlParameter("@LName", objModel.LName);
                    param[4] = new SqlParameter("@Address", objModel.Address);
                    param[5] = new SqlParameter("@Email", objModel.Email);
                    param[6] = new SqlParameter("@Contact", objModel.Contact);
                    param[7] = new SqlParameter("@Qualification", objModel.Qualification);
                    param[8] = new SqlParameter("@Id", Id);
                    DAO.executeTranProcedure(spName, param, transaction, conn);
                    LoginDAL objLogin = new LoginDAL();
                    objLogin.CreateUser("INT" + Id, "INT" + Id, 2);
                    objLogin.SendEmail("INT" + Id, objModel.Email);

                    

                    transaction.Commit();
                }
                catch (Exception ex)

                {

                    transaction.Rollback();
                    throw new Exception("Error" + ex.Message);
                }
            }

            return objModel;
        }

        public string EditDelete(InternModel objModel, string action)
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
                    if (action == "Save")
                    {
                        string spName = "EditIntern";

                        msg = "One Record Updated Successfully";
                        SqlParameter[] param = new SqlParameter[8];

                        param[0] = new SqlParameter("@Id", objModel.Id);
                        param[1] = new SqlParameter("@FName", objModel.FName);
                        param[2] = new SqlParameter("@MName", objModel.MName);
                        param[3] = new SqlParameter("@LName", objModel.LName);
                        param[4] = new SqlParameter("@Address", objModel.Address);
                        param[5] = new SqlParameter("@Email", objModel.Email);
                        param[6] = new SqlParameter("@Contact", objModel.Contact);
                        param[7] = new SqlParameter("@Qualification", objModel.Qualification);

                        DAO.executeTranProcedure(spName, param, transaction, conn);
                        transaction.Commit();

                    }
                    else if (action == "Delete")
                    {
                        string spName = "DeleteIntern";
                        msg = "One Record Deleted Successfully";

                        SqlParameter[] param = new SqlParameter[1];

                        param[0] = new SqlParameter("@Id", objModel.Id);
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
        public DataTable GetAllIntern()
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("getIntern", null);

            return ds.Tables[0];
        }
        public DataTable GetInternByID(int id)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", id);
            ds = DAO.gettable("getInternById", param);

            return ds.Tables[0];
        }

        public bool CheckPreferenceSetting(string internId)
        {
            bool flag = false;
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@internId", int.Parse(internId.Trim()));
            ds = DAO.gettable("checkPreferenceSettingsById", param);
            int prefId = 0;
            if (ds.Tables[0].Rows.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    prefId = String.IsNullOrEmpty(dr["PreId"].ToString()) ? 0 : int.Parse(dr["PreId"].ToString());
                }
            }
            if (prefId == 0) { flag = false; }
            else { flag = true; }

            return flag;
        }

        public DataTable GetAllPreferences()
        {
            DAO.getConnection();
            DataSet ds = new DataSet();

            ds = DAO.gettable("getAllPreferences", null);

            return ds.Tables[0];
        }

        public string UpdatePreference(string prefId, string internId, string projRole)
        {
            string spName = "UpdatePreference";

            string msg = "";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            //int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {
                    msg = "Your internship preference has been updated";
                    SqlParameter[] param = new SqlParameter[3];

                    param[0] = new SqlParameter("@internId", int.Parse(internId.Trim()));
                    param[1] = new SqlParameter("@prefId", int.Parse(prefId));
                    param[2] = new SqlParameter("@projrole", projRole);


                    DAO.executeTranProcedure(spName, param, transaction, conn);
                    transaction.Commit();
                }


                catch (Exception ex)
                {

                    transaction.Rollback();
                    throw new Exception("Error" + ex.Message);
                }

                return msg;

            }

        }

        public List<InternModel> getAllInternByMentorIdAndProjId(string mentorId, string projId)
        {
            List<InternModel> lstModel = new List<InternModel>();
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@mentorId", int.Parse(mentorId));
            param[1] = new SqlParameter("@projId", int.Parse(projId));
            ds = DAO.gettable("getAllInternByMentorId", param);
            if (ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    InternModel obj = new InternModel();
                    obj.Id = int.Parse(dr["Id"].ToString());
                    obj.InternId = dr["InternId"].ToString();
                    obj.FName = dr["FName"].ToString();
                    obj.LName = dr["LName"].ToString();
                    obj.PreId = int.Parse(dr["PreId"].ToString());
                    obj.prefCategory = dr["PreferenceCategory"].ToString();
                    lstModel.Add(obj);
                }
                    }
            return lstModel;
        }

        public DataTable GetInternByProjId()
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("getInternByProjId", null);

            return ds.Tables[0];
        }
        public DataTable GetInternByIDFromtblInternInfo(int InternId)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@InternId", InternId);
            ds = DAO.gettable("getInternByIdFromtblInternInfo", param);

            return ds.Tables[0];
        }

        public string InternAssign(InternModel model)
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
                        string spName = "EdittblInternInfo";

                        msg = "One Record Updated Successfully";
                        SqlParameter[] param = new SqlParameter[3];


                        param[0] = new SqlParameter("@ProjId", model.ProjId);

                        param[1] = new SqlParameter("@InternId", model.Id);
                       
                        param[2] = new SqlParameter("@ProjRole", model.ProjRole);



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

        public List<InternModel> getAllAssignedInternInfo()
        {
            List<InternModel> lstProject = new List<InternModel>();

            DAO.getConnection();
            DataSet ds = new DataSet();

            ds = DAO.gettable("getAllAssignedInternInfo", null);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //Intern_Project objIP = new Intern_Project();
                //Project_Assign objPA = new Project_Assign();
                //ProjectModel objPM = new ProjectModel();
                InternModel objIP = new InternModel();

                objIP.Id = int.Parse(dr["Id"].ToString());
                objIP.FName = dr["FName"].ToString();
                objIP.ProjId = int.Parse(dr["ProjId"].ToString());
                //objPM.ProjectName = dr["ProjectName"].ToString();
                objIP.ProjRole = dr["ProjRole"].ToString();
                //objIP.project_assign_model = objPA;
                //objIP.project_model = objPM;
                // Project_Assign pa = new Project_Assign();
                //ProjectModel pm = new ProjectModel();
                //Intern_Project ip = new Intern_Project();
                //= int.Parse(row["ProjId"].ToString());
                //.ProjectName = row["ProjectName"].ToString();
                // pa.InternId = int.Parse(row["InternId"].ToString());
                //pa.InternFName = row["InternFName"].ToString();
                //pa.ProjRole = row["ProjRole"].ToString();

                lstProject.Add(objIP);



            }
            return lstProject;
        }
        public List<InternModel> getAllInternInfo()
        {
            List<InternModel> lstProject = new List<InternModel>();

            DAO.getConnection();
            DataSet ds = new DataSet();

            ds = DAO.gettable("getIntern", null);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                
                InternModel objIP = new InternModel();

                objIP.Id = int.Parse(dr["Id"].ToString());
                objIP.FName = dr["FName"].ToString();
                objIP.MName = dr["MName"].ToString();
               
                objIP.LName = dr["LName"].ToString();
             //   objIP.prefCategory = dr["prefCategory"].ToString();





                    

                lstProject.Add(objIP);



            }
            return lstProject;
        }
    }
}