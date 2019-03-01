using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Certification.Handler
{
    public class GetChartsData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["requestType"]))
            {
                string requestStrType = context.Request["requestType"];
                string sql = "";

                //所有记录SkillCertification,OperationRecord
                if (requestStrType == "AllData")
                {
                    //sql = @"SELECT SN.SkillSn AS name ,COUNT(OPR.ID) AS value FROM SkillCertification AS SN LEFT JOIN OperationRecord AS OPR ON SN.SkillSn = OPR.SkillSn WHERE SN.IsActive = 1 GROUP BY SN.SkillSn HAVING COUNT(OPR.ID) > 0";
                    sql = @"SELECT SkillSN AS name ,COUNT(SkillSN) AS value FROM Certification GROUP BY SkillSN";
                }
                else if (requestStrType == "GetWorkcell")
                {
                    //所有Workcell部门证书分部 Certification
                    sql = "SELECT COUNT(Workcell) AS value,Workcell AS name FROM Certification GROUP BY Workcell";
                }
                else if (requestStrType == "GetCategory")
                {
                    sql = "SELECT COUNT(Category) AS value,Category AS name FROM Certification GROUP BY Category";
                }

                DataTable dt = DAL.SqlHelper.GetDataTableOfRecord(sql);

                if (dt != null && dt.Rows.Count > 0)
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
                
                
                context.Response.End();
            }
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