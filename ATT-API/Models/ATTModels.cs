﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATT_API.Models
{
    public class ATTModels
    {
        public class ATTModelInfoS
        {
            public List<ATTModelInfo> list { get; set; }
            public int tbCount { get; set; }
            public string ErrorCode { get; set; }
        }
        public class ATTModelInfo
        {
            public  string USERID { get; set; }
            public DateTime CHECKTIME { get; set; }
            public string NAME { get; set; }
        }
        public class ATTNameInfoS
        {
            public List<ATTNameInfo> list;
            public string ErrorCode { get; set; }
        }
        public class ATTNameInfo
        {
            public string USERID { get; set; }
            public string NAME { get; set; }
        }
        public class _ATTNameInfo
        {
            public int USERID { get; set; }
            public string NAME { get; set; }
            public DateTime CHECKTIME { get; set; }
        }
        public class ValidateATT
        {
            public int USERID { get; set; }
            public string CHECKTIME { get; set; }
            public string NAME { get; set; }
            public string errormsg  { get; set; }
            public string TIME { get; set; }
        }
        public class ValidateATTInfoS
        {
            public List<ValidateATT> list;
            public int ErrorCode { get; set; }
        }
        public class UserInfo
        {
            public int USERID { get; set; }
            public string NAME { get; set; }
            public int StateATT { get; set; }
        }
        public class UserInfoList
        {
            public List<UserInfo> listAll;
            public List<UserInfo> list;
            public int ErrorCode { get; set; }
        }
    }
}