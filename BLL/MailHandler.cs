using Certification.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Certification.BLL
{
    public class MailHandler
    {

        private static readonly string EMAILBOTTOM =
                "<p><i><font color=\"red\">Please do not reply this mail.</font></i></p>"
                + "<p>Purchasing eForm System</p>";

        /// <summary>
        /// email reminder
        /// </summary>
        /// <param name="currentUser">current user</param>
        /// <param name="request">request info</param>
        /// <param name="nextUser">next action users</param>
        /// <param name="comments">remark</param>
        public static void SendMail(UserInfo currentUser, RequestInfo request, FlowInfo flow, List<ApproverInfo> nextUser, string comments)
        {
            switch (flow.Status)
            {
                case FlowStatus.Submitted:
                    SendMailToApproval(request, currentUser, nextUser);
                    break;
                case FlowStatus.Approved:
                    SendMailToEng(request, currentUser, nextUser);
                    break;
                case FlowStatus.Closed:
                    SendMailForClose(request, currentUser);
                    break;
                case FlowStatus.Rejected:
                    SendMailForReject(request, currentUser, comments);
                    break;
                case FlowStatus.Canceled:
                    SendMailForCancel(request, currentUser);
                    break;
                default:
                    break;
            }
        }

        #region Email function
        private static void SendMailToApproval(RequestInfo request, UserInfo currentUser, List<ApproverInfo> nextUser)
        {
            if (nextUser == null) return;
            if (nextUser.Count == 0) return;

            string subject = string.Format("Use form category [{0}] waiting for your approval",
                                request.ReqID.ToString("000000")
                            );
            string to = GetUserMails(nextUser);
            string body = "Dear Approver,"
                        + "<p>This is " + request.FormName + " request need your approval.</p>"
                        + "<p><b>Requestor:</b>" + request.User.DisplayName + "</p>"
                        + "<p>Approve link: <a href=\"" + Util.Common.GetCurrentUrl() + "Request.aspx?reqId="
                        + request.ReqID.ToString() + "\">[" + request.ReqID.ToString("000000") + "]</a></p><br/><br/>"
                        + EMAILBOTTOM;
            Util.Common.ClientSendMail(to, currentUser.Email, subject, body);
        }

        private static void SendMailToEng(RequestInfo request, UserInfo currentUser, List<ApproverInfo> nextUser)
        {
            if (nextUser == null) return;
            if (nextUser.Count == 0) return;

            string subject = string.Format("Use form category [{0}] waiting for your complete",
                                request.ReqID.ToString("000000")
                            );
            string to = GetUserMails(nextUser);
            string body = "Dear Engineer,"
                        + "<p>" + request.FormName + " waiting for your complete.</p>"
                        + "<p>Approve link: <a href=\"" + Util.Common.GetCurrentUrl() + "Request.aspx?reqId="
                        + request.ReqID.ToString() + "\">[" + request.ReqID.ToString("000000") + "]</a></p><br/><br/>"
                        + EMAILBOTTOM;
            Util.Common.ClientSendMail(to, currentUser.Email, subject, body);
        }

        private static void SendMailForCancel(RequestInfo request, UserInfo currentUser)
        {
            string subject = string.Format("Use form category [{0}] have been canceled",
                        request.ReqID.ToString("000000")
                    );
            string body = "Dear Requestor,"
                        + "<p>" + request.FormName + " have been canceled by " + currentUser.DisplayName + "</p>"
                        + "<p>Approve link: <a href=\"" + Util.Common.GetCurrentUrl() + "Request.aspx?reqId="
                        + request.ReqID.ToString() + "\">[" + request.ReqID.ToString("000000") + "]</a></p><br/><br/>"
                        + EMAILBOTTOM;
            Util.Common.ClientSendMail(request.User.Email, "", subject, body);
        }

        private static void SendMailForReject(RequestInfo request, UserInfo currentUser, string comments)
        {
            string subject = string.Format("Use form category [{0}] have been rejected",
                        request.ReqID.ToString("000000")
                    );
            string body = "Dear Requestor,"
                        + "<p>" + request.FormName + " have been rejected by " + currentUser.DisplayName + "</p>"
                        + "<p>Remark:<i>" + comments + "</i></p>"
                        + "<p>Approve link: <a href=\"" + Util.Common.GetCurrentUrl() + "Request.aspx?reqId="
                        + request.ReqID.ToString() + "\">[" + request.ReqID.ToString("000000") + "]</a></p><br/><br/>"
                        + EMAILBOTTOM;
            Util.Common.ClientSendMail(request.User.Email, currentUser.Email, subject, body);
        }

        private static void SendMailForClose(RequestInfo request, UserInfo currentUser)
        {
            string subject = string.Format("Use form category [{0}] have been completed",
                       request.ReqID.ToString("000000")
                   );
            string body = "Dear " + request.User.DisplayName + ","
                        + "<p>" + request.FormName + " have been completed.</p>"
                        + "<p>Approve link: <a href=\"" + Util.Common.GetCurrentUrl() + "Request.aspx?reqId="
                        + request.ReqID.ToString() + "\">[" + request.ReqID.ToString("000000") + "]</a></p><br/><br/>"
                        + EMAILBOTTOM;
            Util.Common.ClientSendMail(request.User.Email, currentUser.Email, subject, body);
        }

        private static string GetUserMails(List<ApproverInfo> users)
        {
            string mails = string.Empty;

            foreach (ApproverInfo u in users)
            {
                if (u.IsGroup == 1)
                {
                    List<UserInfo> us = OrgMgr.GetAdUserListByRoleName(u.UID);
                    foreach (var item in us)
                    {
                        if (!string.IsNullOrEmpty(item.Email))
                        {
                            mails += item.Email + ",";
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(u.Mail))
                    {
                        mails += u.Mail + ",";
                    }
                }

            }

            return mails.TrimEnd(',');
        }
        #endregion
    }
}