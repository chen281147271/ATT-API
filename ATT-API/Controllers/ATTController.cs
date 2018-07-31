using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ATT_API.Controllers
{
    public class ATTController : ApiController
    {
        BIZ.QueryATT queryATT = new BIZ.QueryATT();
        [HttpGet]
        public IHttpActionResult GetATTInfo(string Key, DateTime sTime, DateTime eTime,int PageSize,int CurPage)
        {
            return Ok(queryATT.QueryATTInfo(Key, sTime, eTime, PageSize, CurPage));
        }
        [HttpGet]
        public IHttpActionResult GetNameInfo()
        {
            return Ok(queryATT.GetNameList());
        }
    }
}