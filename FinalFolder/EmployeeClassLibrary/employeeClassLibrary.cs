using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeClassLibrary
{
    public class employeeClassLibrary
    {
        public DataSet SelectDataFromEmployeeTable(string str, string p)
        {
            string q = @"Data Source=DESKTOP-GI21EV1\SQLEXPRESS; Initial Catalog=FinalDb; Integrated Security=true;";
            SqlConnection con = new SqlConnection(q);
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            using (DataTable dt = new DataTable())
            {
                adapter.Fill(dt);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Emp");
                    string temp = p + ".xlsx";
                    wb.SaveAs(temp);
                }
            }
            con.Close();
            return ds;
        }
        public void InsertDataIntoEmployee(string p1, string p2)
        {
            string q = @"Data Source=DESKTOP-GI21EV1\SQLEXPRESS; Initial Catalog=FinalDb; Integrated Security=true;";
            SqlConnection con = new SqlConnection(q);
            con.Open();
            using (XLWorkbook workBook = new XLWorkbook(p1))
            {
                IXLWorksheet workSheet = workBook.Worksheet("Emp");
                Worksheet excelSheet = new Worksheet();
                DataTable dt = new DataTable();
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Emp_Id");
                dataTable.Columns.Add("Emp_Name");
                dataTable.Columns.Add("Emp_Email");
                dataTable.Columns.Add("Mobile_No");
                dataTable.Columns.Add("Emp_Age");
                dt.Columns.Add("Emp_Id");
                dt.Columns.Add("Emp_Name");
                dt.Columns.Add("Emp_Email");
                dt.Columns.Add("Mobile_No");
                dt.Columns.Add("Emp_Age");
                dt.Columns.Add("Error");
                int flag = 0;
                int i = 0;
                foreach (IXLRow r in workSheet.Rows())
                {
                    DataRow dr = dataTable.NewRow();
                    dr["Emp_Id"] = r.Cell("A").Value;
                    dr["Emp_Name"] = r.Cell("B").Value;
                    dr["Emp_Email"] = r.Cell("C").Value;
                    dr["Mobile_No"] = r.Cell("D").Value;
                    dr["Emp_Age"] = r.Cell("E").Value;
                    dataTable.Rows.Add(dr);
                }
                int f = 0;
                foreach (IXLRow row in workSheet.Rows())
                {
                    if (f == 0)
                    {
                        f = 1;
                    }
                    else
                    {
                        Int64 id = 0;
                        string age = "";
                        string name = "", email = "", mobile = "";
                        id = Int64.Parse(dataTable.Rows[i][0].ToString());
                        name = dataTable.Rows[i][1].ToString();
                        email = dataTable.Rows[i][2].ToString();
                        mobile = dataTable.Rows[i][3].ToString();
                        age = dataTable.Rows[i][4].ToString();
                        if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(mobile) || String.IsNullOrWhiteSpace(dataTable.Rows[i][0].ToString()) || String.IsNullOrWhiteSpace(dataTable.Rows[i][4].ToString()))
                        {
                            flag = 1;
                            DataRow dr = dt.NewRow();
                            dr["Emp_Id"] = id;
                            dr["Emp_Name"] = name;
                            dr["Emp_Email"] = email;
                            dr["Mobile_No"] = mobile;
                            dr["Emp_Age"] = age;
                            dr["Error"] = "Error";
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            int Eage = Int32.Parse(age);
                            string str = "insert into Employee (Emp_Id,Emp_Name,Emp_Email,Mobile_No,Emp_Age) values('" + id + "','" + name + "','" + email + "','" + mobile + "','" + Eage + "')";
                            SqlCommand cmd = new SqlCommand(str, con);
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.SelectCommand.ExecuteNonQuery();
                            //adapter.SelectCommand.ExecuteNonQueryAsync();
                        }
                    }
                    i++;
                }
                if (flag == 1)
                {
                    using (DataTable dt1 = new DataTable())
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "EmpError");
                            wb.SaveAs("C:\\Users\\kanda\\Downloads\\kanError.xlsx");//p2//p1D:\\intern\\kanError.xlsx                  
                        }
                    }
                }
            }
            con.Close();
        }
    }
}
