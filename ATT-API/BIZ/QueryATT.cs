using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace ATT_API.BIZ
{
    public class QueryATT
    {
        public Models.ATTModels.ATTModelInfoS QueryATTInfo(string Key,DateTime sTime,DateTime eTime, int PageSize, int CurPage)
        {
            List<Models.ATTModels.ATTModelInfo> list = new List<Models.ATTModels.ATTModelInfo>();
            int tbCount = 0;
            try
            {
                string _sTime = "";
                string _eTime = "";
                _sTime = sTime.Month + "/" + sTime.Day + "/" + sTime.Year;
                _eTime = eTime.Month + "/" + eTime.Day + "/" + eTime.Year;
                DataSet ds = new DataSet();
                string sql = "SELECT CHECKINOUT.USERID, CHECKINOUT.CHECKTIME,USERINFO.Name FROM CHECKINOUT, USERINFO WHERE ";
                if (Key != null)
                {
                    if (IsNumeric(Key))
                    {
                        sql += " (CHECKINOUT.USERID = ";
                        sql += Key;
                        sql += " or ";
                    }
                    else
                    {
                        sql += "(";
                    }
                    sql+=" USERINFO.Name = '";
                    sql += Key;
                    sql+= "') and ";
                }
                sql += " CHECKINOUT.USERID = USERINFO.USERID and  CHECKINOUT.CHECKTIME between #";
                sql += _sTime;
                sql += "# and #";
                sql += _eTime;
                sql += "# ORDER BY CHECKINOUT.CHECKTIME;";
                ds = AccessHelper.dataSet(sql);
                tbCount = ds.Tables[0].Rows.Count;
                foreach (DataRow dr in GetPagedTable(ds.Tables[0],CurPage,PageSize).Rows)
                {
                    Models.ATTModels.ATTModelInfo aTTModelInfo = new Models.ATTModels.ATTModelInfo();
                    aTTModelInfo.USERID = dr["USERID"].ToString();
                    aTTModelInfo.NAME = dr["NAME"].ToString();
                    aTTModelInfo.CHECKTIME = Convert.ToDateTime(dr["CHECKTIME"]);
                    list.Add(aTTModelInfo);
                }
                Models.ATTModels.ATTModelInfoS aTTModelInfoS = new Models.ATTModels.ATTModelInfoS();
                aTTModelInfoS.ErrorCode = "";
                aTTModelInfoS.list = list;
                aTTModelInfoS.tbCount = tbCount;
                return aTTModelInfoS;
            }
            catch(Exception ex)
            {
                Models.ATTModels.ATTModelInfoS aTTModelInfoS = new Models.ATTModels.ATTModelInfoS();
                aTTModelInfoS.ErrorCode = ex.Message;
                aTTModelInfoS.list = list;
                aTTModelInfoS.tbCount = 0;
                return aTTModelInfoS;
            }
        }
        public Models.ATTModels.ATTNameInfoS GetNameList()
        {
            List<Models.ATTModels.ATTNameInfo>list = new List<Models.ATTModels.ATTNameInfo>();
            try
            {
                DataSet ds = new DataSet();
                string sql = "SELECT userid,name from userinfo";
                ds = AccessHelper.dataSet(sql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Models.ATTModels.ATTNameInfo aTTNameInfo = new Models.ATTModels.ATTNameInfo();
                    aTTNameInfo.NAME = dr["name"].ToString();
                    aTTNameInfo.USERID = dr["userid"].ToString();
                    list.Add(aTTNameInfo);
                }
                Models.ATTModels.ATTNameInfoS aTTNameInfoS = new Models.ATTModels.ATTNameInfoS();
                aTTNameInfoS.list = list;
                aTTNameInfoS.ErrorCode = "";
                return aTTNameInfoS;
            }
            catch (Exception ex)
            {
                Models.ATTModels.ATTNameInfoS aTTNameInfoS = new Models.ATTModels.ATTNameInfoS();
                aTTNameInfoS.list = list;
                aTTNameInfoS.ErrorCode = ex.Message;
                return aTTNameInfoS;
            }
        }
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)//PageIndex表示第几页，PageSize表示每页的记录数
        {
            if (PageIndex == 0)
                return dt;//0页代表每页数据，直接返回

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;//源数据记录数小于等于要显示的记录，直接返回dt

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
    }
}