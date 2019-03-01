using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class OperationManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["oType"]))
                {
                    Operacation();
                    return;
                }

                BindData("");
                BindDDL();
            }
        }

        void Operacation()
        {
            string strOType = HttpUtility.HtmlDecode(Request["oType"].Trim());


            //0添,1删,2修,3查

            if (strOType == "0")
            {
                AddOrUpdate("0");
            }
            else if (strOType == "1")
            {
                string strID = Request["operationItemID"];
                string sql = string.Format("UPDATE OperationItem SET IsActive = 0 WHERE ID = {0} ", strID);
                int iRows = DAL.SqlHelper.ExecuteNonQuery(sql);
                if (iRows > 0)
                    Response.Write("1");
                else
                    Response.Write("0");
                Response.End();
            }
            else if (strOType == "2")
            {
                AddOrUpdate("1");
            }
            else if (strOType == "3")
            {
                string operationItemID = Request["operationItemID"];
                string sql = string.Format("SELECT  OI.*,OS.OperationStandard FROM OperationStandard AS OS LEFT JOIN OperationItem AS OI ON OI.ID = OS.OperationItemID WHERE  OI.ID = {0}", operationItemID);
                DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);

                if (dt != null && dt.Rows.Count > 0)
                    Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
                else
                    Response.Write("0");
                Response.End();
            }


        }


        void BindData(string strDDLValue)
        {
            string sql = "SELECT OI.*,OS.COUNTITEM FROM OperationItem AS OI LEFT JOIN (SELECT OperationItemID, COUNT(0) AS COUNTITEM FROM OperationStandard GROUP BY OperationItemID) OS ON OI.ID = OS.OperationItemID WHERE OI.IsActive = 1 ";
            if (!string.IsNullOrEmpty(strDDLValue) && strDDLValue != "ALL")
                sql = string.Format("SELECT OI.*,OS.COUNTITEM FROM OperationItem AS OI LEFT JOIN (SELECT OperationItemID, COUNT(0) AS COUNTITEM FROM OperationStandard GROUP BY OperationItemID) OS ON OI.ID = OS.OperationItemID WHERE  OI.IsActive = 1 AND OI.OperationType = N'{0}' ", strDDLValue);

            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);

            rptOperationManage.DataSource = dt;
            rptOperationManage.DataBind();

        }

        void BindDDL()
        {
            string sql = "SELECT DISTINCT OperationType FROM OperationItem";
            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);

            ddlOpeartionType.DataSource = dt;
            ddlOpeartionType.DataTextField = "OperationType";
            ddlOpeartionType.DataValueField = "OperationType";
            ddlOpeartionType.DataBind();
            ddlOpeartionType.Items.Insert(0, new ListItem("ALL", "ALL"));
            ddlOpeartionType.SelectedIndex = 0;

        }

        void AddOrUpdate(string oType)
        {
            List<string> lst = new List<string>();
            if (oType == "0")
            {
                //添加
                try
                {
                    string sql = string.Format("INSERT INTO OperationItem(OperationItem,OperationScore,OperationType,IsActive) VALUES(N'{0}',{1},N'{2}',1)  SELECT SCOPE_IDENTITY() ", Request["operationItem"], Convert.ToDecimal(Request["Score"]), Request["operationType"]);

                    DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                    {
                        int itemID = Convert.ToInt32(dt.Rows[0][0]);
                        for (int i = 5; i < Request.QueryString.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString[i]))
                                lst.Add(string.Format("INSERT INTO OperationStandard(OperationItemID,OperationStandard) VALUES(N'{0}',N'{1}') ", itemID, Request.QueryString[i]));
                        }
                        DAL.SqlHelper.ExecuteNonQueryTran(lst);
                        Common.PageHelper.Alert(this, "添加成功!", "OperationManage.aspx");
                    }

                }
                catch
                {
                    DAL.SqlHelper.ExecuteNonQueryTran(lst);
                    Common.PageHelper.Alert(this, "出错,添加失败!", "OperationManage.aspx");
                }



            }
            else if (oType == "1")
            {
                //修改

                lst.Add(string.Format("UPDATE OperationItem SET OperationItem=N'{0}',OperationScore={1},OperationType=N'{2}' WHERE ID = {3} ", Request["operationItem"], Convert.ToDecimal(Request["Score"]), Request["operationType"], Request["operationItemID"]));

                lst.Add(string.Format("DELETE OperationStandard WHERE OperationItemID = N'{0}'", Request.QueryString["operationItemID"]));

                for (int i = 5; i < Request.QueryString.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString[i]))
                        lst.Add(string.Format("INSERT INTO OperationStandard(OperationItemID,OperationStandard) VALUES(N'{0}',N'{1}') ", Request["operationItemID"], Request.QueryString[i]));
                }

                try
                {
                    DAL.SqlHelper.ExecuteNonQueryTran(lst);
                    Common.PageHelper.Alert(this, "修改成功!", "OperationManage.aspx");
                }
                catch
                {
                    Common.PageHelper.Alert(this, "修改失败!", "OperationManage.aspx");
                }

            }
            BindData("");
        }


        protected void ddlOpeartionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            string str = ddl.SelectedValue;
            BindData(str);
        }
    }
}