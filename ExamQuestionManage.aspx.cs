﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class ExamQuestionManage : BasePage
    {
        public int countActiveQuestion = 0;
        public int countDelQuestion = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["operationType"]))
                {
                    Operation(Request["operationType"].Trim());
                    return;
                }
                BindData("", true, true);
            }
        }


        void Operation(string strOperationType)
        {
            string qID = Request["qID"];
            //增加
            if (strOperationType == "0")
                AddOrUpdate(qID);
            else if (strOperationType == "1")
                //修改
                AddOrUpdate(qID);
            else if (strOperationType == "2")
            {
                //删除
                string sql = string.Format("UPDATE ExamQuestion SET IsActive = 0 WHERE ID = " + qID);
                int rowCount = DAL.SqlHelper.ExecuteNonQuery(sql);
                if (rowCount > 0)
                    Response.Write("1");
                else
                    Response.Write("0");
                Response.End();

            }
            else if (strOperationType == "3")
            {
                //查看
                string sql = string.Format("SELECT * FROM ExamQuestionOptions  AS eqo LEFT JOIN ExamQuestion AS EQ ON EQO.QuestionID = EQ.ID WHERE eqo.QuestionID = {0}", qID);

                DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                Response.Write(str);
                Response.End();
            }
        }

        void BindData(string str, bool isBindDll, bool isFirstEntry)
        {
            string sql = "SELECT * FROM ExamQuestion WHERE IsActive = 1 ";
            if (isFirstEntry)//第一次进入查询所有的页面会变慢
                sql = "SELECT TOP 100 * FROM ExamQuestion WHERE IsActive = 1 ORDER BY ID DESC ";
            if (!string.IsNullOrEmpty(str))
                sql = string.Format("SELECT * FROM ExamQuestion WHERE IsActive = 1 AND QuestionType = N'{0}'", str);

            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            rptExamQuestion.DataSource = dt;
            rptExamQuestion.DataBind();

            sql = "SELECT DISTINCT SkillSn FROM SkillCertification WHERE IsActive = 1 ";
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            foreach (DataRow dr in dt.Rows)
            {
                SkillCN.Items.Add(new ListItem(dr["SkillSn"].ToString(), dr["SkillSn"].ToString()));
            }

            if (isBindDll)
            {
                //sql = "SELECT DISTINCT QuestionType FROM ExamQuestion";
                //sql = "SELECT DISTINCT SkillSn FROM SkillCertification WHERE IsActive = 1";
                //dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                ddlQuestion.DataSource = dt;
                ddlQuestion.DataTextField = "SkillSn";
                ddlQuestion.DataValueField = "SkillSn";
                ddlQuestion.DataBind();

                ddlQuestion.Items.Insert(0, new ListItem("ALL(最近100题)", "ALL"));

                if (string.IsNullOrEmpty(str))
                    ddlQuestion.SelectedIndex = 0;
                else
                    ddlQuestion.SelectedValue = str;



            }

            sql = "SELECT COUNT(0) AS COUNTQ FROM ExamQuestion AS EQ WHERE EQ.IsActive = 1";
            countActiveQuestion = Convert.ToInt32(DAL.SqlHelper.ExecuteScalar(sql));

            sql = "SELECT COUNT(0) AS COUNTQ FROM ExamQuestion AS EQ WHERE EQ.IsActive = 0";
            countDelQuestion = Convert.ToInt32(DAL.SqlHelper.ExecuteScalar(sql));

        }

        protected void ddlQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            string str = ddl.SelectedValue;
            if (ddl.SelectedIndex == 0)
            {
                BindData("", false, true);
            }
            else
            {
                BindData(str, false, true);
            }

        }

        void AddOrUpdate(string qID)
        {

            string strQuestion = Request["Question"];
            string strScore = Request["Score"];
            string strQuestionResult = Request["QuestionResult"] == null ? "" : Request["QuestionResult"].Trim();
            string strQuestionType = Request["SkillCN"];
            string strIsMultiple = Request["IsMultiple"];

            string sql = "";
            if (qID == "0")
            {
                //0增加
                sql = string.Format("INSERT INTO ExamQuestion(Question,QuestionType,QuestionResult,IsMultiple,Score,IsActive) VALUES(N'{0}',N'{1}',N'{2}',N'{3}',{4},1);select @@IDENTITY as ID;", strQuestion, strQuestionType, strQuestionResult, strIsMultiple, strScore);

                int examQuestionID = Convert.ToInt32(DAL.SqlHelper.ExecuteScalar(sql));
                if (examQuestionID > 0)
                {
                    List<string> lst = new List<string>();
                    //增加
                    for (int i = 7; i < Request.Form.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Request.Form[i]))
                            lst.Add(string.Format("INSERT INTO ExamQuestionOptions(QuestionID,QuestionOption) VALUES({0},N'{1}')", examQuestionID, Request.Form[i]));
                    }

                    int iRows = DAL.SqlHelper.ExecuteNonQueryTran(lst);
                    if (iRows == lst.Count)
                        Common.PageHelper.Alert(this, "考题增加成功!");

                }
                else
                    Common.PageHelper.Alert(this, "增加考题失败!");
            }
            else
            {
                //修改
                sql = string.Format("UPDATE ExamQuestion SET  Question=N'{0}',QuestionType=N'{1}',QuestionResult=N'{2}',IsMultiple=N'{3}',Score={4} WHERE ID ={5}", strQuestion, strQuestionType, strQuestionResult, strIsMultiple, strScore, qID);
                int rowCount = Convert.ToInt32(DAL.SqlHelper.ExecuteNonQuery(sql));

                if (rowCount > 0)
                {
                    //删除原来的答案，添加新的答案
                    List<string> lst = new List<string>();
                    lst.Add("DELETE ExamQuestionOptions WHERE QuestionID = " + qID);

                    for (int i = 7; i < Request.Form.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Request.Form[i]))
                            lst.Add(string.Format("INSERT INTO ExamQuestionOptions(QuestionID,QuestionOption) VALUES({0},N'{1}')", qID, Request.Form[i]));
                    }
                    try
                    {

                        DAL.SqlHelper.ExecuteNonQueryTran(lst);
                        Common.PageHelper.Alert(this, "修改成功!");
                    }
                    catch
                    {
                        Common.PageHelper.Alert(this, "修改失败!");
                    }

                }
                else
                    Common.PageHelper.Alert(this, "修改考题失败!");

            }
            BindData(strQuestionType, true, true);
        }

        public string ConvertString(object obj)
        {
            if (DBNull.Value == obj)
                return "";
            if (!string.IsNullOrEmpty(obj.ToString()))
            {
                if (Convert.ToBoolean(obj))
                    return "是";
                else
                    return "否";
            }
            else
                return "";
        }

        protected void btnSearchAll_Click(object sender, EventArgs e)
        {
            BindData("", true, false);
        }
    }
}