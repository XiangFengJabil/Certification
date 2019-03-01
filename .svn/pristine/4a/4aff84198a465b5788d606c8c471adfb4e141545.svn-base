using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Certification.Model;

namespace Certification.BLL
{
    public static class WorkflowHandler
    {
        public static void HandleTask(UserInfo currentUser, int taskId, FlowInfo flow,
                        RequestInfo request, ActionType action, string comments)
        {
            IList<ActionType> accessList = GetRequestPermission(taskId, flow, request, currentUser.UID);

            //judge current user whether have permission to do the action
            if (accessList.IndexOf(action) < 0)
            {
                throw new Exception("You don't have permission to do this action.");
            }

            WorkflowMgr.UpdateTask(taskId, action, comments);
            //FlowStatus oldStatus = flow.Status;
            FlowStatus nextStatus = flow.Status;
            Dictionary<ApproverInfo, RoleType> nextUser = new Dictionary<ApproverInfo, RoleType>();
            if (flow.Status == FlowStatus.Approved)
            {
                if (action == ActionType.Commit)
                {
                    if (request.FlowType == 1)   // 并行
                    {
                        if (WorkflowMgr.IsParallelLastApprover(currentUser.UID, flow))
                        {
                            nextStatus = FlowStatus.Closed;
                            WorkflowMgr.UpdateStatus(flow.FlowId, nextStatus);

                            SendMail(currentUser, request, flow, nextUser, comments);
                        }
                    }
                    else  //串行
                    {
                        List<ApproverInfo> approvers = OrgMgr.GetApprovers(flow.FlowId).FindAll(obj => { return obj.ApproveType == "eng"; });

                        TaskInfo ts = WorkflowMgr.GetTask(taskId);
                        if (ts.UID.ToLower() == approvers[approvers.Count - 1].UID.ToLower())
                        {
                            nextStatus = FlowStatus.Closed;
                            WorkflowMgr.UpdateStatus(flow.FlowId, nextStatus);

                            SendMail(currentUser, request, flow, nextUser, comments);
                        }
                        else
                        {
                            int idx = approvers.FindIndex(obj => { return obj.UID.ToLower() == ts.UID.ToLower(); });
                            nextUser.Add(approvers[idx + 1], RoleType.Eng);

                            AddTasks(nextUser, flow);
                            //send mail to next eng
                            SendMail(currentUser, request, flow, nextUser, comments);
                        }
                    }
                }
                else if (action == ActionType.Cancel)
                {
                    nextStatus = FlowStatus.Canceled;
                    WorkflowMgr.CancelAllOtherTasks(currentUser, flow, action);

                    WorkflowMgr.UpdateStatus(flow.FlowId, nextStatus);

                    SendMail(currentUser, request, flow, nextUser, comments);
                }
                else
                {
                    throw new Exception("Wrong Action");
                }
            }
            else
            {
                SF_MoveAfterTaskDone(currentUser, flow, request, action, comments);
            }
        }

        private static void SF_MoveAfterTaskDone(UserInfo currentUser, FlowInfo flow, RequestInfo request, ActionType action, string comments)
        {
            FlowStatus nextStatus = flow.Status;
            switch (flow.Status)
            {
                #region case RequestStatus.Created
                case FlowStatus.Created:
                    switch (action)
                    {
                        case ActionType.Submit:
                            nextStatus = FlowStatus.Submitted;
                            break;
                        default:
                            throw new Exception("Wrong Action");
                    }
                    break;
                #endregion

                #region case RequestStatus.Submitted
                case FlowStatus.Submitted:
                    switch (action)
                    {
                        case ActionType.Approve:
                            if (WorkflowMgr.IsLastApprover(currentUser.UID, flow))
                            {
                                nextStatus = FlowStatus.Approved;
                            }
                            break;
                        case ActionType.Reject:
                            nextStatus = FlowStatus.Rejected;
                            WorkflowMgr.CancelAllOtherTasks(currentUser, flow, action);
                            break;
                        case ActionType.Cancel:
                            nextStatus = FlowStatus.Canceled;
                            WorkflowMgr.CancelAllOtherTasks(currentUser, flow, action);     // if cancel, then not need manager approval any more.
                            break;
                        default:
                            throw new Exception("Wrong Action");
                    }
                    break;

                #endregion
            }

            if (nextStatus != flow.Status)
                SF_MoveTo(currentUser, flow, nextStatus, request, comments);
        }

