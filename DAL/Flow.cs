using Certification.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Certification.DAL
{
    public class Flow
    {
        public int CreateFlow(Model.FlowInfo flow)
        {
            string cmdText = @"insert into flow (parentId,Status,ReqId) 
                            values (@parentId,@Status,@ReqId); select @@identity;";
            SqlParameter[] parameters = {
                new SqlParameter("@parentId",flow.ParentId),
                new SqlParameter("@Status",(int)flow.Status),
                new SqlParameter("@ReqId",flow.ReqId)
            };
            object obj = SqlHelper.ExecuteScalar(cmdText, parameters);
            return obj == null ? -1 : Convert.ToInt32(obj);
        }

        public Model.FlowInfo GetParentFlow(int reqId)
        {
            Model.FlowInfo flow = null;
            string cmdText = @"select flowId,parentId,Status,ReqId from flow where ReqId=@reqId and parentId=0";
            SqlParameter param = new SqlParameter("@reqId", reqId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText, param))
            {
                if (reader.Read())
                {
                    flow = new Model.FlowInfo
                    {
                        FlowId = Convert.ToInt32(reader["flowId"]),
                        ParentId = Convert.ToInt32(reader["parentId"]),
                        Status = (Model.FlowStatus)int.Parse(reader["Status"].ToString()),
                        ReqId = Convert.ToInt32(reader["ReqId"])
                    };
                }
            }
            return flow;
        }

        public int AddTask(Model.TaskInfo task)
        {
            string cmdText = @"insert into [Task] (flowId,uid,createTime,Role)
                                           values (@flowId,@uid,@createTime,@role); select @@identity";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@flowId",task.FlowId),
                new SqlParameter("@uid",task.UID),
                new SqlParameter("@createTime",task.CreateTime),
                new SqlParameter("@role",task.Role)
            };
            object obj = SqlHelper.ExecuteScalar(cmdText, parameters);
            return obj == null ? -1 : Convert.ToInt32(obj);
        }

        public void UpdateTask(Model.TaskInfo task)
        {
            string cmdText = @"update [Task] set [hostName]=@hostName,
                                [logTime]=@logTime,[ACT]=@act,[Comment]=@Comment 
                                where sno=@sno";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@sno",task.SNo),
                new SqlParameter("@hostName",task.HostName),
                new SqlParameter("@logTime",task.LogTime),
                new SqlParameter("@act",(int)task.Act),
                new SqlParameter("@comment",task.Comments)
            };
            SqlHelper.ExecuteNonQuery(cmdText, parameters);
        }

        public void CancelAllOtherTasks(UserInfo currentUser, Model.FlowInfo flow, Model.ActionType action)
        {
            string cmdText = @"update Task set [Act]=@act,Comment=@comment,logTime=getdate() 
                               where flowId=@flowId 
                               and uid!=@uid 
                               and logTime is null";
            SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@act",(int)action),
                    new SqlParameter("@comment",Enum.GetName(typeof(Model.ActionType),action)+" by "+currentUser.DisplayName),       //base on act for reject/approval/return,
                    new SqlParameter("@flowId",flow.FlowId),
                    new SqlParameter("@uid",currentUser.UID)
                };
            SqlHelper.ExecuteNonQuery(cmdText, parameters);
        }

        public bool IsLastApprover(string uid, Model.FlowInfo flow)
        {
            string cmdText = @"select 1 from Task 
                                where flowId=@flowId 
                                and uid!=@uid 
                                and logtime is null";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@uid",uid),
                new SqlParameter("@flowId",flow.FlowId)
            };
            return SqlHelper.ExecuteScalar(cmdText, parameters) == null;
        }

        public void UpdateStatus(int flowId, Model.FlowStatus status)
        {
            string cmdText = @"update flow Set [Status]=@status 
                               where flowId=@flowId";
            SqlParameter[] parameters = new SqlParameter[]{
                        new SqlParameter("@flowId",flowId),
                        new SqlParameter("@status",(int)status)
                    };
            SqlHelper.ExecuteNonQuery(cmdText, parameters);
        }

        public System.Data.DataTable GetTasks(int flowId)
        {
            string cmdText = @"select t.SNo,t.flowId,isnull(u.displayName,t.UID) as displayName,t.UID,t.CreateTime,t.LogTime,
                                      t.Act,t.Comment,t.Role,a.ActDesc,r.RoleName 
                               from Task t left join WinADUser u on u.userid=t.uid 
                                    left join TaskAction a on t.Act=a.Act 
                                    left join Role r on t.Role=r.RoleID 
                               where t.flowId=@flowId 
                               order by case when t.LogTime is null then getdate() else t.LogTime end";
            SqlParameter param = new SqlParameter("@flowId", flowId);
            return SqlHelper.GetDataTableOfRecord(cmdText, param);
        }
        /// <summary>
        /// 取消代理者或被代理者的task
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="flow"></param>
        /// <param name="action"></param>
        public void CancelDelegateTasks(string uid, Model.FlowInfo flow, Model.ActionType action)
        {
            string cmdText = @"update Task set Act=@act,comment=@comment,logTime=GETDATE()
                               where flowId=@flowId 
                               and uid!=@uid
                               and logTime is null 
                               and Role in (
		                            select Role from task 
		                            where flowId=@flowId and uid=@uid 
		                            and logTime is null)";
            SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@act",(int)action),
                    new SqlParameter("@comment",Enum.GetName(typeof(Model.ActionType),action)+" by "+uid),       //base on act for reject/approval/return,
                    new SqlParameter("@flowId",flow.FlowId),
                    new SqlParameter("@uid",uid)
                };
            SqlHelper.ExecuteNonQuery(cmdText, parameters);
        }

        public Model.FlowInfo GetFlow(int flowId)
        {
            Model.FlowInfo flow = null;
            string cmdText = "select * from flow where flowId=@flowId";
            SqlParameter parameter = new SqlParameter("@flowId", flowId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText, parameter))
            {
                if (reader.Read())
                {
                    flow = new Model.FlowInfo();
                    flow.FlowId = (int)reader["flowId"];
                    flow.ParentId = (int)reader["parentId"];
                    flow.Status = (Model.FlowStatus)reader["Status"];
                    flow.ReqId = (int)reader["ReqId"];
                }
            }
            return flow;
        }

        public bool ChildAllClosed(int flowId)
        {
            string cmdText = "select 1 from flow where parentId=@parentId and Status!=@status";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@parentId",flowId),
                new SqlParameter("@status",(int)Model.FlowStatus.Closed)
            };
            return SqlHelper.ExecuteScalar(cmdText, parameters) == null;
        }

        public Model.FlowInfo GetFlowByTaskId(int taskId)
        {
            Model.FlowInfo flow = null;
            string cmdText = @"select * from flow where flowId=(
                                    select flowId from Task where sno=@sno
                                    )";
            SqlParameter parameter = new SqlParameter("@sno", taskId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText, parameter))
            {
                if (reader.Read())
                {
                    flow = new Model.FlowInfo();
                    flow.FlowId = (int)reader["flowId"];
                    flow.ParentId = (int)reader["parentId"];
                    flow.Status = (Model.FlowStatus)reader["Status"];
                    flow.ReqId = (int)reader["ReqId"];
                }
            }
            return flow;
        }

        public Model.TaskInfo GetOpenTask(int taskId)
        {
            string cmdText = @"select * from task where sno=@sno and logTime is null";
            SqlParameter parameter = new SqlParameter("@sno", taskId);
            Model.TaskInfo ts = null;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText, parameter))
            {
                if (reader.Read())
                {
                    ts = new Model.TaskInfo();
                    ts.SNo = (int)reader["sno"];
                    if (reader["act"] != DBNull.Value && reader["act"] != null)
                    {
                        ts.Act = (Model.ActionType)reader["act"];
                    }
                    if (reader["uid"] != DBNull.Value && reader["uid"] != null)
                    {
                        ts.UID = reader["uid"].ToString();
                    }
                }
            }
            return ts;
        }

        public TaskInfo GetOpenTask(string uid, int flowId)
        {
            string cmdText = @"select * from task where (uid=@uid or uid in (select RoleName from RoleUser ru
                                    inner join Role r on ru.RoleID=r.RoleID 
                                    where ru.uid=@uid and ru.status=1)) and logTime is null and flowId=@flowId";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@uid", uid),
                new SqlParameter("@flowId",flowId)
            };
            TaskInfo ts = null;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText, parameters))
            {
                if (reader.Read())
                {
                    ts = new TaskInfo();
                    ts.SNo = (int)reader["sno"];
                    if (reader["act"] != DBNull.Value && reader["act"] != null)
                    {
                        ts.Act = (ActionType)reader["act"];
                    }
                    if (reader["uid"] != DBNull.Value && reader["uid"] != null)
                    {
                        ts.UID = reader["uid"].ToString();
                    }
                }
            }
            return ts;
        }

        public DataTable GetTasksByRequestId(int reqId)
        {
            string cmdText = @"select t.*,w.displayName,r.RoleName from task t
                            left join WinADUser w on t.uid=w.userid
                            left join Role r on t.Role =r.RoleID
                            where t.logTime is null
	                            and t.flowId in (select flowId from flow where ReqId=@reqId)";
            SqlParameter parameter = new SqlParameter("@reqId", reqId);
            return SqlHelper.GetDataTableOfRecord(cmdText, parameter);
        }

        public void AddTask(int sno, string uid)
        {
            string cmdText = @"insert into task 
                            select flowId,@uid,GETDATE(),Role,null,null,null,null from task
                            where sno=@sno";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@uid",uid),
                new SqlParameter("@sno",sno)
            };
            SqlHelper.ExecuteNonQuery(cmdText, paras);
        }

        public Dictionary<int, string> GetRequestStatusList()
        {
            string cmdText = "select * from RequestStatus where Status!=0";
            Dictionary<int, string> list = new Dictionary<int, string>();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText))
            {
                while (reader.Read())
                {
                    list.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                }
            }
            return list;
        }

        public TaskInfo GetTask(int taskId)
        {
            string cmdText = @"select * from task where sno=@sno";
            SqlParameter parameter = new SqlParameter("@sno", taskId);
            Model.TaskInfo ts = null;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText, parameter))
            {
                if (reader.Read())
                {
                    ts = new Model.TaskInfo();
                    ts.SNo = (int)reader["sno"];
                    if (reader["act"] != DBNull.Value && reader["act"] != null)
                    {
                        ts.Act = (Model.ActionType)reader["act"];
                    }
                    if (reader["uid"] != DBNull.Value && reader["uid"] != null)
                    {
                        ts.UID = reader["uid"].ToString();
                    }
                }
            }
            return ts;
        }
    }
}