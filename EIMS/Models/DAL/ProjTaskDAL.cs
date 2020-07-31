using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EIMS.Models.DAL
{
    public class ProjTaskDAL
    {
        
        public DataTable GetProjTaskDetById(string id)
        {
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", int.Parse(id));
            ds = DAO.gettable("getProjecTaskDetById", param);

            return ds.Tables[0];
        }

        public string UpdateTaskStatus(List<ProjTaskModel> objModelList)
        {
            string msg = "Task Status Updated";
            string spName = "updateTaskStatus";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {
                    foreach (ProjTaskModel objModel in objModelList) { 
                    SqlParameter[] param = new SqlParameter[4];

                    param[0] = new SqlParameter("@internId", objModel.InternId);
                    param[1] = new SqlParameter("@taskId", objModel.TaskId);
                    param[2] = new SqlParameter("@weekId", objModel.WeekId);
                    param[3] = new SqlParameter("@Status", objModel.Status);


                    DAO.executeTranProcedure(spName, param, transaction, conn);
                    
                }
                    transaction.Commit();
                }

                catch (Exception ex) { 

                    transaction.Rollback();
                    throw new Exception("Error" + ex.Message);
                }

            }
            return msg;
        }

        public List<ProjectModel> GetAllProject()
        {
            List<ProjectModel> lstProjModel = new List<ProjectModel>();
            DAO.getConnection();
            DataSet ds = new DataSet();
            ds = DAO.gettable("GetAllProject", null);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProjectModel obj = new ProjectModel();
                obj.ProjId = int.Parse(dr["ProjId"].ToString());
                obj.ProjectName = dr["ProjectName"].ToString();
                lstProjModel.Add(obj);
            }
            return lstProjModel;
        }
        public List<ProjectModel> GetProjectByMentorId(string MentorId)
        {
            List<ProjectModel> lstProjModel = new List<ProjectModel>();
            DAO.getConnection();
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@MentorId", int.Parse(MentorId.Trim()));
            ds = DAO.gettable("GetProjectByMentorId", param);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProjectModel obj = new ProjectModel();
                obj.ProjId = int.Parse(dr["ProjId"].ToString());
                obj.ProjectName = dr["ProjectName"].ToString();
                lstProjModel.Add(obj);
            }
            return lstProjModel;
        }

        public string SaveAssignedTask(ProjectTaskModel objModelList)
        {
            string msg = "Task Added ";
           // string spName = "SaveAssignedTask";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {
                    foreach (TaskModel objModel in objModelList.tasks)
                    {
                        if (objModel.TaskId == 0)
                        {
                            SqlParameter[] param = new SqlParameter[5];

                            param[0] = new SqlParameter("@ProjId", objModelList.project.ProjId);
                            param[1] = new SqlParameter("@prefId", objModelList.prefId);
                            param[2] = new SqlParameter("@weekId", objModelList.weekId);
                            param[3] = new SqlParameter("@TaskDesc", objModel.TaskDes);
                            param[4] = new SqlParameter("@AssignedBy", objModelList.AssignedBy);



                            DAO.executeTranProcedure("SaveAssignedTask", param, transaction, conn);
                        }
                        else
                        {
                            SqlParameter[] param = new SqlParameter[6];

                            param[0] = new SqlParameter("@ProjId", objModelList.project.ProjId);
                            param[1] = new SqlParameter("@prefId", objModelList.prefId);
                            param[2] = new SqlParameter("@weekId", objModelList.weekId);
                            param[3] = new SqlParameter("@TaskDesc", objModel.TaskDes);
                            param[4] = new SqlParameter("@AssignedBy", objModelList.AssignedBy);
                            param[5] = new SqlParameter("@taskId", objModel.TaskId);



                            DAO.executeTranProcedure("UpdateAssignedTask", param, transaction, conn);

                        }

                    }
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

        public string PublishTask(ProjectTaskModel objModelList)
        {
            string msg = "Task Published ";
            // string spName = "SaveAssignedTask";
            List<InternModel> lstIntern = new List<InternModel>();
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {


                    SqlParameter[] param1 = new SqlParameter[3];
                    param1[0] = new SqlParameter("@ProjId", objModelList.project.ProjId);
                    param1[1] = new SqlParameter("@PrefId", objModelList.prefId);
                    param1[2] = new SqlParameter("@mentorId", int.Parse(objModelList.AssignedBy.Trim()));
                    DataSet ds = new DataSet();
                    ds = DAO.gettable("GetInternstoPublish", param1);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            // dr["Id"].ToString();

                            foreach (TaskModel objModel in objModelList.tasks )
                            {
                                if (objModel.Status == false)
                                {
                                    SqlParameter[] param = new SqlParameter[1];
                                    param[0] = new SqlParameter("@taskId", objModel.TaskId);
                                    DAO.executeTranProcedure("UpdateIsPublished", param, transaction, conn);

                                    SqlParameter[] param2 = new SqlParameter[6];
                                    param2[0] = new SqlParameter("@ProjId", objModelList.project.ProjId);
                                    param2[1] = new SqlParameter("@WeekId", objModelList.weekId);
                                    param2[2] = new SqlParameter("@TaskId", objModel.TaskId);
                                    param2[3] = new SqlParameter("@TaskDes", objModel.TaskDes);
                                    param2[4] = new SqlParameter("@Status", 1);
                                    param2[5] = new SqlParameter("@InternId", int.Parse(dr["Id"].ToString().Trim()));

                                    DAO.executeTranProcedure("InsertPublishedTask", param2, transaction, conn);
                                }

                            }
                        }


                        transaction.Commit();
                    }
                    else
                    {
                        msg = "No Interns Assigned to this project";
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

        public ProjectTaskModel GetAssignedTask(ProjectTaskModel objModelList)
        {
           // ProjectTaskModel objAtt = new ProjectTaskModel();
            List<TaskModel> taskList = new List<TaskModel>();
            DAO.getConnection();
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@ProjId", objModelList.project.ProjId);
            param[1] = new SqlParameter("@prefId", objModelList.prefId);
            param[2] = new SqlParameter("@weekId", objModelList.weekId);
           
            param[3] = new SqlParameter("@AssignedBy", objModelList.AssignedBy);
            DataSet ds = new DataSet();
            ds = DAO.gettable("GetAssignedTaskById", param);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TaskModel obj = new TaskModel();
                obj.TaskId = int.Parse(dr["TaskId"].ToString());
                obj.Status = dr["IsPublished"].ToString()=="False"?false:true;
                obj.TaskDes = dr["TaskDescription"].ToString();
                taskList.Add(obj);
               
            }
            objModelList.tasks = taskList;
                return objModelList;

        }

        public string DeleteTask(string taskid)
        {
            string msg = "Task Removed ";
            // string spName = "SaveAssignedTask";
            SqlTransaction transaction;
            SqlConnection conn = DAO.getConnection();
            using (conn)
            {
                transaction = conn.BeginTransaction();
                try
                {
                   
                            SqlParameter[] param = new SqlParameter[1];

                           
                            param[0] = new SqlParameter("@taskId", int.Parse(taskid.Trim()));



                            DAO.executeTranProcedure("DeleteTask", param, transaction, conn);
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
    }
}