        private static void SF_MoveTo(UserInfo currentUser, FlowInfo flow, FlowStatus nextStatus, RequestInfo request, string comments)
        {
            Dictionary<ApproverInfo, RoleType> nextUser = new Dictionary<ApproverInfo, RoleType>();

            switch (nextStatus)
            {
                #region case RequestStatus.Submitted
                case FlowStatus.Submitted:
                    //List<UserInfo> users = OrgMgr.GetApprovers(flow.FlowId, "approver");
                    List<ApproverInfo> approvers = OrgMgr.GetApprovers(flow.FlowId).FindAll(obj => { return obj.ApproveType == "approver"; });
                    foreach (var item in approvers)
                    {
                        nextUser.Add(item, RoleType.Approver);
                    }
                    break;
                #endregion

                #region case RequestStatus.Approved
                case FlowStatus.Approved:
                    List<ApproverInfo> engs = OrgMgr.GetApprovers(flow.FlowId).FindAll(obj => { return obj.ApproveType == "eng"; });
                    if (request.FlowType == 1)  //并行
                    {
                        foreach (var item in engs)
                        {
                            nextUser.Add(item, RoleType.Eng);
                        }
                    }
                    else  //串行
                    {
                        nextUser.Add(engs[0], RoleType.Eng);
                    }
                    break;
                #endregion
            }

            WorkflowMgr.UpdateStatus(flow.FlowId, nextStatus);
            flow.Status = nextStatus;
            AddTasks(nextUser, flow);

            SendMail(currentUser, request, flow, nextUser, comments);
        }

        private static void SendMail(UserInfo currentUser, RequestInfo request, FlowInfo flow, Dictionary<ApproverInfo, RoleType> nextUser, string comments)
        {
            // send mail
            List<ApproverInfo> taskUsers = new List<ApproverInfo>();
            taskUsers.AddRange(nextUser.Keys);

            MailHandler.SendMail(currentUser, request, flow, taskUsers, comments);
        }

        private static void AddTasks(Dictionary<ApproverInfo, RoleType> nextUser, FlowInfo flow)
        {
            if (nextUser.Count == 0) return;
            // add task
            foreach (var u in nextUser)
            {
                WorkflowMgr.AddTask(u.Key.UID, flow.FlowId, u.Value);
            }
        }

        public static IList<ActionType> GetRequestPermission(int taskId, FlowInfo flow, RequestInfo reqInfo, string userId)
        {
            List<ActionType> actions = new List<ActionType>();

            TaskInfo ts = WorkflowMgr.GetOpenTask(taskId);

            #region taskowners' permissions
            // ReqID<0 if for new creation request, no need check open task.
            if (ts != null &&
                (ts.UID.ToLower() == userId.ToLower()
                    || OrgMgr.GetGroups(userId).Contains(ts.UID)))
            {
                switch (flow.Status)
                {
                    case FlowStatus.Created:
                        actions.Add(ActionType.Submit);
                        break;
                    case FlowStatus.Submitted:
                        actions.Add(ActionType.Approve);
                        actions.Add(ActionType.Reject);
                        break;
                    case FlowStatus.Approved:
                        actions.Add(ActionType.Commit);
                        break;
                }
            }

            #endregion

            #region requstor's permissions

            if (RequestMgr.IsRequestOwner(userId, reqInfo))
            {
                switch (flow.Status)
                {
                    case FlowStatus.Created:
                        actions.Add(ActionType.Submit);
                        break;
                    case FlowStatus.Submitted:
                    case FlowStatus.Approved:
                        actions.Add(ActionType.Cancel);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            return actions;
        }
    }
}