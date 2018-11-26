﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;
using ATT_API.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Infrastructure;

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
                using (kaoqingEntities db = new kaoqingEntities())
                {
                    string _sTime = "";
                    string _eTime = "";
                    // _sTime = sTime.Month + "/" + sTime.Day + "/" + sTime.Year;
                    //  _eTime = eTime.Month + "/" + (eTime.Day+1).ToString() + "/" + eTime.Year;
                    _sTime = sTime.Year + "-" + sTime.Month + "-" + sTime.Day;
                    _eTime = eTime.Year + "-" + eTime.Month + "-" + eTime.Day.ToString();
                    _eTime = Convert.ToDateTime(_eTime).AddDays(1).ToString("yyyy/MM/dd");
                    // DataSet ds = new DataSet();
                    //string sql = "SELECT CHECKINOUT.USERID, CHECKINOUT.CHECKTIME,USERINFO.Name FROM CHECKINOUT, USERINFO WHERE ";
                    string sql = "SELECT distinct CHECKINOUT.CHECKTIME,CHECKINOUT.USERID,USERINFO.Name FROM CHECKINOUT, USERINFO WHERE ";
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
                        sql += " USERINFO.Name = '";
                        sql += Key;
                        sql += "') and ";
                    }
                    //sql += " CHECKINOUT.USERID = USERINFO.USERID and  CHECKINOUT.CHECKTIME between #";
                    //sql += _sTime;
                    //sql += "# and #";
                    //sql += _eTime;
                    //sql += "# ORDER BY CHECKINOUT.CHECKTIME;";
                    sql += " CHECKINOUT.USERID = USERINFO.USERID and  CHECKINOUT.CHECKTIME between '";
                    sql += _sTime;
                    sql += "' and '";
                    sql += _eTime;
                    sql += "' ORDER BY CHECKINOUT.CHECKTIME DESC;";
                    //  ds = AccessHelper.dataSet(sql);
                    var ds = db.Database.SqlQuery<Models.ATTModels._ATTNameInfo>(sql);
                    // tbCount = ds.Tables[0].Rows.Count;
                    tbCount = ds.Count();
                    //  foreach (DataRow dr in GetPagedTable(ds.Tables[0], CurPage, PageSize).Rows)
                    foreach (var dr in ds.Skip((CurPage - 1) * PageSize).Take(PageSize))
                    {
                        Models.ATTModels.ATTModelInfo aTTModelInfo = new Models.ATTModels.ATTModelInfo();
                        //aTTModelInfo.USERID = dr["USERID"].ToString();
                        //aTTModelInfo.NAME = dr["NAME"].ToString();
                        //aTTModelInfo.CHECKTIME = Convert.ToDateTime(dr["CHECKTIME"]);
                        aTTModelInfo.USERID = dr.USERID.ToString();
                        aTTModelInfo.NAME = dr.NAME.ToString();
                        aTTModelInfo.CHECKTIME = dr.CHECKTIME;
                        list.Add(aTTModelInfo);
                    }
                    Models.ATTModels.ATTModelInfoS aTTModelInfoS = new Models.ATTModels.ATTModelInfoS();
                    aTTModelInfoS.ErrorCode = "";
                    aTTModelInfoS.list = list;
                    aTTModelInfoS.tbCount = tbCount;
                    return aTTModelInfoS;
                }
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
                using (kaoqingEntities db = new kaoqingEntities())
                {
                   // DataSet ds = new DataSet();
                    // string sql = "SELECT userid,name from userinfo";
                    // ds = AccessHelper.dataSet(sql);
                    var ds = db.USERINFO.ToList();
                    // foreach (DataRow dr in ds.Tables[0].Rows)
                    foreach (var a in ds)
                    {
                        Models.ATTModels.ATTNameInfo aTTNameInfo = new Models.ATTModels.ATTNameInfo();
                        //aTTNameInfo.NAME = dr["name"].ToString();
                        //aTTNameInfo.USERID = dr["userid"].ToString();
                        aTTNameInfo.NAME = a.Name.ToString();
                        aTTNameInfo.USERID = a.USERID.ToString();
                        list.Add(aTTNameInfo);
                    }
                    Models.ATTModels.ATTNameInfoS aTTNameInfoS = new Models.ATTModels.ATTNameInfoS();
                    aTTNameInfoS.list = list;
                    aTTNameInfoS.ErrorCode = "";
                    return aTTNameInfoS;
                }
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
        public Models.ATTModels.ValidateATTInfoS ValidateATT(object workday, object satday,object time)
        {

            JArray arraytime = (JArray)time;
            string stime = arraytime[0].ToString();
            string etime = arraytime[1].ToString();
            List<Models.ATTModels.ATTModelInfo> list = new List<Models.ATTModels.ATTModelInfo>();
            list = QueryATTInfo(null, Convert.ToDateTime(stime), Convert.ToDateTime(etime).AddDays(1), 999999, 1).list;
            List<Models.ATTModels.ValidateATT> listVA = new List<ATTModels.ValidateATT>();
            Models.ATTModels.ValidateATTInfoS aTTInfoS = new ATTModels.ValidateATTInfoS();
            List<string> arrayworkday = ChangeDay((JArray)workday);
            List<string> arraysatday = ChangeDay((JArray)satday);
            try
            {
                using (kaoqingEntities db = new kaoqingEntities())
                {
                    var userlist = db.USERINFO.Where(p => p.StateATT == 1).ToList();
                    foreach (var a in userlist)
                    {
                        var vt = CheckValidateATT(a.USERID, list, arrayworkday, arraysatday);
                        foreach (var vtt in vt)
                        {
                            listVA.Add(vtt);
                        }
                    }
                    aTTInfoS.ErrorCode = 0;
                    aTTInfoS.list = listVA;
                    return aTTInfoS;
                }
            }catch(Exception ex)
            {
                aTTInfoS.ErrorCode = -1;
                aTTInfoS.list = listVA;
                return aTTInfoS;
            }
        }
        private List<string> ChangeDay(JArray array)
        {
            List<string> list = new List<string>();
            foreach (var a in array)
            {
                string str = a.ToString();
                str = str.Replace("年", "/");
                str = str.Replace("月", "/");
                str = str.Replace("日", "");
                list.Add(str);
            }
            return list;
        }
        private List<Models.ATTModels.ValidateATT> CheckValidateATT(int uid, List<Models.ATTModels.ATTModelInfo> list, List<string> arrayworkday, List<string> arraysatday)
        {
            List<Models.ATTModels.ValidateATT> listVA = new List<ATTModels.ValidateATT>();
            foreach (var a in arrayworkday) {
                List<Models.ATTModels.ATTModelInfo> temp = new List<ATTModels.ATTModelInfo>();
                Models.ATTModels.ValidateATT validateATT = new ATTModels.ValidateATT();
                string strstime = a + " 9:00:00";
                DateTime stime = Convert.ToDateTime(strstime);
                string stretime = a + " 18:30:00";
                DateTime etime = Convert.ToDateTime(stretime);
                string errmsg = "";
                string CHECKTIME = "";
                temp =list.Where(p => p.CHECKTIME.ToString("yyyy/M/d") == a && p.USERID==uid.ToString()).ToList();
                if (temp.Count == 0)
                {
                    validateATT.TIME = a;
                    validateATT.errormsg = "该日期没有打卡记录！";
                    validateATT.NAME = GetName(uid);
                    validateATT.USERID = uid;
                    validateATT.CHECKTIME = "";
                    listVA.Add(validateATT);
                    continue;
                }
                else
                {
                    var s = temp.Where(p => p.CHECKTIME < stime).ToList();
                    if (s.Count == 0 )
                    {
                        errmsg += "早上迟到！  ";
                    }
                    var e = temp.Where(p => p.CHECKTIME > etime).ToList();
                    if (e.Count == 0 )
                    {
                        errmsg += "下午早退！   ";
                    }
                    foreach(var ctime in temp)
                    {
                        CHECKTIME += ctime.CHECKTIME;
                        CHECKTIME += " 、";
                    }
                    CHECKTIME=CHECKTIME.Remove(CHECKTIME.Length - 1, 1);
                    validateATT.TIME = a;
                    validateATT.errormsg = errmsg;
                    validateATT.NAME = GetName(uid);
                    validateATT.USERID = uid;
                    validateATT.CHECKTIME = CHECKTIME;
                    if (errmsg != "")
                    {
                        listVA.Add(validateATT);
                    }
                }
            }
            foreach (var a in arraysatday)
            {
                List<Models.ATTModels.ATTModelInfo> temp = new List<ATTModels.ATTModelInfo>();
                Models.ATTModels.ValidateATT validateATT = new ATTModels.ValidateATT();
                string strstime = a + " 9:00:00";
                DateTime stime = Convert.ToDateTime(strstime);
                string stretime = a + " 16:30:00";
                DateTime etime = Convert.ToDateTime(stretime);
                string errmsg = "";
                string CHECKTIME = "";
                temp = list.Where(p => p.CHECKTIME.ToString("yyyy/M/d") == a && p.USERID == uid.ToString()).ToList();
                if (temp.Count == 0)
                {
                    validateATT.TIME = a;
                    validateATT.errormsg = "该日期没有打卡记录！";
                    validateATT.NAME = GetName(uid);
                    validateATT.USERID = uid;
                    validateATT.CHECKTIME = "";
                    listVA.Add(validateATT);
                    continue;
                }
                else
                {
                    var s = temp.Where(p => p.CHECKTIME < stime).ToList();
                    if (s.Count == 0)
                    {
                        errmsg += "早上迟到！  ";
                    }
                    var e = temp.Where(p => p.CHECKTIME > etime).ToList();
                    if (e.Count == 0)
                    {
                        errmsg += "下午早退！   ";
                    }
                    foreach (var ctime in temp)
                    {
                        CHECKTIME += ctime.CHECKTIME;
                        CHECKTIME += " 、";
                    }
                    CHECKTIME = CHECKTIME.Remove(CHECKTIME.Length - 1, 1);
                    validateATT.TIME = a;
                    validateATT.errormsg = errmsg;
                    validateATT.NAME = GetName(uid);
                    validateATT.USERID = uid;
                    validateATT.CHECKTIME = CHECKTIME;
                    if (errmsg != "")
                    {
                        listVA.Add(validateATT);
                    }
                }
            }

            return listVA;
        }
        private string GetName(int uid)
        {
            using (kaoqingEntities db = new kaoqingEntities())
            {
                return db.USERINFO.Where(p=>p.USERID==uid).FirstOrDefault().Name;
            }
        }
        public Models.ATTModels.UserInfoList GetUserInfo()
        {
            List<Models.ATTModels.UserInfo> list = new List<Models.ATTModels.UserInfo>();
            List<Models.ATTModels.UserInfo> listALL = new List<Models.ATTModels.UserInfo>();
            Models.ATTModels.UserInfoList infoList = new ATTModels.UserInfoList();
            try
            {
                using (kaoqingEntities db = new kaoqingEntities())
                {
                    var T_list = db.USERINFO.Where(p => p.StateATT == 1).ToList();
                    foreach (var a in T_list)
                    {
                        Models.ATTModels.UserInfo userInfo = new ATTModels.UserInfo();
                        userInfo.NAME = a.Name;
                        userInfo.StateATT = 1;
                        userInfo.USERID = a.USERID;
                        list.Add(userInfo);
                    }
                    T_list = db.USERINFO.ToList();
                    foreach (var a in T_list)
                    {
                        Models.ATTModels.UserInfo userInfo = new ATTModels.UserInfo();
                        userInfo.NAME = a.Name;
                        userInfo.StateATT = 0;
                        userInfo.USERID = a.USERID;
                        listALL.Add(userInfo);
                    }
                    infoList.list = list;
                    infoList.listAll = listALL;
                    infoList.ErrorCode = 0;
                    return infoList;
                }
            }catch(Exception ex)
            {
                infoList.list = null;
                infoList.listAll = null;
                infoList.ErrorCode =-1;
                return infoList;
            }
        }
        public int Up_UserInfo(object value)
        {
            try
            {
                using (kaoqingEntities db = new kaoqingEntities())
                {
                    string str = "";
                    string sql = "";
                    JArray arrayvalue = (JArray)value;
                    foreach (var a in arrayvalue)
                    {
                        int uid = Convert.ToInt32( ((Newtonsoft.Json.Linq.JValue)a).Value);
                        USERINFO entity = db.USERINFO.Where(p => p.USERID == uid).FirstOrDefault();
                        entity.StateATT = 1;
                        db.USERINFO.Attach(entity);
                        var stateEntity = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntry(entity);
                        stateEntity.SetModifiedProperty("StateATT");
                        db.SaveChanges();

                        str += uid.ToString();
                        str += ",";
                    }
                    if (str == "")
                    {
                        sql = "update USERINFO set StateATT = 0";
                    }
                    else
                    {
                        str = str.Substring(0, str.Length - 1);
                        sql = "update USERINFO set StateATT = 0 where  USERID not in(" + str + ")";
                    }
                    db.Database.ExecuteSqlCommand(sql);
                    return 0;
                }
            }
            catch(Exception ex)
            {
                return -1;
            }
        }
    }
}