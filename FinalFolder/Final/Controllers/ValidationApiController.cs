using Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Final.Controllers
{
    public class ValidationApiController : ApiController
    {
        FinalDbEntities db = new FinalDbEntities();
        [Route("api/ValidationApi/InsertValidatedata")]
        [HttpPost]
        public IHttpActionResult InsertValidatedata([FromBody] test_Data da)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //int id = da.t_id;
            //string name = da.t_name;
            //string type = da.t_type;
            //test_Data da2 = new test_Data();
            //da2.t_id = id;
            //da2.t_name = name;
            //da2.t_type = type;
            db.test_Data.Add(da);
            db.SaveChangesAsync();
            return Ok();
        }
    }
}
