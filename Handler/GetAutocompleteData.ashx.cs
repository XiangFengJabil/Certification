using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Certification.Handler
{
    /// <summary>
    /// GetAutocompleteData 的摘要说明
    /// </summary>
    public class GetAutocompleteData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sql = "";
            if (context.Request["requestType"] == "OperationRecordDisplayName")//Operation Record > Name
                sql = "SELECT DISTINCT DisplayName FROM OperationRecord WHERE DisplayName IS NOT NULL AND DisplayName != ''";
            else if (context.Request["requestType"] == "ExamSkillSN")//Exam > SkillSN
                sql = "SELECT DISTINCT SkillSn FROM SkillCertification WHERE IsActive = 1";

            DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);
            if (dt != null && dt.Rows.Count > 0)
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}