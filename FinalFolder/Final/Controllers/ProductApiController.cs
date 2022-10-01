using Final.Models;
using ProductPurchaseApiClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using FinalClassLibrary1;
using Microsoft.Ajax.Utilities;

namespace Final.Controllers
{
    public class ProductApiController : ApiController
    {
        FinalDbEntities db = new FinalDbEntities();
        [Route("api/ProductApi/GetProductAndPurchaseData")]
        [HttpGet]
        public IHttpActionResult GetProductAndPurchaseData()
        {
            ////removed for loop
            DataSet ds = new DataSet();
            Class1 obj=new Class1();
            //ds = obj.selectData("spgetPurchase_TableData");
            //ds = obj.selectData("spGetTwoTableData");
            //var l = ds.Tables[1].AsEnumerable().Select(dataRow => new Purchase_Product
            //{
            //    PurchaseProductId = dataRow.Field<int>("PurchaseProductId"),
            //    Purchase_Id = dataRow.Field<int>("Purchase_Id"),
            //    Item = dataRow.Field<string>("Item"),
            //    Quantity = dataRow.Field<int>("Quantity"),
            //    Amount = dataRow.Field<int>("Amount")
            //}).ToList();
            //var list1 = ds.Tables[0].AsEnumerable().Select(dataRow => new Purchase_Table
            //{
            //    Purchase_Id = dataRow.Field<int>("Purchase_Id"),
            //    Purchase_No = dataRow.Field<string>("Purchase_No"),
            //    Total = dataRow.Field<int>("Total"),
            //    Date = dataRow.Field<DateTime>("Date"),
            //    Purchase_Product=l.Where(a=>a.Purchase_Id==dataRow.Field<int>("Purchase_Id")).ToList()
            //}).ToList();

            ds = obj.selectData("spGetDataInOneTable");
            var l = ds.Tables[0].AsEnumerable().Select(dataRow => new Purchase_Table
            {
                Purchase_Id = dataRow.Field<int>("Purchase_Id"),
                Purchase_No = dataRow.Field<string>("Purchase_No"),
                Total = dataRow.Field<int>("Total"),
                Date = dataRow.Field<DateTime>("Date"),
                Purchase_Product = ds.Tables[0].AsEnumerable().Select(dataRow2 => new Purchase_Product
                {
                    PurchaseProductId = dataRow2.Field<int>("PurchaseProductId"),
                    Purchase_Id = dataRow2.Field<int>("Purchase_Id"),
                    Item = dataRow2.Field<string>("Item"),
                    Quantity = dataRow2.Field<int>("Quantity"),
                    Amount = dataRow2.Field<int>("Amount")
                }).Where(a => a.Purchase_Id == dataRow.Field<int>("Purchase_Id")).ToList()
            }).DistinctBy(a => a.Purchase_Id).ToList();
            //int i = 0;
            //foreach (var item in list1)
            //{
            //    int temp = item.Purchase_Id;
            //    //DataSet ds2 = obj.selectData($"spGetAllDataPurchaseId {temp}");
            //    var l = ds.Tables[1].AsEnumerable().Select(dataRow => new Purchase_Product
            //    {
            //        PurchaseProductId = dataRow.Field<int>("PurchaseProductId"),
            //        Purchase_Id = dataRow.Field<int>("Purchase_Id"),
            //        Item = dataRow.Field<string>("Item"),
            //        Quantity = dataRow.Field<int>("Quantity"),
            //        Amount = dataRow.Field<int>("Amount")
            //    }).ToList();
            //    list1[i].Purchase_Product= l;
            //    i++;
            //}

            //List<Purchase_Table> list = new List<Purchase_Table>();
            //int i = 0;
            //foreach (var item in list1)
            //{
            //    int temp=item.Purchase_Id;
            //    DataSet dataSet = obj.selectData($"spgetdatabypurchaseId {temp}");
            //    var l = dataSet.Tables[0].AsEnumerable().Select(dataRow => new Purchase_Table
            //    {
            //        Purchase_Id = dataRow.Field<int>("Purchase_Id"),
            //        Purchase_No = dataRow.Field<string>("Purchase_No"),
            //        Date = dataRow.Field<DateTime>("Date"),
            //        Total = dataRow.Field<int>("Total"),
            //        Purchase_Product = products[i]

            //    }).ToList();
            //    i++;
            //    list.Add(l);
            //}
            return Ok(l);
        }
        [Route("api/ProductApi/insertProductData")]
        [HttpPost]
        public IHttpActionResult insertProductData(ProductTest[] obj1)
        {
            //productPurchaseApiClassLibrary obj = new productPurchaseApiClassLibrary();
            int totalAmount = 0;
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                { 
                    for (int i = 0; i < obj1.Length; i++)
                    {
                        totalAmount = totalAmount + obj1[i].amount;
                        //obj.InsertDataIntoPurchase_Product(obj1[0].Purchase_Id, (string)obj1[i].Item, obj1[i].quantity, obj1[i].amount);
                        Purchase_Product purchase_Product = new Purchase_Product();
                        purchase_Product.Purchase_Id = obj1[0].Purchase_Id;
                        purchase_Product.Item = (string)obj1[i].Item;
                        purchase_Product.Amount = obj1[i].amount;
                        purchase_Product.Quantity = obj1[i].quantity;
                        db.Purchase_Product.Add(purchase_Product);

                    }
                    //obj.InsertDataIntoPurchase_Table(obj1[0].Purchase_Id, obj1[0].Purchase_No, DateTime.Now, totalAmount);
                    SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GI21EV1\SQLEXPRESS; Initial Catalog=FinalDb; Integrated Security=true;");
                    con.Open();
                    SqlCommand pNo = new SqlCommand("spgetLastProductNo", con);
                    string number =(string)pNo.ExecuteScalar();
                    con.Close();
                    int temp = 0;
                    if (String.IsNullOrWhiteSpace(number))
                    {
                        number = "0001";
                    }
                    else
                    {
                        temp = int.Parse(number) ;
                        temp = temp+1;
                        number = temp.ToString();
                        number=number.PadLeft(4, '0');              
                    }
                    Purchase_Table purchase_Table = new Purchase_Table();
                    purchase_Table.Purchase_Id = obj1[0].Purchase_Id;
                    purchase_Table.Purchase_No = number;
                    purchase_Table.Date = DateTime.Now;
                    purchase_Table.Total = totalAmount;
                    db.Purchase_Table.Add(purchase_Table);
                    db.SaveChanges();
                    transaction.Commit();                 
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return BadRequest();
        }
        [Route("api/ProductApi/UpdateProductdata")]
        [HttpPut]
        public IHttpActionResult UpdateProductdata(ProductTest[] obj1)
        {
            //productPurchaseApiClassLibrary obj = new productPurchaseApiClassLibrary();
            for (int i = 0; i < obj1.Length; i++)
            {
                //obj.UpdateDataIntoPurchase_Table(obj1[0].PurchaseProductId, obj1[0].Purchase_Id, obj1[i].amount);
                int id = obj1[i].Purchase_Id;
                Purchase_Table purchase_Table=(from p in db.Purchase_Table where p.Purchase_Id == id select p).FirstOrDefault();
                int pid = obj1[i].PurchaseProductId;
                Purchase_Product purchase_Product=(from pp in db.Purchase_Product where pp.PurchaseProductId == pid select pp).FirstOrDefault();
                if ((purchase_Table != null) && (purchase_Product != null))
                {
                    int oldTotal = purchase_Table.Total;
                    int oldaAmount = purchase_Product.Amount;
                    int newTotal=oldTotal-oldaAmount+obj1[i].amount;
                    purchase_Product.Amount=obj1[i].amount;
                    purchase_Product.Quantity=obj1[i].quantity;
                    purchase_Table.Total = newTotal;
                    db.SaveChanges();
                }
                //obj.UpdateDataIntoPurchase_Product(obj1[0].PurchaseProductId, obj1[i].quantity, obj1[i].amount);
            }
            return Ok();
        }
        [Route("api/ProductApi/deleteProductDataByPurchaseProductId")]
        [HttpDelete]
        public IHttpActionResult deleteProductDataByPurchaseProductId([FromBody] int id)
        {
            //productPurchaseApiClassLibrary obj = new productPurchaseApiClassLibrary();
            Purchase_Product purchase_Product = (from c in db.Purchase_Product where c.PurchaseProductId == id select c).FirstOrDefault();
            //int purchaseId = (from c in db.Purchase_Product where c.PurchaseProductId == id select c.Purchase_Id).FirstOrDefault();
            //int amount = (from c in db.Purchase_Product where c.PurchaseProductId == id select c.Amount).FirstOrDefault();
            if (purchase_Product != null)
            {
                Purchase_Table purchase_Table = (from c in db.Purchase_Table where c.Purchase_Id == purchase_Product.Purchase_Id select c).FirstOrDefault();
                purchase_Table.Total = purchase_Table.Total - purchase_Product.Amount;
                db.SaveChanges();
                db.Purchase_Product.Remove(purchase_Product);
                db.SaveChanges();
            }
            return Ok();
        }
        [Route("api/ProductApi/deleteProductDataByPurchaseId")]
        [HttpDelete]
        public IHttpActionResult deleteProductDataByPurchaseId([FromBody] int id)
        {
            Purchase_Table purchase_Table = (from c in db.Purchase_Table where c.Purchase_Id == id select c).FirstOrDefault();
            if (purchase_Table != null)
            { 
                db.Purchase_Table.Remove(purchase_Table);
                db.SaveChanges();
            }
            Purchase_Product p = (from c in db.Purchase_Product where c.Purchase_Id == id select c).FirstOrDefault();
            while (p != null)
            {
                db.Purchase_Product.Remove(p);
                db.SaveChanges();
                p = (from c in db.Purchase_Product where c.Purchase_Id == id select c).FirstOrDefault();
            }
            return Ok();
        }
    }
    public class ProductTest
    {
        [NotMapped]
        public int PurchaseProductId { get; set; }
        public int Purchase_Id { get; set; }
        public string Purchase_No { get; set; }
        public string Item { get; set; }
        public int quantity { get; set; }
        public int amount { get; set; }
    }
}
