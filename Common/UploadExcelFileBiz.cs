﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.OleDb;

namespace Certification.Common
{
    public class UploadExcelFileBiz
    {
        //Excel数据导入Datable
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
            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();
            //查询sheet中的数据            
            string strSql = "select * from [" + sheetName + "]";
            OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dt = ds.Tables[0];
            return dt;
        }

        public static int WriteIntoDataBase(DataTable dt, string tableName)
        {
            return DAL.SqlHelper.WriteIntoDataBase(dt, tableName);

        }

        public static DataTable ModifyColumnsForCertificationTable(DataTable dt, out int errorRow)
        {
            DataTable newDt = new DataTable();
            /*
                database CertificationTable table columns:
                Item , Name , Workcell , EEID , NTID , OnBoardingDate , Category , SkillSN , 
	            CertificateDate , 1st_1 , 1st_2 , 2nd_1 , 2nd_2 , 3rd_1 , 3rd_2 , 4th_1 , 
	            4th_2 , 5th_1 , 5th_2 , 6th_1 , 6th_2 , 7th_1 , 7th_2 , 8th_1 , 
	            8th_2 , 9th_1 , 9th_2 , 10th_1 , 10th_2 , 11th_1 , 11th_2 , 12th_1 , 
	            12th_2,IsSendEmail
             */
            List<string> listString = new List<string>() { "Item", "Name", "Workcell", "EEID", "NTID", "OnBoardingDate", "Category", "SkillSN", "CertificateDate", "1st_1", "1st_2", "2nd_1", "2nd_2", "3rd_1", "3rd_2", "4th_1", "4th_2", "5th_1", "5th_2", "6th_1", "6th_2", "7th_1", "7th_2", "8th_1", "8th_2", "9th_1", "9th_2", "10th_1", "10th_2", "11th_1", "11th_2", "12th_1", "12th_2", "IsSendEmail" };
            //List<string> listString = new List<string>();
            //listString.Add("Item"); listString.Add("Name"); listString.Add("Workcell"); listString.Add("EEID");
            //listString.Add("NTID"); listString.Add("OnBoardingDate"); listString.Add("Category"); listString.Add("SkillSN");
            //listString.Add("CertificateDate"); listString.Add("1st_1"); listString.Add("1st_2"); listString.Add("2nd_1");
            //listString.Add("2nd_2"); listString.Add("3rd_1"); listString.Add("3rd_2"); listString.Add("4th_1");
            //listString.Add("4th_2"); listString.Add("5th_1"); listString.Add("5th_2"); listString.Add("6th_1");
            //listString.Add("6th_2"); listString.Add("7th_1"); listString.Add("7th_2"); listString.Add("8th_1");
            //listString.Add("8th_2"); listString.Add("9th_1"); listString.Add("9th_2"); listString.Add("10th_1");
            //listString.Add("10th_2"); listString.Add("11th_1"); listString.Add("11th_2"); listString.Add("12th_1");
            //listString.Add("12th_2"); listString.Add("IsSendEmail");

            errorRow = 0;
            try
            {
                newDt = dt.Clone();

                for (int i = 0; i < listString.Count; i++)
                {
                    //同频数据库中Certification的列名
                    newDt.Columns[i].ColumnName = listString[i];
                    dt.Columns[i].ColumnName = listString[i];

                    //处理数据类型
                    if (listString[i] == "Item")
                    {
                        newDt.Columns[i].DataType = System.Type.GetType("System.Int32");
                    }
                    else if (listString[i] == "OnBoardingDate" || listString[i] == "CertificateDate"
                       || listString[i] == "1st_1" || listString[i] == "2nd_1" || listString[i] == "3rd_1" || listString[i] == "4th_1"
                       || listString[i] == "5th_1" || listString[i] == "6th_1" || listString[i] == "7th_1" || listString[i] == "8th_1" ||
                       listString[i] == "9th_1" || listString[i] == "10th_1" || listString[i] == "11th_1" || listString[i] == "12th_1")
                    {
                        newDt.Columns[i].DataType = System.Type.GetType("System.DateTime");
                    }
                    else
                    {
                        newDt.Columns[i].DataType = System.Type.GetType("System.String");
                    }
                }

                //同步Excel中的数据到新的Datatable中
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowNew = newDt.NewRow();
                    //errorRow = i;
                    //newDt.ImportRow(dt.Rows[i]);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "Item")
                        {
                            rowNew[j] = Convert.ToInt32(dt.Rows[i][j]);
                        }
                        else if (dt.Columns[j].ColumnName == "OnBoardingDate" || dt.Columns[j].ColumnName == "CertificateDate"
                       || dt.Columns[j].ColumnName == "1st_1" || dt.Columns[j].ColumnName == "2nd_1" || dt.Columns[j].ColumnName == "3rd_1" || dt.Columns[j].ColumnName == "4th_1"
                       || dt.Columns[j].ColumnName == "5th_1" || dt.Columns[j].ColumnName == "6th_1" || dt.Columns[j].ColumnName == "7th_1" || dt.Columns[j].ColumnName == "8th_1" ||
                       dt.Columns[j].ColumnName == "9th_1" || dt.Columns[j].ColumnName == "10th_1" || dt.Columns[j].ColumnName == "11th_1" || dt.Columns[j].ColumnName == "12th_1")
                        {
                            if (dt.Rows[i][j] != null && !string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                            {
                                rowNew[j] = Convert.ToDateTime(dt.Rows[i][j].ToString().Trim());
                            }
                            else
                            {
                                rowNew[j] = DBNull.Value;
                            }
                        }
                        else
                        {
                            rowNew[j] = dt.Rows[i][j].ToString();
                        }
                    }
                    errorRow = i;
                    newDt.Rows.Add(rowNew);
                }
                errorRow = 0;
                return newDt;
            }
            catch (Exception ex)
            {
                errorRow = errorRow + 2;
                return new DataTable();
                throw ex;
            }

        }
    }
}