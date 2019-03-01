using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Certification.BLL;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.OleDb;
using System.Collections;

namespace Certification
{
    public partial class RequestList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) return;

            UserRoleControl();

            //DataTable dt = RequestMgr.GetAllRequests();
            //第一次进入，数据量太大,注释上面，只显示最后的100条记录item
            string sql = "select top 100 * from Certification  order by item asc";
            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            Session["Certification"] = dt;

            rptCertificate.DataSource = dt;
            rptCertificate.DataBind();
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //string site = this.ddlSites.SelectedValue;
            //string form = this.ddlForms.SelectedValue;
            //string status = this.ddlStatus.SelectedValue;
            string item = this.ddlItems.SelectedValue;
            string text = this.txtItem.Text.Trim();

            //string date = this.ddlDate.SelectedValue;
            //string startDate = this.txtStartDate.Text.Trim();
            //string endDate = this.txtEndDate.Text.Trim();

            DataTable dt = RequestMgr.GetAllRequestsByFilter(item, text);
            Session["Certification"] = dt;
            rptCertificate.DataSource = dt;
            rptCertificate.DataBind();
        }

        protected void btnUploadExcel_Click(object sender, EventArgs e)
        {
            //上传excel文件
            string road = "";
            #region 文件上传
            try
            {
                //全名
                string excelFile = this.FileUpload1.PostedFile.FileName;
                //获取文件名（不包括扩展名）
                string fileName = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
                if (fileName == "" || fileName == null)
                {
                    Common.PageHelper.Alert(this, "Select Excel File！");
                    return;
                }
                //扩展名
                string extentionName = excelFile.Substring(excelFile.LastIndexOf(".")).ToLower();
                if (extentionName != ".xls" && extentionName != ".xlsx")
                {
                    Common.PageHelper.Alert(this, "Not an Excel file！");
                    return;
                }
                //浏览器安全性限制 无法直接获取客户端文件的真实路径，将文件上传到服务器端 然后获取文件源路径

                #region 设置上传路径将文件保存到服务器
                string dateTime = DateTime.Now.ToString("yyyyMMddhhmmssffff");
                string newFileName = "UpdateData_" + dateTime + DateTime.Now.Millisecond.ToString() + extentionName;
                //TempExcel是自己创建的文件夹  位置随意   合理即可
                road = Server.MapPath("./uploads/TempExcel") + "\\" + newFileName;
                this.FileUpload1.PostedFile.SaveAs(road);
                //Response.Write("<script>alert('已经上传到服务器文件夹')</script>");
                #endregion

            }
            catch
            {
                Common.PageHelper.Alert(this, "Data upload failed, please re-import");
                return;
            }
            #endregion


            int error = 0;
            #region 数据导入到数据库中
            try
            {
                //将Excel文件转成Datatable
                //DataTable dtExcel = Common.UploadExcelFileBiz.GetExcelDatatable(road);
                DataTable dtExcel = Common.ExcelHelper.ImportExcel(road, out error);

                //将Dtatable的列名与数据库的列名保持一致
                int errorRow = 0;
                dtExcel = Common.UploadExcelFileBiz.ModifyColumnsForCertificationTable(dtExcel, out errorRow);
                if (errorRow != 0)
                {
                    Common.PageHelper.Alert(this, "IError, Import data failed！ Error Data Line " + errorRow);
                    return;
                }

                //将Excel内容插入到数据库中
                int iRows = Common.UploadExcelFileBiz.WriteIntoDataBase(dtExcel, "Certification");
                if (iRows > 0)
                    Common.PageHelper.Alert(this, "Import data success！");
                else
                    Common.PageHelper.Alert(this, "Error, Import data failed！");
            }
            catch
            {
                Common.PageHelper.Alert(this, "Import " + error + " data failed！");
                return;
            }
            #endregion
        }


        public string DateConvert(object obj)
        {
            if (DBNull.Value == obj || obj == null | obj == null || obj.ToString().Trim() == "") return "";
            else
            {
                return Convert.ToDateTime(obj).ToString("yyyy/MM/dd");
            }
        }


        void UserRoleControl()
        {
            if (userRole == 1)
            {
                FileUpload1.Visible = true;
                btnUploadExcel.Visible = true;
            }
            else
            {
                FileUpload1.Visible = false;
                btnUploadExcel.Visible = false;
            }
        }

        protected void btnDataExport_Click(object sender, EventArgs e)
        {
            //data export
            if (Session["Certification"] == null) return;

            DataTable dt = Session["Certification"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string fileName = "Certification_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx";
                string path = Server.MapPath("uploads/TempExcel/");
                Common.NPOIHelper.TableToExcel(dt, path + fileName);
                Common.NPOIHelper.DownLoadExcel(path, fileName);
            }
            else
                Response.Write("");
        }
    }
}