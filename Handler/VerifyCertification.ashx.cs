﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Certification.Handler
{
    /// <summary>
    /// Summary description for VerifyCertification
    /// </summary>
    public class VerifyCertification : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string NTID = context.Request.QueryString["NTID"];
            string skillSN = context.Request.QueryString["skillSN"];
            bool returnValue = false;

            context.Response.Clear();
            try
            {
                //判断证书是否存在
                returnValue = BLL.OrgMgr.ExistsCertification(NTID, skillSN);
                if (!returnValue)
                {
                    context.Response.Write(StrJsonMsg(false, " 没有对应资质！", "请求成功"));
                }
                else
                {
                    returnValue = BLL.OrgMgr.VerifyCertification(NTID, skillSN);
                    if (!returnValue)
                    {//存在已过期
                        context.Response.Write(StrJsonMsg(false, "当前岗位资质已过期！", "请求成功"));
                    }
                    else
                    {
                        context.Response.Write(StrJsonMsg(true, "请求成功！", "Success"));
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(StrJsonMsg(false, "请求出错： " + ex.ToString(), "请求失败"));
            }

        }

        string StrJsonMsg(bool flag, string msg, string status)
        {
            string json = "";
            json += "{\"Result\":\"" +
                         flag + "\",\"Msg\":\"" + msg + "\"" +
                         ",\"RequestStatus\":\"" + status + "\"\"";
            if (json.IndexOf(',') >= 0)
            {
                json = json.Remove(json.Length - 1, 1);
            }
            json += "}";
            return json;

            //---------------------------------------------------
            //string json = "";
            //returnValue = BLL.OrgMgr.VerifyCertification(NTID, skillSN);
            //json += "{\"HasCertification\":\"" +
            //             returnValue + "\",\"Msg\":\"请求成功\"\"";
            //if (json.IndexOf(',') >= 0)
            //{
            //    json = json.Remove(json.Length - 1, 1);
            //}
            //json += "}";
            //context.Response.Clear();
            //context.Response.Write(json);
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