using Certification.Model;
using System;
using System.Collections.Generic;
using System.Web;

namespace Certification.BLL
{
    public class OrgMgr
    {
        private static readonly DAL.Org _org = new DAL.Org();

        #region below function don't change
        private static UserInfo GetAdSingleUser(RoleType role)
        {
            return _org.GetUser((int)role);
        }

        public static List<UserInfo> GetAdUserList(RoleType role)
        {
            return _org.GetUserList((int)role);
        }

        public static UserInfo GetCurrentAdUser()
        {
            UserInfo user = null;
            if (HttpContext.Current.Session["ADUser"] == null)
            {
                string uid = Util.Common.GetCurrentNTID();
                //string uid = "lic153";
                user = GetUser(uid);

                if (user == null)
                {
                    user = Util.Common.GetADUserEntity(uid);
                }
                HttpContext.Current.Session["ADUser"] = user;
            }
            else
            {
                user = (UserInfo)HttpContext.Current.Session["ADUser"];
            }
            return user;
        }
        private static void SaveUser(UserInfo user)
        {
            _org.SaveUser(user);
        }

        public static UserInfo GetUser(string uid)
        {
            return _org.GetUser(uid);
        }
        #endregion

        #region To do delegate Func
        /// <summary>
        /// Get Role delegate user
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static UserInfo GetDelegator(string uid, int role)
        {
            return _org.GetDelegator(uid, role);
        }
        /// <summary>
        /// Get Org delegate user
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="wcid"></param>
        /// <returns></returns>
        public static UserInfo GetDelegator(string uid, int role, int orgId)
        {
            return _org.GetDelegator(uid, role, orgId);
        }

        #endregion

        public static IList<UserInfo> GetNTIDList(string prefixText)
        {
            return _org.GetNTIDList(prefixText);
        }

        public static bool ExistsCertification(string uid, string skillSN)
        {
            return _org.ExistsCertification(uid, skillSN);
        }

        public static bool VerifyCertification(string uid, string skillSN)
        {
            return _org.VerifyCertification(uid, skillSN);
        }

        public static bool IsAdmin(string uid)
        {
            List<UserInfo> us = GetAdUserList(RoleType.Admin);
            foreach (var item in us)
            {
                if (item.UID.ToUpper() == uid.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetGroups(string userId)
        {
            return _org.GetGroups(userId);
        }

        public static ApproverInfo GetLastEngApprover(int flowId)
        {
            return _org.GetLastEngApprover(flowId);
        }

        public static List<ApproverInfo> GetApprovers(int flowId, string appType)
        {
            return _org.GetApprovers(flowId, appType);
        }

        public static IList<UserInfo> GetGroupNTIDList(string prefixText)
        {
            return _org.GetGroupNTIDList(prefixText);
        }

        public static List<ApproverInfo> GetApprovers(int flowId)
        {
            return _org.GetApprovers(flowId);
        }

        public static Dictionary<int, string> GetForms(int siteID)
        {
            return _org.GetForms(siteID);
        }

        public static int GetSiteId(string siteName)
        {
            int siteId = 0;
            if (!string.IsNullOrEmpty(siteName))
            {
                siteId = _org.GetSiteId(siteName);
                if (siteId == -1)
                {
                    siteId = 0;
                }
            }
            return siteId;
        }

        public static Dictionary<int, string> GetSiteList()
        {
            return _org.GetSiteList();
        }

        public static void AddApproveList(int flowId, string approvers)
        {
            string[] apps = approvers.Split(',');
            for (int i = 0; i < apps.Length; i++)
            {
                Model.ApproverInfo approverInfo = new ApproverInfo
                {
                    FlowId = flowId,
                    UID = apps[i],
                    ApproveType = "approver",
                    IsGroup = 0,
                    SeqNo = (i + 1) * 10,
                };
                _org.AddApprover(approverInfo);
            }
        }

        public static void AddEngList(int flowId, string engs)
        {
            string[] apps = engs.Split(',');
            for (int i = 0; i < apps.Length; i++)
            {
                string[] e = apps[i].Split('-');
                Model.ApproverInfo approverInfo = new ApproverInfo
                {
                    FlowId = flowId,
                    UID = e[0],
                    ApproveType = "eng",
                    IsGroup = int.Parse(e[1]),
                    SeqNo = (i + 1) * 10,
                };
                _org.AddApprover(approverInfo);
            }
        }

        public static List<UserInfo> GetAdUserListByRoleName(string roleName)
        {
            return _org.GetAdUserListByRoleName(roleName);
        }

        public static void UpdateApproverList(int flowId, string uid, string ouid, string rolename)
        {
            _org.UpdateApproverList(flowId, uid, ouid, rolename);
        }
    }
}