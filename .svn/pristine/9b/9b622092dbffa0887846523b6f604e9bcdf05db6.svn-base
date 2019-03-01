using Certification.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Certification.Common
{
    public class Common
    {
        public static int CreateSqlTable(DataTable dt, string tableName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"if exists(select * from sysobjects where name = '" + tableName + "' and xtype = 'u')  DROP TABLE [" + tableName + "] CREATE TABLE [" + tableName + "]( ");

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sb.Append("[" + dt.Columns[i].ColumnName + "] nvarchar(300) ,");
            }
            string str = sb.ToString().Substring(0, sb.ToString().Length - 1) + ") ";

            int iRow = SqlHelper.ExecuteNonQuery(SqlHelper.CONN_STRING_DEFAULT, CommandType.Text, str, null);
            str = "select count(0) from sysobjects where name = '" + tableName + "' and xtype = 'u'";
            return (int)SqlHelper.ExecuteScalar(SqlHelper.CONN_STRING_DEFAULT, CommandType.Text, str, null);
        }



        /// <summary>
        /// 获取datatable列名
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> GetDataTableColumnsName(DataTable dt)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                list.Add(dt.Columns[i].ColumnName);
            }

            return list;
        }


        public static DataTable GetExcelDatatable(string fileUrl)
        {
            //office2007之前 仅支持.xls            
            //const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";            
            //支持.xls和.xlsx，即包括office2010等版本的   HDR=Yes代表第一行是标题，不是数据；            
            const string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            System.Data.DataTable dt = null;
            //建立连接            
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));
            //打开连接            
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
            { conn.Open(); }
            System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //获取Excel的第一个Sheet名称            
            //string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();
            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();

            //schemaTable.Rows[1].Delete();


            //查询sheet中的数据            
            string strSql = "select * from [" + sheetName + "]";
            OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt;
        }







        /// <summary>
        /// 写入日志到文本文件
        /// </summary>
        /// <param name="action">动作</param>
        /// <param name="strMessage">日志内容</param>
        /// <param name="time">时间</param>
        public static void WriteTextLog(string action, string strMessage, DateTime time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Log\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + time.ToString("yyyyMMddhhmmssffff") + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append("Time:    " + time.ToString() + "\r\n");
            str.Append("Action:  " + action + "\r\n");
            str.Append("Message: " + strMessage + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }








    }
}