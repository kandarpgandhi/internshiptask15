using Final.Models;
using FinalClassLibrary1;
using MembershipApiClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace Final.Controllers
{
    public class MembershipApiController : ApiController
    {
        FinalDbEntities db = new FinalDbEntities();
        [Route("api/MembershipApi/GetCondoctorDataByAuthentication")]
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetCondoctorDataByAuthentication()
        {
            Class1 obj = new Class1();
            //membershipApiClassLibrary obj = new membershipApiClassLibrary();
            obj.connectionToFinalDb();
            DataSet l = obj.selectData("sploginData");
            //DataSet l = obj.selectCondoctorData();
            var list = l.Tables[0].AsEnumerable().Select(DataRow => new login
            {
                UserId = DataRow.Field<int>("UserId"),
                Name = DataRow.Field<string>("Name"),
                password = DataRow.Field<string>("password"),
                email = DataRow.Field<string>("email"),
                Mobile_No = DataRow.Field<string>("Mobile_No")
            });
            return Ok(list);
        }
        [Route("api/MembershipApi/RegisterUserIntoFinalDb")]
        [HttpPost]
        public IHttpActionResult RegisterUserIntoFinalDb(login l)
        {
            //int id = l.UserId;
            //string name = l.Name;
            //string pass = l.password;
            //string email = l.email;
            //string mo = l.Mobile_No;
            //membershipApiClassLibrary obj = new membershipApiClassLibrary();
            //obj.connectionToFinalDb();
            //obj.insertData(id, name, pass, email, mo);
            db.logins.Add(l);
            db.SaveChangesAsync();
            MembershipUser newUser = Membership.CreateUser(l.Name, l.password);
            return Ok();
        }
    }
}
