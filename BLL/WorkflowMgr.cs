using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Data;
using Certification.Model;

namespace Certification.BLL
{
    public static class WorkflowMgr
    {
        private static readonly DAL.Flow _flow = new DAL.Flow();
        public static FlowInfo GetParentFlow(int reqId)
        {
            return _flow.GetParentFlow(reqId);
        }

        public static FlowInfo GetFlow(int flowId)
        {
            return _flow.GetFlow(flowId);
        }

        public static int CreateFlow(FlowInfo flow)
        {
            int FlowId = _flow.CreateFlow(flow);
            flow.FlowId = FlowId;
            return FlowId;
        }

        public static FlowInfo CreateChildFlow(FlowInfo parent, FlowStatus status)
        {
            FlowInfo childflow = new FlowInfo();
            childflow.ReqId = parent.ReqId;
            childflow.ParentId = parent.FlowId;
            childflow.Status = status;
            childflow.FlowId = CreateFlow(childflow);
            return childflow;
        }

        #region task operations

        public static TaskInfo AddTask(string uid, int flowId, RoleType roleType)
        {
            TaskInfo task = new TaskInfo();
            task.FlowId = flowId;
            task.UID = uid;
            task.CreateTime = DateTime.Now;
            task.Role = roleType;
            task.SNo = _flow.AddTask(task);
            return task;
        }

        public static void UpdateTask(int taskId, ActionType action, string comments)
        {
            TaskInfo task = new TaskInfo
            {
                SNo = taskId,
                Act = action,
                Comments = comments,
                LogTime = DateTime.Now,
                HostName = string.Empty
            };
            _flow.UpdateTask(task);
        }

        public static void CancelAllOtherTasks(UserInfo currentUser, FlowInfo flow, ActionType action)
        {
            _flow.CancelAllOtherTasks(currentUser, flow, action);
        }

        public static DataTable GetTasks(int flowId)
        {
            return _flow.GetTasks(flowId);
        }

        public static TaskInfo GetOpenTask(int taskId)
        {
            return _flow.GetOpenTask(taskId);
        }

        public static TaskInfo GetOpenTask(string uid, int flowId)
        {
            return _flow.GetOpenTask(uid, flowId);
        }

        #endregion task operations

        public static bool IsLastApprover(string uid, FlowInfo flow)
        {
            return _flow.IsLastApprover(uid, flow);
        }

        public static void UpdateStatus(int flowId, FlowStatus status)
        {
            _flow.UpdateStatus(flowId, status);
        }

        public static void CancelDelegateTasks(string uid, FlowInfo flow, ActionType action)
        {
            _flow.CancelDelegateTasks(uid, flow, action);
        }

        public static bool ChildAllClosed(int flowId)
        {
            return _flow.ChildAllClosed(flowId);
        }

        public static FlowInfo GetFlowByTaskId(int taskId)
        {
            return _flow.GetFlowByTaskId(taskId);
        }

        public static DataTable GetTasksByRequestId(int reqId)
        {
            return _flow.GetTasksByRequestId(reqId);
        }

        public static void UpdateTaskOwner(int flowId, int sno, string uid, string ouid, int roldId)
        {
            UpdateTask(sno, ActionType.Forward, "turn over to " + OrgMgr.GetUser(uid).DisplayName);

            _flow.AddTask(sno, uid);
            string rolename=string.Empty;
            if((Model.RoleType)roldId==Model.RoleType.Approver)
            {
                rolename="approver";
            }
            else if((Model.RoleType)roldId==Model.RoleType.Eng)
            {
                rolename="eng";
            }
            if(!string.IsNullOrEmpty(rolename))
                OrgMgr.UpdateApproverList(flowId, uid, ouid, rolename);
        }

        public static Dictionary<int, string> GetRequestStatusList()
        {
            return _flow.GetRequestStatusList();
        }

        public static bool IsParallelLastApprover(string userid, FlowInfo flow)
        {
            List<string> groups = OrgMgr.GetGroups(userid);
            if (groups.Count > 0)
                userid = groups[0];
            return _flow.IsLastApprover(userid, flow);
        }

        public static TaskInfo GetTask(int taskId)
        {
            return _flow.GetTask(taskId);
        }
    }
}