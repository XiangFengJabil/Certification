using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class Tasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Model.UserInfo user = BLL.OrgMgr.GetCurrentAdUser();

            DataTable approvalTable = BLL.RequestMgr.GetRequestsByTaskOwner(user.UID);
            if (approvalTable.Rows.Count > 0)
            {
                this.dgvRequests.DataSource = approvalTable;
                this.dgvRequests.DataBind();
            }
        }
    }
}