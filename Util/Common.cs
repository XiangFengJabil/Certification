using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using Certification.Model;

namespace Certification.Util
{
    public static class Common
    {
        private readonly static string smtpClient = ConfigurationManager.AppSettings["smtpClient"];
        private readonly static string smtpPort = ConfigurationManager.AppSettings["smtpPort"];
        private readonly static string address = ConfigurationManager.AppSettings["address"];
        private readonly static string displayname = ConfigurationManager.AppSettings["displayname"];

        private readonly static string LDAP_PATH = ConfigurationManager.AppSettings["LDAP"];

        /// <summary>
        /// Get user id from HttpContext
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentNTID()
        {
            string identityName = HttpContext.Current.User.Identity.Name;
            int splitIndex = identityName.IndexOf('\\');
            return splitIndex > -1 ? identityName.Substring(splitIndex + 1) : identityName;
        }
        public static string GetHtmls(List<Model.ApproverInfo> users)
        {
            string html = string.Empty;
            string displayname = string.Empty;
            foreach (Model.ApproverInfo u in users)
            {
                if (u.IsGroup == 1)
                    displayname = u.UID;
                else
                    displayname = u.DisplayName + "," + u.Mail;
                html += "<div class='ep-gl-guest'>"
                        + "<img src='images/icon_oa_required.png' alt='' class='imgep'>"
                        + "<span class='ep-gc-chip-text'>" + displayname
                        + "</span><input type='hidden' value='" + u.UID
                        + "' name='hid1' /><a class='a1'></a></div>";

            }
            return html;
        }

        /// <summary>
        /// Get User information from AD
        /// </summary>
        /// <param name="ntid">AD用户名</param>
        /// <returns>用户实例</returns>
        public static UserInfo GetADUserEntity(string ntid)
        {
            if (string.IsNullOrEmpty(ntid))
            {
                throw new Exception("Searched user id cannot be null.");
            }

            DirectoryEntry entry = new DirectoryEntry(LDAP_PATH);
            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = "(&(objectClass=user)(sAMAccountName=" + ntid + "))";

            SearchResult searchResult = searcher.FindOne();
            if (searchResult != null)
            {
                Model.UserInfo user = new Model.UserInfo();
                user.UID = ntid;
                user.Department = GetADProperty(searchResult, "department");
                user.DisplayName = GetADProperty(searchResult, "displayName");
                user.Email = GetADProperty(searchResult, "mail");
                user.Site = GetSiteCodeFromUserOUPath(searchResult.Path);

                return user;
            }
            return null;
        }

        /// <summary>
        /// 从OU中获取SITE信息
        /// </summary>
        /// <example>
        /// LDAP://corp.jabil.org/CN=Mingming Liu,OU=Users,OU=Wuxi,OU=RegionAsia,DC=corp,DC=JABIL,DC=ORG
        /// Site: OU=Wuxi
        /// </example>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        private static string GetSiteCodeFromUserOUPath(string ouPath)
        {
            string[] ouFolders = ouPath.Split(new char[] { ',' });
            if (ouFolders.Length > 5)
            {
                string site = ouFolders[ouFolders.Length - 5].Substring(3);
                switch (site)
                {
                    case "Anaheim": return "ANA";
                    case "AuburnHills": return "AUH";
                    case "BeloHorizonte": return "BEL";
                    case "Chennai": return "CHE";
                    case "Chihuahua": return "CHI";
                    case "Moises Amezcua": return "MEM";
                    case "Gotemba": return "got";
                    case "GPBeijing": return "BCY";
                    case "GPNanJing": return "NJN";
                    case "GPShenzhen": return "SZH";
                    case "GPSungaiPetani": return "sgp";
                    case "GPSuzhou": return "SLF";
                    case "GPTaichung": return "TCH";
                    case "GPTianjin": return "TXQ";
                    case "GPWuxi": return "WXI";
                    case "GPYantai": return "YAN";
                    case "Guadalajara": return "gdl";
                    case "HoChiMinh": return "HCM";
                    case "Huangpu": return "HUA";
                    case "Jena": return "JEN";
                    case "Kwidzyn": return "PLK";
                    case "Manaus": return "MAN";
                    case "Marcianise": return "MAR";
                    case "Memphis": return "MEM";
                    case "Mumbai": return "MUM";
                    case "Penang": return "PEN";
                    case "Poughkeepsie": return "POU";
                    case "Ranjan": return "RJN";
                    case "SanJose": return "SAN";
                    case "Shanghai": return "SHA";
                    case "Singapore": return "SIN";
                    case "StPete": return "STP";
                    case "Taipei": return "TRK";
                    case "Tempe": return "TEM";
                    case "Uzhgorod": return "UZH";
                    case "Venray": return "VNR";
                    case "Wuxi": return "WUX";
                    case "Zapopan": return "ZAP";
                    case "Hachioji": return "HAC";
                    case "Livingston": return "LIV";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据属性名，在搜索结果中查找属性值
        /// </summary>
        /// <param name="searchResult">DirectorySearcher返回的搜索结果</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值</returns>
        private static string GetADProperty(SearchResult searchResult, string propertyName)
        {
            if (searchResult.Properties.Contains(propertyName))
            {
                return searchResult.Properties[propertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetCurrentUrl()
        {
            string absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri;
            return absoluteUri.Substring(0, absoluteUri.LastIndexOf("/") + 1);
        }

        public static void ClientSendMail(string To, string Cc, string subject, string body)
        {
            if (string.IsNullOrEmpty(To)) return;

            SmtpClient client = new SmtpClient(smtpClient, int.Parse(smtpPort));

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(address, displayname);
            mail.To.Add(To);
            if (!string.IsNullOrEmpty(Cc))
            {
                mail.CC.Add(Cc);
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            client.Send(mail);

            mail.Dispose();
        }

        public static string UploadFile(FileUpload fupload, string folderName)
        {
            if (fupload.HasFile)
            {
                string folderPath = Path.Combine(fupload.Page.Server.MapPath("uploads"), folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = fupload.FileName;
                string saveFile = Path.Combine(folderPath, fileName);

                fupload.SaveAs(saveFile);

                return folderName + "/" + fileName;
            }
            return string.Empty;
        }

        public static int GetSiteId()
        {
            string siteName = BLL.OrgMgr.GetCurrentAdUser().Site;
            return BLL.OrgMgr.GetSiteId(siteName);
        }
    }

}