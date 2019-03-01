using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class DeptHead : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind("", true);
                Model.UserInfo user = BLL.OrgMgr.GetCurrentAdUser();
                if (user.UID == "1518819" || user.UID == "1258504" || user.UID == "1189648" || user.UID == "xuh4")
                {
                    FileUploadDeptHead.Visible = true;
                    btnUploadDeptHead.Visible = true;
                }
                UserRoleControl();
            }
        }


        void Bind(string strWhere, bool isBindDDL)
        {
            //第一次进来可能表不存在
            string sql = "SELECT * FROM SYSOBJECTS WHERE NAME =  'UploadDeptHead'";
            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                sql = "SELECT * FROM UploadDeptHead WHERE 1 = 1 ";
                if (!string.IsNullOrEmpty(strWhere))
                {
                    sql += " and " + strWhere;
                }
                dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                rptDeptHead.DataSource = dt;
                rptDeptHead.DataBind();

                if (isBindDDL)
                {
                    sql = "SELECT DISTINCT [部门] FROM UploadDeptHead";
                    dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ddl.DataSource = dt;
                        ddl.DataTextField = "部门";
                        ddl.DataValueField = "部门";
                        ddl.DataBind();
                        ddl.Items.Insert(0, new ListItem("ALL", "ALL"));
                        ddl.SelectedIndex = 0;
                    }
                    else
                        ddl.Visible = false;
                }

            }
        }

        protected void btnUploadDeptHead_Click(object sender, EventArgs e)
        {
            btnUploadDeptHead.Enabled = false;
            //上传Excel更新到数据库
            string road = "";
            #region 上传Excel
            try
            {
                #region 设置上传路径将文件保存到服务器
                //全名
                string excelFile = this.FileUploadDeptHead.PostedFile.FileName;
                //获取文件名（不包括扩展名）
                string fileName = Path.GetFileNameWithoutExtension(FileUploadDeptHead.PostedFile.FileName);
                //扩展名
                string extentionName = excelFile.Substring(excelFile.LastIndexOf(".") + 1).ToLower();
                if (fileName == "" || fileName == null)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Select Excel File！');window.location.href=window.location.href;", true);
                    return;
                }
                if (extentionName != "xls" && extentionName != "xlsx")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Not an Excel file！');window.location.href=window.location.href;", true);
                    return;
                }
                //浏览器安全性限制 无法直接获取客户端文件的真实路径，将文件上传到服务器端 然后获取文件源路径

                string dateTime = DateTime.Now.ToString("yyyyMMddhhmmssffff");
                string newFileName = "UploadDeptHead_" + dateTime + ".xlsx";
                //TempExcel是自己创建的文件夹  位置随意   合理即可
                road = Server.MapPath("uploads/TempExcel") + "\\" + newFileName;
                this.FileUploadDeptHead.PostedFile.SaveAs(road);
                #endregion

                //将Excel文件转成Datatable中
                DataTable dtExcel = Common.NPOIHelper.ExcelToTable(road);
                string tempTable = "UploadDeptHead";
                ///1.获取datatble的表结构，插入到数据中。                
                int iRow = Common.Common.CreateSqlTable(dtExcel, tempTable);
                if (iRow > 0)
                {
                    //2.将数据插入到数据库中,先清空原来的数据。
                    int insertCount = DAL.SqlHelper.WriteIntoDataBase(dtExcel, tempTable);
                    if (insertCount == dtExcel.Rows.Count)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Upload Dept Head  Successfully!');window.location.href=window.location.href;", true);
                        return;

                        #region Email
                        ////去重后的部门
                        //DataTable dtDept = GetAllDept();
                        ////一次查出所有人员证书类型,证书号避免多次查询
                        //DataTable dtAllUser = GetAllWorkcellName();

                        //if (dtDept != null && dtDept.Rows.Count > 0)
                        //{
                        //    string strBody = System.Configuration.ConfigurationManager.AppSettings["emailBody"];
                        //    //发送次数  "Xiang_Feng@jabil.com,Jerry_Wang1@jabil.com,Hongshan_Xu@Jabil.com";
                        //    for (int i = 0; i < dtDept.Rows.Count; i++)
                        //    {
                        //        string strDeptName = dtDept.Rows[i]["部门"].ToString();
                        //        string deptUser = GetUserByDept(dtAllUser, "Workcell", strDeptName);

                        //        string to = GetEmail(dtExcel, "部门", dtDept.Rows[i]["部门"].ToString());
                        //        //string to = "Xiang_Feng@jabil.com";

                        //        if (!string.IsNullOrEmpty(to) && !string.IsNullOrEmpty(deptUser))
                        //        {
                        //            string cc = "Xiang_Feng@jabil.com,Jerry_Wang1@jabil.com,Hongshan_Xu@Jabil.com";
                        //            string subject = "员工岗位技能认证到期提醒";
                        //            string body = "<p style='text-indent:25px;'>【<span style='color:red;'>重要提醒！！！</span>】" + strBody + "<br /><br />" + strDeptName + " 部门年检人员名单如下：<br /> " + TableStyle() + "<table  cellspacing='0'><tr><th> 人员 </th><th> 编号 </th></tr >" + deptUser + "</table></p>";

                        //            Certification.Util.Common.ClientSendMail(to, cc, subject, body);

                        //        }
                        //    }
                        //    Response.Write("<script>alert('Mail sent successfully!');window.location.href=window.location.href;</script>");
                        //    return;
                        //}
                        #endregion
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "Error importing data');window.location.href=window.location.href;", true);
                        return;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "Error,Importing data structure！');window.location.href=window.location.href;", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Common.Common.WriteTextLog("Upload Dept Head ", ex.ToString(), DateTime.Now);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "Data upload failed, please re-import');window.location.href=window.location.href;", true);
                return;
            }
            #endregion
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl.SelectedValue.Trim() == "ALL")
                Bind("", false);
            else
                Bind(string.Format(" [部门]= '{0}'", ddl.SelectedValue), false);

        }

        void UserRoleControl()
        {
            if (userRole == 0)
            {
                spanLiable.Visible = false;
                FileUploadDeptHead.Visible = false;
                btnUploadDeptHead.Visible = false;
            }
        }

    }
}