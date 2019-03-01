using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 用户角色userRole=0普通用户,userRole==1是管理员的权限
        /// </summary>
        public int userRole = 0;
        public Model.UserInfo user;

        protected override void OnPreLoad(EventArgs e)
        {
            UserRole();
        }


        public void UserRole()
        {
            user = BLL.OrgMgr.GetCurrentAdUser();

            string sqlUser = "SELECT * FROM [Admin] WHERE NTID = '" + user.UID + "'";
            DataTable dtUser = DAL.SqlHelper.GetDataTableOfRecord(sqlUser);

            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                var role = dtUser.Rows[0]["UserRole"];

                userRole = 1;
            }
        }



        /// <summary>
        /// 弹出页面
        /// </summary>
        /// <param name="url"></param>
        public void OpenUrl(string url)
        {
            string script = string.Format("<script language='JavaScript'>window.open('" + "{0}" + "','" + "','');<", url);
            script += "/";
            script += "script>";

            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), script);
        }

        /// <summary>
        /// 返回上一页
        /// </summary>
        public void JavascriptGoBack()
        {
            string script = "<script>history.go(-1);</script>";
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), script);
        }

        /// <summary>
        /// 执行JavaScript方法
        /// </summary>
        /// <param name="script"></param>
        public void JavascriptFun(string script)
        {
            string AllScript = string.Format("<script>{0}</script>", script);
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), AllScript);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void JavascriptClose()
        {
            string script = "<script>window.opener = null;window.close();</script>";
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), script);
        }

        /// <summary>
        /// 跳转到其他页面
        /// </summary>
        /// <param name="url">页面地址</param>
        public void JavascriptGoUrl(string url)
        {
            string script = string.Format("<script>location = '{0}';</script>", url);
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), script);
        }

        /// <summary>
        /// 显示信息并跳转页面
        /// </summary>
        /// <param name="message"></param>
        /// <param name="url"></param>
        public void ShowMessageAndGoUrl(string message, string url)
        {
            string script = string.Format("<script>alert('{0}');location.href='{1}';</script>", message, url);
            ClientScript.RegisterStartupScript(this.GetType(), System.Guid.NewGuid().ToString(), script);
        }





    }
}