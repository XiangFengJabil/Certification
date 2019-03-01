using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Certification.DAL
{
    public class Request
    {
        #region below function don't change

        public DataTable GetRequestsByUID(string uid)
        {
            string cmdText = GetRequestCmdText() + " where r.uid=@uid order by r.reqID desc";
            SqlParameter param = new SqlParameter("@uid", uid);
            return SqlHelper.GetDataTableOfRecord(cmdText, param);
        }

        public DataTable GetAllRequests()
        {
            string cmdText = GetRequestCmdText() + " order by item asc";
            return SqlHelper.GetDataTableOfRecord(SqlHelper.CONN_STRING_DEFAULT, CommandType.Text, cmdText, null);
        }

        public DataTable GetRequestsByTaskOwner(string uid,string group)
        {
            string cmdText = @"select r.reqID,s.Name as SiteName,f.FormName,r.ExpectDay,b.displayName as Requestor,
                                    (select StatusDesc from RequestStatus where status=fl.status) as [Status],
                                    t.sno 
                                from task t 
                                    inner join flow fl on t.flowId=fl.flowId 
                                    inner join request r on r.reqId=fl.ReqId 
                                    inner join form f on r.FormID=f.FormID 
                                    inner join Site s on r.SiteID=s.SiteID 
                                    left join WinADUser b on r.uid=b.userid 
                                where t.logTime is null and (t.uid=@uid or t.uid=@group)";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@uid", uid),
                new SqlParameter("@group", group),
            };
            return SqlHelper.GetDataTableOfRecord(cmdText, parameters);
        }

        #endregion

        #region To do function

        public Model.RequestInfo GetRequest(int reqId)
        {
            Model.RequestInfo request = null;
            string cmdText = @"select r.reqID,r.SiteID,r.UID,r.CreateTime,
                                w.displayName as UserName,
                                w.dept as UserDept,
                                w.mail as UserMail,
                                w.site as UserSite,
                                r.FormID,f.FormName,r.ExpectDay,r.FlowType,
                                r.Detail,r.FormFile,r.AttFile,
                                r.Remark  
                                from request r 
                                    inner join form f on r.FormID=f.FormID 
	                                left join WinADUser w on r.uid=w.userid
                                where r.reqID=@RequestId";
            SqlParameter param = new SqlParameter("@RequestId", reqId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(cmdText, param))
            {
                if (reader.Read())
                {
                    request = ReaderToRequestEntity(reader);
                }
            }
            return request;
        }

        private string GetRequestCmdText()
        {
            //string cmdText = @"select r.reqID,s.Name as SiteName,f.FormName,r.ExpectDay,b.displayName as Requestor,
            //                    (select StatusDesc from RequestStatus where status=fl.status) as [Status]
            //                from request r 
            //                 inner join form f on r.FormID=f.FormID 
            //                    inner join Site s on r.SiteID=s.SiteID 
            //                    inner join flow fl on r.reqID=fl.ReqId and fl.parentId=0 
            //                    left join WinADUser b on r.uid=b.userid ";
            //return cmdText;
            string cmdText = @"select * from Certification ";
            return cmdText;
        }

//        public bool Update(Model.RequestInfo request)
//        {
//            string cmdText = @"update Request Set [Title]=@Title,[UID]=@UID,
//                                    [content]=@Detail,[deptId]=@DepartmentId,[workcellid]=@WorkcellId,
//                                    [FileName]=@FileName,[QuotFile]=@QuotFile,amt=@amt where reqId=@RequestId";
//            SqlParameter[] parameters = new SqlParameter[]{
//                        new SqlParameter("@RequestId",request.ReqID),
//                        new SqlParameter("@Title",request.Title),
//                        new SqlParameter("@UID",request.User.UID),
//                        new SqlParameter("@Detail",request.Detail),
//                        new SqlParameter("@FileName",request.FileName),
//                        new SqlParameter("@DepartmentId",request.DeptId),
//                        new SqlParameter("@WorkcellId",request.WcId),
//                        new SqlParameter("@QuotFile",request.QuotFile),
//                        new SqlParameter("@amt",request.Amt)
//                    };
//            if (parameters[8].Value == null)
//            {
//                parameters[8].Value = DBNull.Value;
//            }
//            if (parameters[9].Value == null)
//            {
//                parameters[9].Value = DBNull.Value;
//            }
//            return SqlHelper.ExecuteNonQuery(cmdText, parameters) > 0;
//        }

        private Model.RequestInfo ReaderToRequestEntity(SqlDataReader reader)
        {
            Model.RequestInfo request = new Model.RequestInfo();
            request.ReqID = int.Parse(reader["reqId"].ToString());
            request.SiteID = int.Parse(reader["SiteID"].ToString());
            request.User = new Model.UserInfo
            {
                UID = reader["uid"].ToString().ToLower(),
                DisplayName = reader["UserName"].ToString(),
                Department = reader["UserDept"].ToString(),
                Email = reader["UserMail"].ToString()
            };
            request.CreateTime = DateTime.Parse(reader["CreateTime"].ToString());
            request.ExpectDay = DateTime.Parse(reader["ExpectDay"].ToString());
            request.FormID = int.Parse(reader["FormID"].ToString());
            request.FlowType = int.Parse(reader["FlowType"].ToString());
            request.Detail = reader["Detail"].ToString();
            request.FormName = reader["FormName"].ToString();

            if (reader["FormFile"] != DBNull.Value)
                request.FormFile = reader["FormFile"].ToString();
            if (reader["AttFile"] != DBNull.Value)
                request.AttFile = reader["AttFile"].ToString();
            request.Remark = reader["Remark"].ToString();

            return request;
        }

        public int Create(Model.RequestInfo requestInfo)
        {
            string cmdText = @"insert into Request (SiteID,UID,CreateTime,FormID,ExpectDay,FlowType,Detail,
                    FormFile,AttFile,Remark) values (@SiteID,@UID,@CreateTime,@FormID,@ExpectDay,@FlowType,@Detail,
                    @FormFile,@AttFile,@Remark); select @@identity;";
            SqlParameter[] parameters = {
                new SqlParameter("@SiteID",requestInfo.SiteID),
                new SqlParameter("@UID",requestInfo.User.UID),
                new SqlParameter("@CreateTime",requestInfo.CreateTime),
                new SqlParameter("@FormID",requestInfo.FormID),
                new SqlParameter("@ExpectDay",requestInfo.ExpectDay),
                new SqlParameter("@FlowType",requestInfo.FlowType),
                new SqlParameter("@Detail",requestInfo.Detail),
                new SqlParameter("@FormFile",requestInfo.FormFile),
                new SqlParameter("@AttFile",requestInfo.AttFile??""),
                new SqlParameter("@Remark",requestInfo.Remark)
            };
            object obj = SqlHelper.ExecuteScalar(cmdText, parameters);
            return obj == null ? -1 : Convert.ToInt32(obj);
        }

        #endregion

        public DataTable GetAllRequestsByFilter(string filter)
        {
            string cmdText = GetRequestCmdText() + " where 1=1 ";
            if (!string.IsNullOrEmpty(filter))
                cmdText += filter;

            cmdText += " order by item asc ";
            return SqlHelper.GetDataTableOfRecord(cmdText);
        }
    }
}