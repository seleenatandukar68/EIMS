using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EIMS.Models
{
    public class DAO
    {
        
            public static string connectionStr
            {
                get { return ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString; }
            }

            public static SqlConnection getConnection()
            {
                SqlConnection con = new SqlConnection(DAO.connectionStr);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                return con;
            }

            public static int executeProcedure(string StoreProcName, SqlParameter[] param)
            {
                using (SqlConnection con = DAO.getConnection())
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = StoreProcName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (param != null)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }

            public static int executeTranProcedure(string StoreProcName, SqlParameter[] param, SqlTransaction tran, SqlConnection con)
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = StoreProcName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (tran != null)
                    {
                        cmd.Transaction = tran;
                    }
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }

            public static DataSet gettable(string StoreProcName, SqlParameter[] param)
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = DAO.getConnection())
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = StoreProcName;
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (param != null)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {

                            da.Fill(ds);

                        }
                        return ds;
                    }
                }


            }
        }
    }

