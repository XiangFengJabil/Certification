using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class Operation : System.Web.UI.Page
    {
        public IList<OperationItemInfo> lstOperationItem;
        public IList<OperationStandardInfo> lstOperationStandard;
        public IList<OperationUserScoreInfo> lstOperationUserScore;
        public IList<OperationRecordInfo> lstOperationRecordInfo;
        public Model.UserInfo user;
        public int userRole = 0;
        public int recordID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["basis"]))
                    SaveBasis();
                else if (!string.IsNullOrEmpty(Request["operation"]))
                    SaveOperation();
                else
                    Bind();
            }
        }


        void Bind()
        {
            recordID = Convert.ToInt32(Request["id"]);

            string sql = "SELECT * FROM OperationStandard";
            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            lstOperationStandard = Common.ModelConvertHelper<OperationStandardInfo>.ConvertToModel(dt);

            sql = string.Format("SELECT * FROM OperationUserScore WHERE OperationRecordID = {0}", recordID);
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            lstOperationUserScore = Common.ModelConvertHelper<OperationUserScoreInfo>.ConvertToModel(dt);

            sql = string.Format("SELECT * FROM OperationRecord WHERE ID = {0}", recordID);
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            lstOperationRecordInfo = Common.ModelConvertHelper<OperationRecordInfo>.ConvertToModel(dt);

            sql = "SELECT * FROM OperationItem";
            dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            lstOperationItem = Common.ModelConvertHelper<OperationItemInfo>.ConvertToModel(dt);
            rptOperation.DataSource = lstOperationItem.Where(s => s.OperationType == "实际操作").ToList();
            rptOperation.DataBind();

            rptLean.DataSource = lstOperationItem.Where(s => s.OperationType == "精益分").ToList();
            rptLean.DataBind();

            rptOther.DataSource = lstOperationItem.Where(s => s.OperationType == "其它").ToList();
            rptOther.DataBind();



            UserRoleControl();

        }


        /// <summary>
        /// 保存基础信息
        /// </summary>
        void SaveBasis()
        {
            OperationRecordInfo ori = new OperationRecordInfo();
            ori.ID = Convert.ToInt32(Request["ID"]);
            ori.Customer = Request["Customer"];
            ori.ProductType = Request["ProductType"];
            ori.ProductPnOrSn = Request["ProductPnOrSn"];
            ori.Quantity = Request["Quantity"];
            ori.InspectorName = Request["InspectorName"];
            ori.AssessmentDate = Convert.ToDateTime(Request["AssessmentDate"]);

            try
            {
                string sql = string.Format("UPDATE OperationRecord SET Customer = N'{0}',ProductType = N'{1}',ProductPnOrSn = N'{2}', Quantity = N'{3}', InspectorName = N'{4}', AssessmentDate = N'{5}'  WHERE ID = {6} ", ori.ID, ori.ProductType, ori.ProductPnOrSn, ori.Quantity, ori.InspectorName, ori.AssessmentDate, ori.ID);
                int i = DAL.SqlHelper.ExecuteNonQuery(sql);
                Response.Write(i);

            }
            catch
            {
                Response.Write("-1");
            }
            Response.End();

        }

        /// <summary>
        /// 保存实操信息
        /// </summary>
        void SaveOperation()
        {
            List<string> lst = new List<string>();
            recordID = Convert.ToInt32(Request["recordID"]);

            int startIndex = 2;
            int countNum = (Request.Form.Count - startIndex) / 2;
            //保存实操每项分
            for (int i = 0; i < countNum; i++)
            {
                int itemID = Convert.ToInt32(Request.Form[i + startIndex]);
                decimal score = string.IsNullOrEmpty(Request.Form[startIndex + i + countNum]) ? 0 : Convert.ToDecimal(Request.Form[startIndex + i + countNum]);
                lst.Add(string.Format("INSERT INTO OperationUserScore(OperationRecordID,OperationItemID,OperationItem,OperationItemScore,OperationType,OperationUserScore) SELECT {0},{1},OperationItem,OperationScore,OperationType,{2} FROM OperationItem WHERE ID = {3}", recordID, itemID, score, itemID));
            }

            //保存实操每项总分
            lst.Add(string.Format("UPDATE R SET R.IsOperationApproved = 1, R.OperationTotalScore = U.实际操作,R.LeanTotalScore = U.精益分,R.OtherTotalScore = U.其它 FROM OperationRecord AS R LEFT JOIN (SELECT * FROM( SELECT OperationRecordID, OperationType, SUM(OperationUserScore) AS SUMSCORE FROM OperationUserScore GROUP BY OperationType, OperationRecordID) AS T PIVOT(SUM(SUMSCORE) FOR OperationType in ([实际操作],[精益分],[其它])) AS S ) AS U ON R.ID = U.OperationRecordID WHERE R.ID = {0}", recordID));

            //更新总得分
            //总分 = 理论知识得分 X 15% + 实际操作得分 X 50% + 精盗分 X 20% + 其它分 X 15% 
            lst.Add(string.Format("UPDATE OperationRecord SET TotalScore = ExamTotalScore * 0.15 + OperationTotalScore * 0.5 + LeanTotalScore * 0.2 + OtherTotalScore * 0.15 WHERE ID = {0}", recordID));

            //插入总分及格信息到审批流程中。
            //lst.Add("");

            int irows = DAL.SqlHelper.ExecuteNonQueryTran(lst);
            //INSERT INTO OperationUserScore(OperationRecordID,OperationItemID,OperationItem,OperationItemScore,OperationType,OperationUserScore) SELECT 1,1,OperationItem,OperationScore,OperationType,10 FROM OperationItem WHERE ID = 1


            if (irows > 0)
                Response.Write(irows);
            else
                Response.Write(-1);
            Response.End();

        }


        /// <summary>
        /// 权限
        /// </summary>
        void UserRoleControl()
        {
            user = BLL.OrgMgr.GetCurrentAdUser();

            string sqlUser = "SELECT * FROM [Admin] WHERE NTID = '" + user.UID + "'";
            DataTable dtUser = DAL.SqlHelper.GetDataTableOfRecord(sqlUser);
            //user.UID == "1518819" || user.UID == "1258504" || user.UID == "xuh4"
            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                userRole = dtUser.Rows.Count;
            }
            else
            {
                userRole = 0;
            }
        }

        public string GetUserScore(object operationItemID)
        {
            if (string.IsNullOrEmpty(operationItemID.ToString()))
                return "";
            else
            {
                int itemID = Convert.ToInt32(operationItemID);
                if (lstOperationUserScore != null && lstOperationUserScore.Count > 0)
                {

                    List<OperationUserScoreInfo> lst = lstOperationUserScore.Where(s => s.OperationRecordID == recordID && s.OperationItemID == itemID).ToList();
                    if (lst != null && lst.Count > 0)
                        return lst[0].OperationUserScore.ToString();
                    else
                        return "";
                }
                else
                    return "";
            }
        }


        protected void RptOperation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("rptOperationStandard") as Repeater;
                OperationItemInfo rowv = (OperationItemInfo)e.Item.DataItem;
                int typeid = Convert.ToInt32(rowv.ID);

                rep.DataSource = lstOperationStandard.Where(s => s.OperationItemID == typeid).ToList();
                rep.DataBind();
            }
        }

    }

    [Serializable]
    public class OperationItemInfo
    {
        private int _id;
        private string _operationitem;
        private decimal _operationscore;
        private string _operationtype;


        public OperationItemInfo()
        {

        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string OperationItem
        {
            get { return _operationitem; }
            set { _operationitem = value; }
        }
        public decimal OperationScore
        {
            get { return _operationscore; }
            set { _operationscore = value; }
        }
        public string OperationType
        {
            get { return _operationtype; }
            set { _operationtype = value; }
        }

    }

    [Serializable]
    public class OperationStandardInfo
    {
        private int _id;
        private int _operationitemid;
        private string _operationstandard;


        public OperationStandardInfo()
        {

        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public int OperationItemID
        {
            get { return _operationitemid; }
            set { _operationitemid = value; }
        }
        public string OperationStandard
        {
            get { return _operationstandard; }
            set { _operationstandard = value; }
        }

    }

    [Serializable]
    public class OperationUserScoreInfo
    {
        private int _id;
        private int _operationrecordid;
        private int _operationitemid;
        private string _operationitem;
        private decimal _operationitemscore;
        private string _operationtype;
        private decimal _operationuserscore;


        public OperationUserScoreInfo()
        {

        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public int OperationRecordID
        {
            get { return _operationrecordid; }
            set { _operationrecordid = value; }
        }
        public int OperationItemID
        {
            get { return _operationitemid; }
            set { _operationitemid = value; }
        }
        public string OperationItem
        {
            get { return _operationitem; }
            set { _operationitem = value; }
        }
        public decimal OperationItemScore
        {
            get { return _operationitemscore; }
            set { _operationitemscore = value; }
        }
        public string OperationType
        {
            get { return _operationtype; }
            set { _operationtype = value; }
        }
        public decimal OperationUserScore
        {
            get { return _operationuserscore; }
            set { _operationuserscore = value; }
        }

    }

    [Serializable]
    public class OperationRecordInfo
    {
        private int _id;
        private string _ntid;
        private string _displayname;
        private string _department;
        private string _skillsn;
        private decimal _examtotalscore;
        private DateTime _createdate;
        private decimal _operationtotalscore;
        private decimal _leantotalscore;
        private decimal _othertotalscore;
        private decimal _totalscore;
        private bool _isoperationapproved;
        private string _customer;
        private string _producttype;
        private string _productpnorsn;
        private string _quantity;
        private string _inspectorname;
        private DateTime _assessmentdate;


        public OperationRecordInfo()
        {

        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string NTID
        {
            get { return _ntid; }
            set { _ntid = value; }
        }
        public string DisplayName
        {
            get { return _displayname; }
            set { _displayname = value; }
        }
        public string Department
        {
            get { return _department; }
            set { _department = value; }
        }
        public string SkillSn
        {
            get { return _skillsn; }
            set { _skillsn = value; }
        }
        public decimal ExamTotalScore
        {
            get { return _examtotalscore; }
            set { _examtotalscore = value; }
        }
        public DateTime CreateDate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        public decimal OperationTotalScore
        {
            get { return _operationtotalscore; }
            set { _operationtotalscore = value; }
        }
        public decimal LeanTotalScore
        {
            get { return _leantotalscore; }
            set { _leantotalscore = value; }
        }
        public decimal OtherTotalScore
        {
            get { return _othertotalscore; }
            set { _othertotalscore = value; }
        }

        public decimal TotalScore
        {
            get { return _totalscore; }
            set { _totalscore = value; }
        }

        public bool IsOperationApproved
        {
            get { return _isoperationapproved; }
            set { _isoperationapproved = value; }
        }
        public string Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }
        public string ProductType
        {
            get { return _producttype; }
            set { _producttype = value; }
        }
        public string ProductPnOrSn
        {
            get { return _productpnorsn; }
            set { _productpnorsn = value; }
        }
        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public string InspectorName
        {
            get { return _inspectorname; }
            set { _inspectorname = value; }
        }
        public DateTime AssessmentDate
        {
            get { return _assessmentdate; }
            set { _assessmentdate = value; }
        }

    }



}