using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EIMS.Models.DAL
{
    public class MentorDAL
    {



        public string AddMentor(MentorModel objModel)
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
                cmdd.CommandText = "GetMaxMentorId";
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
                string spName = "InsertMentor";
                try
                {
                    msg = "Mentor ID : MNT" + Id;
                    SqlParameter[] param = new SqlParameter[8];

                    param[0] = new SqlParameter("@MentorId", "MNT" + Id);
                    param[1] = new SqlParameter("@FName", objModel.FName);
                    param[2] = new SqlParameter("@MName", objModel.MName);
                    param[3] = new SqlParameter("@LName", objModel.LName);
                    param[4] = new SqlParameter("@Address", objModel.Address);
                    param[5] = new SqlParameter("@Email", objModel.Email);
                    param[6] = new SqlParameter("@Contact", objModel.Contact);
                    param[7] = new SqlParameter("@Id", Id);
                    DAO.executeTranProcedure(spName, param, transaction, conn);
                    LoginDAL objLogin = new LoginDAL();
                    objLogin.CreateUser("MNT" + Id, "MNT" + Id, 3);
                    objLogin.SendEmail("MNT" + Id, objModel.Email);
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

        public string EditDelete(InternModel objModel, string action)
        {
            string msg = "No Record Inserted";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            int Id = 0;
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
        public DataTable GetAllMentor()
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("getMentor", null);

            return ds.Tables[0];
        }
        public DataTable GetMentorByID(int id)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", id);
            ds = DAO.gettable("getMentorById", param);

            return ds.Tables[0];
        }

        public string EditDelete(MentorModel objModel, string action)
        {
            string msg = "No Record Inserted";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {
                    if (action == "Save")
                    {
                        string spName = "EditMentor";

                        msg = "One Record Updated Successfully";
                        SqlParameter[] param = new SqlParameter[7];

                        param[0] = new SqlParameter("@Id", objModel.Id);
                        param[1] = new SqlParameter("@FName", objModel.FName);
                        param[2] = new SqlParameter("@MName", objModel.MName);
                        param[3] = new SqlParameter("@LName", objModel.LName);
                        param[4] = new SqlParameter("@Address", objModel.Address);
                        param[5] = new SqlParameter("@Email", objModel.Email);
                        param[6] = new SqlParameter("@Contact", objModel.Contact);


                        DAO.executeTranProcedure(spName, param, transaction, conn);
                        transaction.Commit();

                    }
                    else if (action == "Delete")
                    {
                        string spName = "DeleteMentor";
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

        public DataTable GetMentorByProjId()
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("getMentorByProjId", null);

            return ds.Tables[0];
        }
        public DataTable GetMentorByIDFromMentorModel(int id)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", id);
            ds = DAO.gettable("getMentorById", param);

            return ds.Tables[0];
        }

        public string MentorAssign(MentorModel model)
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
                        string spName = "EditMentortbl";

                        msg = "One Record Updated Successfully";
                        SqlParameter[] param = new SqlParameter[5];


                        param[0] = new SqlParameter("@Id", model.Id);

                        param[1] = new SqlParameter("@FName", model.FName);
                        param[2] = new SqlParameter("@MName", model.MName);
                        param[3] = new SqlParameter("@LName", model.LName);
                        param[4] = new SqlParameter("@ProjId", model.ProjId);



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

        public List<MentorModel> getAllAssignedMentorInfo()
        {
            List<MentorModel> lstAssignedMentor = new List<MentorModel>();

            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("getAllAssignedMentor", null);
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                MentorModel mm = new MentorModel();
                mm.Id = int.Parse(row["Id"].ToString());
                mm.ProjId = int.Parse(row["ProjId"].ToString());
                mm.FName = row["FName"].ToString();
                mm.MName = row["MName"].ToString();
                mm.LName = row["LName"].ToString();
                lstAssignedMentor.Add(mm);
                //    ip.pa= int.Parse(row["ProjId"].ToString());
                //    pm.ProjectName = row["ProjectName"].ToString();
                //    pa.InternId = int.Parse(row["InternId"].ToString());
                //    pa.InternFName = row["InternFName"].ToString();
                //    pa.ProjRole = row["ProjRole"].ToString();




            }
            return lstAssignedMentor;
        }
        public List<MentorModel> getAllMentorInfo()
        {
            List<MentorModel> lstProject = new List<MentorModel>();

            DAO.getConnection();
            DataSet ds = new DataSet();

            ds = DAO.gettable("getMentor", null);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                MentorModel objIP = new MentorModel();

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