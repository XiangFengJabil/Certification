using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class OperationRecord : BasePage
    {
        IList<OperationRecordInfo> operationRecord;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData("", true);
        }

        void BindData(string strDDLStr, bool isBindDDL)
        {
            UserRoleControl();

            string sql = "SELECT * FROM OperationRecord  WHERE ExamTotalScore >= 80 ORDER BY CreateDate DESC";
            if (!string.IsNullOrEmpty(txtUserName.Text.Trim()))
                sql = string.Format("SELECT * FROM OperationRecord WHERE DisplayName = N'{0}' AND ExamTotalScore >= 80  ORDER BY CreateDate DESC", txtUserName.Text.Trim());

            if (!string.IsNullOrEmpty(strDDLStr) && strDDLStr != "ALL")
                sql = string.Format("SELECT * FROM OperationRecord WHERE SkillSn = N'{0}' AND ExamTotalScore >= 80 ORDER BY CreateDate DESC", strDDLStr);


            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            operationRecord = Common.ModelConvertHelper<OperationRecordInfo>.ConvertToModel(dt);
            rptOperationItem.DataSource = dt;
            rptOperationItem.DataBind();

            sql = "SELECT * FROM OperationRecord  WHERE ExamTotalScore < 80 ORDER BY CreateDate DESC";
            rptTheoryExamFailed.DataSource = DAL.SqlHelper.GetDataTableOfRecord(sql);
            rptTheoryExamFailed.DataBind();


            if (isBindDDL)
            {
                sql = "SELECT DISTINCT SkillSn FROM OperationRecord WHERE ExamTotalScore >= 80 ";
                dt = DAL.SqlHelper.GetDataTableOfRecord(sql);

                ddlSkillSN.DataSource = dt;
                ddlSkillSN.DataTextField = "SkillSn";
                ddlSkillSN.DataValueField = "SkillSn";
                ddlSkillSN.DataBind();

                ddlSkillSN.Items.Insert(0, new ListItem("ALL(SkillSN)", "ALL"));
                if (string.IsNullOrEmpty(strDDLStr))
                    ddlSkillSN.SelectedIndex = 0;
                else
                    ddlSkillSN.SelectedValue = strDDLStr;

            }

        }


        /// <summary>
        /// 权限
        /// </summary>
        void UserRoleControl()
        {
            if (userRole == 0)
            {
                divRole.Style.Add("display", "none");
            }
        }


        public string OutHtmlTd(object objID, object objIsOperationApproved)
        {
            string str = "";
            if (userRole == 1)
            {
                if (string.IsNullOrEmpty(objIsOperationApproved.ToString()) || objIsOperationApproved.ToString() == "False")
                    str = "<td style='text-align:center;' ><a style='color: #006699;' href='Operation.aspx?id=" + objID.ToString() + "'>评分</a></td>";
                else
                {
                    //< 73 不合格 73~87.5 合格 ﹥87.5 优秀
                    decimal totalScore = operationRecord.Where(s => s.ID == Convert.ToInt32(objID)).ToList()[0].TotalScore;

                    if (totalScore < 73)
                        str = "<td style='text-align:center;' >已评分<span style='color:red;margin-left:10px;'>不合格</span></td>";
                    else if (totalScore > 73 && totalScore < 87.5m)
                        str = "<td style='text-align:center;'>已评分<span style='color:orange;margin-left:10px;'>合格</span></td>";
                    else
                        str = "<td style='text-align:center;'>已评分<span style='color:green;margin-left:10px;'>优秀</span></td>";
                }
            }
            return str;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
                ddlTotal.SelectedIndex = 0;
            BindData("", true);
        }

        protected void ddlSkillSN_SelectedIndexChanged(object sender, EventArgs e)
        {


            DropDownList ddl = sender as DropDownList;
            string str = ddl.SelectedValue;
            BindData(str, false);
        }

        protected void ddlTotal_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = (sender as DropDownList).SelectedValue;
            BindResult(str);
        }

        void BindResult(string selectValue)
        {
            //0ALL,1合格,2优秀，3不合格，4未评分
            string sql = "SELECT * FROM OperationRecord ORDER BY CreateDate DESC";

            if (selectValue == "3")
                sql = "SELECT * FROM OperationRecord WHERE TotalScore < 73 AND ExamTotalScore >= 80 order by AssessmentDate desc";
            else if (selectValue == "2")
                sql = "SELECT * FROM OperationRecord WHERE TotalScore > 87.5 AND ExamTotalScore >= 80 order by AssessmentDate desc";
            else if (selectValue == "1")
                sql = "SELECT * FROM OperationRecord WHERE TotalScore >= 73 AND TotalScore <= 87.5 AND ExamTotalScore >= 80 order by AssessmentDate desc";
            else if (selectValue == "4")
                sql = "SELECT * FROM OperationRecord WHERE IsOperationApproved = 0 AND ExamTotalScore >= 80 order by AssessmentDate desc";

            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            operationRecord = Common.ModelConvertHelper<OperationRecordInfo>.ConvertToModel(dt);
            rptOperationItem.DataSource = dt;
            rptOperationItem.DataBind();
        }

    }
}