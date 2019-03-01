﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Certification.Model;

namespace Certification.BLL
{
    public class RequestMgr
    {
        private static readonly DAL.Request _request = new DAL.Request();

        #region below function don't change

        public static void CreateRequest(RequestInfo requestInfo)
        {
            try
            {
                int reqId = _request.Create(requestInfo);
                requestInfo.ReqID = reqId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static RequestInfo GetRequest(int reqId)
        {
            // Validate input
            if (reqId < 1)
                return null;

            return _request.GetRequest(reqId);
        }

        public static DataTable GetRequestsByUID(string uid)
        {
            return _request.GetRequestsByUID(uid);
        }

        public static DataTable GetAllRequests()
        {
            return _request.GetAllRequests();
        }

        public static DataTable GetRequestsByTaskOwner(string uid)
        {
            //get all request that pending approve node
            string group = uid;
            List<string> groups = OrgMgr.GetGroups(uid);
            if (groups.Count > 0)
                group = groups[0];
            return _request.GetRequestsByTaskOwner(uid, group);
        }

        public static bool IsRequestOwner(string userId, RequestInfo reqInfo)
        {
            return string.Compare(reqInfo.User.UID, userId, true) == 0;
        }

        #endregion

        //public static DataTable GetAllRequestsByFilter(string item, string text, string date, string startDate, string endDate)
        public static DataTable GetAllRequestsByFilter(string item, string text)
        {
            string filter = string.Empty;
            if (item == "-1")
            {

            }
            else if (item == "0")
            {
                filter = filter + " and name = N'" + text + "'";
            }
            else if (item == "1")
            {
                filter = filter + " and workcell = N'" + text + "'";
            }
            else if (item == "2")
            {
                filter = filter + " and eeid = N'" + text + "'";
            }
            else if (item == "3")
            {
                filter = filter + " and category = N'" + text + "'";
            }
            else if (item == "4")
            {
                filter = filter + " and skillSN = N'" + text + "'";
            }

            //if (date == "-1")
            //{

            //}
            //else if(date == "0")
            //{
            //    if (startDate != "")
            //    {
            //        filter = filter + " and OnBoardingDate >= '" + startDate + "'";
            //    }
            //    if (endDate != "")
            //    {
            //        filter = filter + " and OnBoardingDate <= '" + endDate + "'";
            //    } 
            //}
            //else if (date == "1")
            //{
            //    if (startDate != "")
            //    {
            //        filter = filter + " and CertificateDate >= '" + startDate + "'";
            //    }
            //    if (endDate != "")
            //    {
            //        filter = filter + " and CertificateDate <= '" + endDate + "'";
            //    }
            //}

            return _request.GetAllRequestsByFilter(filter);
        }
    }
}