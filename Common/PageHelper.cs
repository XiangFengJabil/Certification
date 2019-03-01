using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Certification.Common
{
    public static class PageHelper
    {
        public static void Alert(Page objPage, string message)
        {
            string key = "AlertMessage";
            string script = string.Format("alert('{0}')", message);
            objPage.ClientScript.RegisterStartupScript(typeof(Page), key, script, true);
        }

        public static void Alert(Page objPage, string message, string url)
        {
            string key = "AlertMessage";
            string script = String.Format("alert('{0}');window.location='{1}';", message, url);
            objPage.ClientScript.RegisterStartupScript(typeof(Page), key, script, true);
        }
    }
}