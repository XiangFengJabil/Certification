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
    public partial class TempDataImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                string newFileName = "ImportExamQuestion_" + dateTime + DateTime.Now.Millisecond.ToString() + extentionName;
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
                DataTable dtExcel = Common.ExcelHelper.ImportExcel(road, 0, out error);

                ////将Excel内容插入到数据库中
                Common.Common.CreateSqlTable(dtExcel, "Temp_ExamQuestion");
                int iRows = Common.UploadExcelFileBiz.WriteIntoDataBase(dtExcel, "Temp_ExamQuestion");

                string Question, QuestionType, QuestionResult, IsMultiple, sql;
                int Score;
                List<string> lstQuestion = new List<string>();
                List<string> lstQuestionResult = new List<string>();

                //插入问题
                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    Question = dtExcel.Rows[i]["F5"].ToString();
                    QuestionType = dtExcel.Rows[i]["F3"].ToString();
                    QuestionResult = dtExcel.Rows[i]["F1"].ToString();
                    IsMultiple = dtExcel.Rows[i]["F4"].ToString() == "是" ? "true" : "false";
                    Score = Convert.ToInt32(dtExcel.Rows[i]["F2"]);
                    sql = string.Format("INSERT INTO [dbo].[ExamQuestion]([Question],[QuestionType],[QuestionResult],[IsMultiple],[Score],[IsActive])VALUES(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',1)", Question, QuestionType, QuestionResult, IsMultiple, Score);
                    lstQuestion.Add(sql);
                }
                int rows = DAL.SqlHelper.ExecuteNonQueryTran(lstQuestion);

                //插入答案
                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    Question = dtExcel.Rows[i]["F5"].ToString();
                    QuestionType = dtExcel.Rows[i]["F3"].ToString();
                    for (int j = 0; j < dtExcel.Columns.Count; j++)
                    {
                        if (j > 4)
                        {
                            var v = dtExcel.Rows[i][j];

                            if (v != DBNull.Value && v != null && !string.IsNullOrEmpty(v.ToString()))
                            {
                                QuestionResult = dtExcel.Rows[i][j].ToString();

                                sql = string.Format("INSERT INTO ExamQuestionOptions(QuestionID,QuestionOption) SELECT ID,N'{0}' as s FROM ExamQuestion WHERE Question = N'{1}' AND QuestionType = N'{2}' ", QuestionResult, Question, QuestionType);
                                lstQuestionResult.Add(sql);
                            }
                        }
                    }

                }
                rows = DAL.SqlHelper.ExecuteNonQueryTran(lstQuestionResult);

                //Excel 1170    F=> options
                //ExamQuestion,ExamQuestionOptions
                //1.F1-F5插入到问题表中，返回ID。
                //2.将第一步返回的ID跟F6 -F11插入到问题答案表中（F6为字母A,F7为字母B,F8为字母C...）
                //3.更新问题表中的答案。

                Common.PageHelper.Alert(this, "Import data success！", "ExamQuestionManage.aspx");
                return;
            }
            catch
            {
                Common.PageHelper.Alert(this, "Import " + error + " data failed！");
                return;
            }
            #endregion
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            lst.Add("TRUNCATE TABLE ExamQuestion");
            lst.Add("TRUNCATE TABLE ExamQuestionOptions");
            lst.Add("TRUNCATE TABLE Temp_ExamQuestion");
            DAL.SqlHelper.ExecuteNonQueryTran(lst);
            Common.PageHelper.Alert(this, "数据清除成功！");
            return;

        }
    }
}