using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Certification.Handler
{
    /// <summary>
    /// Summary description for GetUserlist
    /// </summary>
    public class GetUserlist : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string prefixText = context.Request.QueryString["prefix"];
            string json = "[";
            IList<Model.UserInfo> _users = BLL.OrgMgr.GetNTIDList(prefixText);
            foreach (var item in _users)
            {
                json += "{\"NTID\":\"" +
                             item.UID + "\",\"DisplayName\":\"" +
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