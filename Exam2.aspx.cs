﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class Exam2 : BasePage
    {
        public int countActiveSkillSN = 0;
        public int countDelSkillSN = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["iptHandle"]))
                {
                    RequestHandle(Request["iptHandle"]);
                    return;
                }
                Bind("");
            }
        }

        void Bind(string strWhere)
        {

            string sql = @"SELECT S.SkillSn,S.SkillDescription,S.CertificationCreateDate, COUNT(Q.Score) AS QCOUNT,ISNULL(SUM(Q.Score),0) QSUM FROM SkillCertification AS S LEFT JOIN (SELECT * FROM ExamQuestion WHERE IsActive = 1) AS Q ON S.SkillSn = Q.QuestionType WHERE S.IsActive = 1 GROUP BY S.SkillSn,SkillDescription,S.CertificationCreateDate  ORDER BY QCOUNT DESC";
            if (!string.IsNullOrEmpty(strWhere))
                sql = string.Format("SELECT S.SkillSn,S.SkillDescription,S.CertificationCreateDate, COUNT(Q.Score) AS QCOUNT,ISNULL(SUM(Q.Score),0) QSUM  FROM SkillCertification AS S LEFT JOIN (SELECT * FROM ExamQuestion WHERE IsActive = 1) AS Q ON S.SkillSn = Q.QuestionType WHERE S.IsActive = 1 AND  SkillSn = N'{0}' GROUP BY S.SkillSn,SkillDescription,S.CertificationCreateDate ORDER BY QCOUNT DESC", strWhere);

            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            rptSkill.DataSource = dt;
            rptSkill.DataBind();

            sql = string.Format("SELECT  TOP 20 A.SkillSn,SUM(A.Score) AS SUMSCORE,B.ExamDate FROM ExamUserResult AS A LEFT JOIN(SELECT ID, CAST(ExamDate as date) AS ExamDate FROM ExamUserResult)AS B ON A.ID = B.ID WHERE NTID = N'{0}' AND A.ExamResult = A.UserResult AND DATEDIFF(DAY, A.ExamDate, B.ExamDate) = 0 GROUP BY A.SkillSn,B.ExamDate  ORDER BY B.ExamDate DESC ", user.UID);
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            rptExamHistory.DataSource = dt;
            rptExamHistory.DataBind();


            sql = "SELECT COUNT(0) AS COUNTS FROM SkillCertification WHERE IsActive = 1";
            countActiveSkillSN = Convert.ToInt32(DAL.SqlHelper.ExecuteScalar(sql));

            sql = "SELECT COUNT(0) AS COUNTS FROM SkillCertification WHERE IsActive = 0";
            countDelSkillSN = Convert.ToInt32(DAL.SqlHelper.ExecuteScalar(sql));




        }

        void RequestHandle(string iptHandle)
        {
            int rows;
            string sql, SkillSn, oldSkillSn, SkillDescription;
            //0添加
            if (iptHandle.Trim() == "0")
            {
                SkillSn = Request["SKillSN"];
                SkillDescription = Request["SkillDescription"];
                sql = string.Format("INSERT INTO [dbo].[SkillCertification]([SkillSn],[SkillDescription],[IsActive],CertificationCreateDate)VALUES(N'{0}',N'{1}',1,N'{2}')", SkillSn, SkillDescription, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                rows = DAL.SqlHelper.ExecuteNonQuery(sql);
                if (rows > 0)
                    Response.Write("1");
                else
                    Response.Write("0");

                //插入删除记录
                InsertLog("Insert", "SkillCertification", SkillSn);

                Response.End();

            }
            else if (iptHandle.Trim() == "1")
            {//1修改
                SkillSn = Request["SKillSN"].Trim();
                oldSkillSn = Request["OldSkillSN"].Trim();

                SkillDescription = Request["SkillDescription"];
                List<string> lst = new List<string>();

                //修改证书
                sql = string.Format("UPDATE [dbo].[SkillCertification] SET [SkillSn] = N'{0}',[SkillDescription] = N'{1}' WHERE SkillSn = N'{2}'", SkillSn, SkillDescription, oldSkillSn);
                lst.Add(sql);
                //修改题库的证书名
                sql = string.Format("UPDATE ExamQuestion SET QuestionType = N'{0}' WHERE QuestionType = N'{1}'", SkillSn, oldSkillSn);
                lst.Add(sql);
                //修改Certification表
                sql = string.Format("UPDATE Certification SET SkillSN = N'{0}' WHERE SkillSN = N'{1}' ", SkillSn, oldSkillSn);
                lst.Add(sql);

                rows = DAL.SqlHelper.ExecuteNonQueryTran(lst);
                if (rows > 0)
                    Response.Write(1);
                else
                    Response.Write(0);

                //插入修改记录
                InsertLog("Update", "SkillCertification,ExamQuestion,Certification", SkillSn);

                Response.End();
            }
            else if (iptHandle.Trim() == "2")
            {//2删除
                List<string> lst = new List<string>();
                SkillSn = Request["SKillSN"].Trim(); ;
                sql = string.Format("UPDATE SkillCertification SET IsActive = 0 WHERE SkillSn = N'{0}' ", SkillSn);
                lst.Add(sql);
                sql = string.Format("UPDATE ExamQuestion SET IsActive = 0 WHERE QuestionType = N'{0}'", SkillSn);
                lst.Add(sql);

                rows = DAL.SqlHelper.ExecuteNonQueryTran(lst);
                if (rows > 0)
                    Response.Write("1");
                else
                    Response.Write("0");

                //插入删除记录
                InsertLog("Delete", "SkillCertification,ExamQuestion", SkillSn);

                Response.End();
            }
            else if (iptHandle.Trim() == "3")
            {
                SkillSn = Request["SKillSN"].Trim();
                sql = string.Format("SELECT * FROM SkillCertification WHERE SkillSn = N'{0}'", SkillSn);
                DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
                Response.End();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //
            string skillSN = txtSkillSN.Text.Trim();
            Bind(skillSN);

        }

        void InsertLog(string handle, string table, string item)
        {
            try
            {
                user = BLL.OrgMgr.GetCurrentAdUser();
                string sql = string.Format("INSERT INTO HandleHistory(NTID,DisplayName,HandleType,HandleTable,HandleTableItem,HandleDateTime) VALUES(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}')", user.UID, user.DisplayName, handle, table, item, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                DAL.SqlHelper.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
            }

        }
    }
}