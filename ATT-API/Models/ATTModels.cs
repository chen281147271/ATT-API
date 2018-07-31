using System;
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
    }
}