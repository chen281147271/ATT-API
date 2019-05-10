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
        [HttpPost]
        public IHttpActionResult ValidateATT([FromBody]dynamic jsonText)
        {
            var workday = jsonText.workday;
            var satday = jsonText.satday;
            var time = jsonText.time;
            return Ok(queryATT.ValidateATT(workday, satday,time));
        }
        [HttpGet]
        [Route("api/GetUserInfo_ATT")]
        public IHttpActionResult GetUserInfo_ATT()
        {
            return Ok(queryATT.GetUserInfo());
        }
        [HttpPost]
        [Route("api/Up_UserInfo")]
        public IHttpActionResult Up_UserInfo([FromBody]dynamic jsonText)
        {
            var value = jsonText.value;
            return Ok(queryATT.Up_UserInfo(value));
        }
        [HttpGet]
        [Route("api/SetX")]
        public IHttpActionResult SetX( DateTime sTime, DateTime eTime, int UID)
        {
            return Ok(queryATT.SetX( sTime, eTime, UID));
        }
    }
}