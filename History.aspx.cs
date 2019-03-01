using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class History : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Bind("", true);
        }

        public string handleTable = "";

        void Bind(string strHandleType, bool isBindDDL)
        {
            if (!string.IsNullOrEmpty(Request["handle"]))
            {
                handleTable = Request["handle"];
                string sql = string.Format("SELECT * FROM HandleHistory WHERE HandleTable LIKE '%{0}%'", handleTable);

                if (!string.IsNullOrEmpty(strHandleType))
                    sql += " AND HandleType = '" + strHandleType + "'";

                sql += " ORDER BY HandleDateTime DESC ";

                DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                rptHandle.DataSource = dt;
                rptHandle.DataBind();

                if (!isBindDDL) return;

                sql = "SELECT DISTINCT HandleType FROM HandleHistory";
                dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                //HandleType
                ddlHandleType.DataSource = dt;
                ddlHandleType.DataTextField = "HandleType";
                ddlHandleType.DataValueField = "HandleType";
                ddlHandleType.DataBind();

                ddlHandleType.Items.Insert(0, new ListItem("ALL", "ALL"));

                if (string.IsNullOrEmpty(strHandleType))
                    ddlHandleType.SelectedIndex = 0;
                else
                    ddlHandleType.SelectedValue = strHandleType;


            }
        }

        protected void ddlHandleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            string str = ddl.SelectedValue;
            if (ddl.SelectedIndex == 0)
            {
                Bind("", true);
            }
            else
            {
                string strHandleType = ddlHandleType.SelectedValue;
                Bind(strHandleType, false);
            }
        }
    }
}