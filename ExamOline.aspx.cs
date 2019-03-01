using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class ExamOline : System.Web.UI.Page
    {
        public IList<ExamQuestion> questionList;
        public IList<ExamQuestionOptions> dtQuestionOption;

        public int totalScore;

        Model.UserInfo user;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                user = BLL.OrgMgr.GetCurrentAdUser();


                if (!string.IsNullOrEmpty(Request.QueryString["Action"]))
                {
                    if (VaildateExamTime(HttpUtility.UrlDecode(Request["Action"]).Trim()))
                    {
                        Response.Write("<script>alert('再次考试需要满24小时！');window.history.go(-1);</script>");
                        return;
                    }
                    //保存考试信息
                    SaveExamResult();
                    return;
                }

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    if (VaildateExamTime(HttpUtility.UrlDecode(Request["id"]).Trim()))
                        Response.Write("<script>alert('再次考试需要满24小时！');window.history.go(-1);</script>");
                    //取题
                    GetQuestionList();
                    Session["examTime"] = DateTime.Now.AddMinutes(40).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        /// <summary>
        /// 验证同一人同一个证书，大于24小时后才能再次考试。
        /// </summary>
        /// <returns></returns>
        bool VaildateExamTime(string strSkillSN)
        {
            //string strQuestionType = HttpUtility.UrlDecode(Request["id"]).Trim();

            string sql = string.Format("SELECT NTID,SkillSn,ExamDate FROM ExamUserResult WHERE DATEDIFF(HOUR,EXAMDATE,'{0}') < 24 AND NTID = N'{1}' AND SkillSn = N'{2}' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.UID, strSkillSN);
            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);

            return dt != null && dt.Rows.Count > 0 ? true : false;
        }

        void SaveExamResult()
        {
            string[] name = Request.Form.AllKeys;
            string sql = "";
            List<string> lst = new List<string>();

            for (int i = 0; i < name.Length; i++)
            {
                string strResult = "";
                string[] value = Request.Form.GetValues(i);
                for (int j = 0; j < value.Length; j++)
                {
                    strResult = strResult + (value[j] + "※€");
                }
                if (!string.IsNullOrEmpty(strResult))
                    strResult = strResult.Substring(0, strResult.Length - 2);
                sql = string.Format("INSERT INTO ExamUserResult(NTID,QuestionID,ExamQuestion,SkillSn,ExamResult,IsMultiple,Score,UserResult,ExamDate) SELECT N'{0}',{1},Question,QuestionType,QuestionResult,IsMultiple,Score,N'{2}',GETDATE() FROM ExamQuestion AS EQ WHERE EQ.ID = N'{3}'", user.UID, name[i], strResult, name[i]);

                lst.Add(sql);
            }



            int irows = DAL.SqlHelper.ExecuteNonQueryTran(lst);
            if (irows > 0)
            {
                string skillSN = HttpUtility.UrlDecode(Request["Action"]).Trim();
                try
                {
                    //添加考试大于等于80分的记录插入到实操模块中.
                    sql = string.Format("INSERT INTO [dbo].[OperationRecord]([NTID],[DisplayName],[Department],[SkillSn],[ExamTotalScore],[CreateDate],[IsOperationApproved]) SELECT NTID,N'{0}',N'{1}' ,SkillSn, SUM(Score) AS SUMSCORE, GETDATE() AS CreateDate,'0' FROM ExamUserResult WHERE ExamResult = UserResult  AND NTID = N'{2}'  AND SkillSn = N'{3}' AND DATEDIFF(DAY,GETDATE(),ExamDate) = 0  GROUP BY SkillSn, NTID, CONVERT(varchar(10), ExamDate, 120)  HAVING SUM(Score) >= 80 ", user.DisplayName, user.Department, user.UID, skillSN);

                    DAL.SqlHelper.ExecuteNonQuery(sql);

                    sql = string.Format("SELECT SUM(Score) AS SUMSCORE FROM ExamUserResult WHERE NTID = N'{0}' AND SkillSn = N'{1}' AND DATEDIFF(HOUR,ExamDate,GETDATE())<24", user.UID, skillSN);
                    DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);

                    Response.Write("<script>alert('提交成功! 您此次考试的[" + skillSN + "] 分数为:[" + dt.Rows[0]["SUMSCORE"].ToString() + "]分');location.href = 'Exam.aspx';</script>");
                }
                catch
                {
                    Response.Write("<script>alert('出错了，请换个时间段在考试。。');location.href = 'Exam.aspx';</script>");
                }

            }
            else
                Response.Write("<script>alert('出错了，请换个时间段在考试。');location.href = 'Exam.aspx';</script>");
            //Response.End();
        }

        void GetQuestionList()
        {
            string strQuestionType = HttpUtility.UrlDecode(Request["id"]).Trim();

            //根据请求的证书获取该证书下面的问题ID跟答案，避免repeater里面重复去数据库中查询，节省数据库的访问。
            string sql = string.Format("SELECT O.* FROM ExamQuestionOptions AS O LEFT JOIN ExamQuestion AS Q ON O.QuestionID = Q.ID WHERE Q.QuestionType = N'{0}'", strQuestionType);
            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            dtQuestionOption = Common.ModelConvertHelper<ExamQuestionOptions>.ConvertToModel(dt);

            //根据请求获取所有请求中的试题
            sql = string.Format("SELECT * FROM ExamQuestion WHERE IsActive = 1 AND QuestionType = N'{0}'  ORDER BY NEWID() ", strQuestionType);
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            questionList = Common.ModelConvertHelper<ExamQuestion>.ConvertToModel(dt);

            //总分数
            totalScore = questionList.Sum(s => s.Score);
            rptExamQuestion.DataSource = dt;
            rptExamQuestion.DataBind();


            #region 需求变化。
            //大于100分未完成随机取题功能。。。
            //int sumScore = questionList.Sum(s => s.Score);
            //if (sumScore == 100){rptExamQuestion.DataSource = dt;}
            //else
            //{
            //    //随机取题满100分

            //    List<ExamQuestion> newExamQuestion = new List<ExamQuestion>();
            //    minScore = newExamQuestion.Min(s => s.Score);
            //    maxScore = newExamQuestion.Max(s => s.Score);

            //    newExamQuestion = GenrateQuestion(newExamQuestion);
            //    rptExamQuestion.DataSource = newExamQuestion;
            //}
            //rptExamQuestion.DataBind();

            #endregion

        }


        //Random random = new Random();
        //int minScore = 2;
        //int maxScore = 10;
        //int tempScore = 0;

        //List<ExamQuestion> GenrateQuestion(List<ExamQuestion> newExamQuestion)
        //{
        //    int sumScore = newExamQuestion.Sum(s => s.Score);
        //    if (sumScore == 100)
        //        return newExamQuestion;
        //    else
        //    {
        //        //计算总分在随机取题。

        //        int i = random.Next(0, questionList.Count);
        //        tempScore = sumScore + questionList[i].Score;
        //        int s = 100 - tempScore;


        //        //&& (100 - tempScore) > minScore

        //        if (tempScore <= 100 )
        //        {
        //            newExamQuestion.Add(questionList[i]);
        //            questionList.RemoveAt(i);
        //        }
        //        return GenrateQuestion(newExamQuestion);

        //    }
        //}


        protected void rptExamQuestion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("rptQuestionOption") as Repeater;//找到里层的repeater对象
                DataRowView rowv = (DataRowView)e.Item.DataItem;//找到分类Repeater关联的数据项 
                int questionID = Convert.ToInt32(rowv["ID"]); //获取填充子类的id 

                rep.DataSource = dtQuestionOption.Where(s => s.QuestionID == questionID);
                rep.DataBind();
            }
        }

        public static string ConvertIsMultiple(object objIsMultiple, object questionID, object questionOption)
        {
            if (Convert.ToBoolean(objIsMultiple))
            {
                return "<input type='checkbox' name='" + questionID + "' value='" + questionOption + "' title='" + questionOption + "' >";
            }
            else
            {
                return "<input type='radio' name='" + questionID + "' value='" + questionOption + "' title='" + questionOption + "' >";
            }
        }

        public static string ConvertQuestionType(object objIsMultiple, object obj)
        {
            if (Convert.ToBoolean(objIsMultiple))
            {
                if (obj.ToString().Contains("※€"))
                    return "选择题(多选)";
                else
                    return "选择题(单选)";
            }
            else
                return "判断题";

        }


    }

    public class ExamQuestion
    {
        public int ID { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string QuestionResult { get; set; }
        public int IsMultiple { get; set; }
        public int Score { get; set; }
        public int IsActive { get; set; }

    }

    public class ExamQuestionOptions
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionOption { get; set; }
    }

    public class ExamUserResult
    {
        public int ID { get; set; }
        public string NTID { get; set; }
        public int QuestionID { get; set; }
        public string ExamQuestion { get; set; }
        public string SkillSn { get; set; }
        public string ExamResult { get; set; }
        public int IsMultiple { get; set; }
        public int Score { get; set; }
        public string UserResult { get; set; }
        public DateTime ExamDate { get; set; }

    }


}