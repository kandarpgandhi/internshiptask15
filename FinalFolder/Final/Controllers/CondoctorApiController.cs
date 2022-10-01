using CondoctorApiClassLibrary;
using Final.Models;
using FinalClassLibrary1;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Final.Controllers
{
    public class CondoctorApiController : ApiController
    {
        FinalDbEntities db = new FinalDbEntities();
        [Route("api/CondoctorApi/GetCondoctorData")]
        [HttpGet]
        public IHttpActionResult GetCondoctorData()
        {
            //condoctorApiclassLibrary obj=new condoctorApiclassLibrary();
            //DataSet ds = obj.selectConDoctorData("spGetCondoctor");
            Class1 obj=new Class1();
            DataSet ds = obj.selectData("spGetCondoctor");
            var l = ds.Tables[0].AsEnumerable().Select(DataRow => new condoctor { c_id = DataRow.Field<int>("c_id"), c_name = DataRow.Field<string>("c_name"), c_age = DataRow.Field<int>("c_age") }).ToList();
            return Ok(l);
        }
        [Route("api/CondoctorApi/GetCondoctorDataById")]
        [HttpGet]
        public IHttpActionResult GetCondoctorDataById([FromBody]int id)
        {
            //condoctorApiclassLibrary obj = new condoctorApiclassLibrary();
            //DataSet ds = obj.selectConDoctorData($"spGetCndoctorById {id}");
            Class1 obj = new Class1();
            DataSet ds = obj.selectData($"spGetCndoctorById {id}");
            var l = ds.Tables[0].AsEnumerable().Select(DataRow => new condoctor { c_id = DataRow.Field<int>("c_id"), c_name = DataRow.Field<string>("c_name"), c_age = DataRow.Field<int>("c_age") }).ToList();
            return Ok(l);
        }
        [Route("api/CondoctorApi/InsertDataIntoCondoctorOrBus")]
        [HttpPost]
        public IHttpActionResult InsertDataIntoCondoctorOrBus(CondoctorTest objData)
        {
            //int id = objData.c_id;
            //string name = objData.name;
            //int age = objData.age;
            if (objData.bus_id != null)
            {
                //int bId = int.Parse(objData.bus_id);
                common cm = new common();
                //cm.c_id = id;
                cm.c_id = objData.c_id;
                //cm.bus_id = bId;
                cm.bus_id=int.Parse(objData.bus_id);
                db.commons.Add(cm);
                db.SaveChanges();
            }
            condoctor c = new condoctor();
            //c.c_id = id;
            c.c_id=objData.c_id;
            //c.c_name = name;
            c.c_name=objData.name.ToString();
            //c.c_age = age;
            c.c_age = objData.age;
            db.condoctors.Add(c);
            db.SaveChanges();
            return Ok();
        }
        [Route("api/CondoctorApi/updateDataIntoCondoctorOrBus")]
        [HttpPut]
        public IHttpActionResult updateDataIntoCondoctorOrBus(CondoctorTest obj)
        {
            var con = db.condoctors.Where(model => model.c_id == obj.c_id).FirstOrDefault();
            if (con != null)
            {
                if (obj.bus_id != null)
                {
                    int bId = int.Parse(obj.bus_id);
                    var com = db.commons.Where(model => model.bus_id == bId).FirstOrDefault();
                    //var com = db.commons.Where(model => model.bus_id == int.Parse(obj.bus_id)).FirstOrDefault();
                    //com.bus_id = bId;
                    com.bus_id = int.Parse(obj.bus_id);
                    db.SaveChanges();
                }
                con.c_id = obj.c_id;
                con.c_name = obj.name;
                con.c_age = obj.age;
                db.SaveChanges();
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }
    }
    public class CondoctorTest
    {
        //public int bus_id { get; set; }
        [NotMapped]
        public string bus_id { get; set; }
        public int bus_time { get; set; }
        public string bus_route { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        [Required]
        public int c_id { get; set; }
    }
}
