using Microsoft.AspNetCore.Mvc;
using System;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace restsharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class productCategoriesController : Controller
    {
        public static string[] Things;
        public List<dynamic> list = new List<dynamic>();

        //get products
        [HttpGet]
        public String Get(int id)
        {
            string cs = @"server=localhost;userid=root;password=;database=testa";

            using var con = new MySqlConnection(cs);
            con.Open();
            var stm = "SELECT * FROM categories";
            var cmd = new MySqlCommand(stm, con);

            MySqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Category prod = new Category()
                {
                    categoryid = rdr.GetString(0),
                    categoryname = rdr.GetString(1),                
                };

                string jsonval = JsonConvert.SerializeObject(prod);
                Console.WriteLine(jsonval);
                list.Add(prod);
            }

            Console.WriteLine("line 61");
            var output = JsonConvert.SerializeObject(list);
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(output);
            con.Close();
            return output;
        }

        //create categories
        [HttpPost]
        public ActionResult Products([FromForm] ProductModel prd)
        {
            string name = prd.name;

            Console.WriteLine(name);

            string cs = @"server=localhost;userid=root;password=;database=testa";

            using var con = new MySqlConnection(cs);
            con.Open();

            using var cmd = new MySqlCommand();
            cmd.Connection = con;

            cmd.CommandText = $"Insert INTO categories (category_name) VALUES ("+ '"' + name + '"' + ")";
            cmd.ExecuteNonQuery();
            con.Close();
            return Ok(new { status = true, message = "successfully saved" });
        }

    }
}

class Category
{
    public string categoryid { get; set;  }

    public string categoryname { get; set; }
}



public class ProductModel
{
    public string name { get; set; }

    public string atid { get; set; }

    public string category { get; set; }

}