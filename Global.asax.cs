using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Certification
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception unhandledException = Server.GetLastError();
            Exception actualException = unhandledException.InnerException;

            string errorMsg = String.Empty;
            string particular = String.Empty;
            if (actualException != null)
            {
                errorMsg = actualException.Message;
                particular = actualException.StackTrace;
            }
            else
            {
                errorMsg = unhandledException.Message;
                particular = unhandledException.StackTrace;
            }
            HttpContext.Current.Response.Write(errorMsg);

            Server.ClearError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}