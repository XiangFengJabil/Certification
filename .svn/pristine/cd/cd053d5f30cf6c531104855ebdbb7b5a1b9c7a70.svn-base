using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Certification.Controls
{
    public partial class Menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Model.UserInfo user = BLL.OrgMgr.GetCurrentAdUser();
                this.lbUser.Text = user.DisplayName;
            }
        }


        public string GetUrl(string pageName)
        {
            string str = Request.Url.AbsoluteUri;
            str = str.Substring(0, str.LastIndexOf("/")) + "/" + pageName;
            return str;
        }
    }
}