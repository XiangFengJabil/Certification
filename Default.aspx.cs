﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Certification
{
    public partial class Default : BasePage
    {
        //public int userRole = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    UserRoleControl();
            //}
        }

        //void UserRoleControl()
        //{
        //    Model.UserInfo user = BLL.OrgMgr.GetCurrentAdUser();

        //    string sqlUser = "SELECT * FROM [Admin] WHERE NTID = '" + user.UID + "'";
        //    DataTable dtUser = DAL.SqlHelper.GetDataTableOfRecord(sqlUser);
        //    //user.UID == "1518819" || user.UID == "1258504" || user.UID == "xuh4"
        //    if (dtUser != null && dtUser.Rows.Count > 0)
        //    {
        //        userRole = dtUser.Rows.Count;
        //    }
        //}

    }
}