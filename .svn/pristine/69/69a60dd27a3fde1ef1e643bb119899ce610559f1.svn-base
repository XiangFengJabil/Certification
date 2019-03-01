using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Certification.Handler
{
    /// <summary>
    /// Summary description for GetGroups
    /// </summary>
    public class GetGroups : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string prefixText = context.Request.QueryString["prefix"];
            string json = "[";
            IList<Model.UserInfo> _users = BLL.OrgMgr.GetGroupNTIDList(prefixText);
            foreach (var item in _users)
            {
                json += "{\"NTID\":\"" +
                                item.UID + "\",\"IsGroup\":" +
                                item.IsGroup + ",\"DisplayName\":\"" +
                                item.DisplayName + "\",\"Mail\":\"" + item.Email + "\"},";
            }
            if (json.IndexOf(',') >= 0)
            {
                json = json.Remove(json.Length - 1, 1);
            }
            json += "]";
            context.Response.Clear();
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}