using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EIMS.Models.DAL
{
    public class LoginDAL
    {
        public bool verifyLogin(LoginModel objLogin)
        {
            bool isExist = false;
            String spName = "verifyLogin";
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@username", objLogin.UserName);
            param[1] = new SqlParameter("@password", objLogin.Password);


            ds = DAO.gettable(spName, param);
            if(ds.Tables[0].Rows.Count == 1)
             {
                isExist = true;
             }
            return isExist;
        }
        public void CreateUser(string username, string password, int roleId)
        {
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {

                    SqlParameter[] param2 = new SqlParameter[3];
                    param2[0] = new SqlParameter("@username", username);
                    param2[1] = new SqlParameter("@password", password);
                    param2[2] = new SqlParameter("@roleId", roleId);
                    DAO.executeTranProcedure("CreateUser", param2, transaction, conn);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            
        }
        public string UpdatePassword(string Username, int role, string email)
        {
            string msg = "Invalid Username or User type";
            try
            {
                DAO.getConnection();
                DataSet ds = new DataSet();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@username", Username);
                param[1] = new SqlParameter("@role", role);



                ds = DAO.gettable("GetUserByUsername", param);
                if(ds.Tables[0].Rows.Count == 1)
                {
                    msg = "New password sent to " + email;
                    LoginModel obj = new LoginModel();
                    obj.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                    obj.Password = Username;
                    UpdateUser(obj);
                    SendEmail(Username, email);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return msg;
        }
        public bool UpdateUser(LoginModel objModel)
        {
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            int Id = 0;
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {

                    SqlParameter[] param2 = new SqlParameter[2];
                    param2[0] = new SqlParameter("@userId", objModel.UserId);
                    param2[1] = new SqlParameter("@password", objModel.Password);
                    
                    DAO.executeTranProcedure("UpdateUser", param2, transaction, conn);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                    return false;
                }
            }

        }

        public void SendEmail(string Id, string receiver)
        {
            var senderEmail = new MailAddress("seleena68@gmail.com", "seleena");
            var receiverEmail = new MailAddress(receiver, "Receiver");
            var password = "pthopjrveqbgdvmk";
            var sub = "Login Details for EIMT";
            var body = "Please find your username and password to login to EIMT \n username: " + Id + "\n password" +  Id;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
        }
    }
}