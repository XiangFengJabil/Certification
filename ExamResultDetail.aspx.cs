using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class ExamResultDetail : System.Web.UI.Page
    {
        public Model.UserInfo user;
        public IList<ExamQuestion> questionList;
        public IList<ExamQuestionOptions> dtQuestionOption;
        public IList<ExamUserResult> userResult;
        public int totalScore;
        public DateTime dateTm;
        public string skillsn;
        public int userScore = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                user = BLL.OrgMgr.GetCurrentAdUser();
                Bind();
            }
        }

        void Bind()
        {
            dateTm = Convert.ToDateTime(Request["ExamDate"]);
            skillsn = Request["skillsn"];

            //获取用户做题
            string sql = string.Format("SELECT * FROM ExamUserResult WHERE SkillSn = N'{0}' AND NTID = N'{1}' AND DATEDIFF(DAY,N'{2}',ExamDate) = 0", skillsn, user.UID, dateTm);
            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            userResult = Common.ModelConvertHelper<ExamUserResult>.ConvertToModel(dt);
            userScore = userResult.Where(s => s.UserResult == s.ExamResult).Sum(s => s.Score);


            //根据请求的证书获取该证书下面的问题ID跟答案，避免repeater里面重复去数据库中查询，节省数据库的访问。
            sql = string.Format("SELECT O.* FROM ExamQuestionOptions AS O LEFT JOIN ExamQuestion AS Q ON O.QuestionID = Q.ID WHERE Q.QuestionType = N'{0}'", skillsn);
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            dtQuestionOption = Common.ModelConvertHelper<ExamQuestionOptions>.ConvertToModel(dt);


            //根据请求获取所有请求中的试题
            sql = string.Format("SELECT * FROM ExamQuestion WHERE IsActive = 1 AND QuestionType = N'{0}'", skillsn);
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            questionList = Common.ModelConvertHelper<ExamQuestion>.ConvertToModel(dt);

            //总分数
            totalScore = questionList.Sum(s => s.Score);
            rptExamQuestion.DataSource = dt;
            rptExamQuestion.DataBind();

        }


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

        public static string ConvertIsMultiple(object objIsMultiple, object questionID, object questionOption, string objUserResult)
        {
            if (Convert.ToBoolean(objIsMultiple))
            {
                if (objUserResult.Contains(questionOption.ToString()))
                    return "<input type='checkbox' name='" + questionID + "' value='" + questionOption + "' checked='' readonly='' title = '" + questionOption + "' > ";
                else
                    return "<input type='checkbox' name='" + questionID + "' value='" + questionOption + "' disabled='' title = '" + questionOption + "' > ";
            }
            else
            {
                if (objUserResult.Contains(questionOption.ToString()))
                    return "<input type='radio' name='" + questionID + "' value='" + questionOption + "' checked='' readonly='' title='" + questionOption + "' >";
                else
                    return "<input type='radio' name='" + questionID + "' value='" + questionOption + "' disabled=''  title='" + questionOption + "' >";
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

        public string GetUserResult(object questionID)
        {
            if (userResult.Where(s => s.NTID == user.UID && s.QuestionID.Equals(questionID) && (s.ExamDate - dateTm).Days == 0).ToList<ExamUserResult>().Count == 0)
            {
                return "";
            }
            else
                return userResult.Where(s => s.NTID == user.UID && s.QuestionID.Equals(questionID) && (s.ExamDate - dateTm).Days == 0).ToList<ExamUserResult>()[0].UserResult;
        }

        /// <summary>
        /// 判断题是否做正确
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns></returns>
        public string GetExamStandardResult(object questionID)
        {
            string qResult = questionList.Where(s => s.ID == Convert.ToInt32(questionID)).ToList<ExamQuestion>()[0].QuestionResult;

            //该题可能没有做
            IList<ExamUserResult> lu = userResult.Where(s => s.QuestionID == Convert.ToInt32(questionID)).ToList<ExamUserResult>();
            string uResult = "";
            if (lu != null & lu.Count > 0)
                uResult = userResult.Where(s => s.QuestionID == Convert.ToInt32(questionID)).ToList<ExamUserResult>()[0].UserResult;

            if (qResult.Equals(uResult))
                return "<span style='line-height: 28px; margin-right:20px;float:right; font-size:large;font-weight:700; color: #01AAED;'>正确</span>";
            else
                return "<span style='line-height: 28px; margin-right:20px;float:right; font-size:large;font-weight:700; color: #FF5722;'>错误</span>";
        }

        public string GetQuestionResult(object objQuestionResult)
        {
            string str = objQuestionResult.ToString();
            if (str.Contains("※€"))
            {
                string[] array = Regex.Split(str, "※€");
                str = "";
                for (int i = 0; i < array.Length; i++)
                {
                    str += (i + 1) + "  :" + array[i] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }

                return str;
            }
            else
                return str;
        }

        public string GetUserScore()
        {
            if (userScore >= 80)
                return "<label style='color: green;' >" + userScore + "</label>";
            else
                return "<label style='color: red;' >" + userScore + "</label>";
        }

    }
}