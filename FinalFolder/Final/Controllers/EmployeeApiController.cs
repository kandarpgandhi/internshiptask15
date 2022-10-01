using EmployeeClassLibrary;
using Final.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Final.Controllers
{
    public class EmployeeApiController : ApiController
    {
        FinalDbEntities db = new FinalDbEntities();
        ///////////////////////////upload//////////////////////////////
        [Route("api/EmployeeApi/UploadFiles")]
        [HttpPost]
        public HttpResponseMessage UploadFiles()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
            string fileName = HttpContext.Current.Request.Form["fileName"] + Path.GetExtension(postedFile.FileName);
            postedFile.SaveAs(path + fileName);
            employeeClassLibrary obj=new employeeClassLibrary();
            obj.InsertDataIntoEmployee((path + fileName),"a");
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, fileName);
        }
        /////////////////////////////////////////////
        [Route("api/EmployeeApi/GetEmployeeData")]
        [HttpGet]
        public IHttpActionResult GetEmployeeData([FromBody] string p)
        {
            employeeClassLibrary obj = new employeeClassLibrary();
            DataSet ds = new DataSet();
            ds = obj.SelectDataFromEmployeeTable("spGetEmployeeData", p);
            var l = ds.Tables[0].AsEnumerable().Select(DataRow => new Employee { Emp_Id = DataRow.Field<int>("Emp_Id"), Emp_Name = DataRow.Field<string>("Emp_Name"), Emp_Email = DataRow.Field<string>("Emp_Email"), Emp_Age = DataRow.Field<int>("Emp_Age") }).ToList();
            return Ok(l);
        }
        [Route("api/EmployeeApi/GetEmployeeDataByName")]
        [HttpGet]
        public IHttpActionResult GetEmployeeDataByName([FromBody] string p)
        {
            employeeClassLibrary obj = new employeeClassLibrary();
            DataSet ds = new DataSet();
            ds = obj.SelectDataFromEmployeeTable("spGetEmployeeName", p);
            var l = ds.Tables[0].AsEnumerable().Select(DataRow => new Employee { Emp_Name = DataRow.Field<string>("Emp_Name") }).ToList();
            return Ok(l);
        }
    }
}
