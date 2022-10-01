using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalClassLibrary1
{
    public class Class1
    {
        
        public void connectionToFinalDb()
        {
            
        }
        public DataSet selectData(string str)
        {
            SqlConnection con;
            string constr = @"Data Source=DESKTOP-GI21EV1\SQLEXPRESS; Initial Catalog=FinalDb; Integrated Security=true;";
            con = new SqlConnection(constr);
            con.Open();
            //string query = "select * from login";
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlDataAdapter.Fill(ds);
            con.Close();
            return ds;
        }
    }
}
