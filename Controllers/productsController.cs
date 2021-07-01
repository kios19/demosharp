using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace restsharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors]
    public class productsController : Controller
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
            var stm = "SELECT * FROM products LEFT JOIN categories ON products.`category` = categories.`catid` LEFT JOIN attributes ON products.`atid` = attributes.`attrid`";
            var cmd = new MySqlCommand(stm, con);

            MySqlDataReader rdr;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                product prod = new product()
                {
                    recid = rdr.GetInt32(0),
                    name = rdr.GetString(1),
                    category = rdr.GetString(5),
                    size = rdr.GetString(7),
                    color = rdr.GetString(8),
                    price = rdr.GetString(9),
                    categoryname = rdr.GetString(6),
                };
                string jsonval = JsonConvert.SerializeObject(prod);
                Console.WriteLine(jsonval);
                list.Add(prod);
                Console.WriteLine("line 51");
            }

            Console.WriteLine("line 61");
            var output = JsonConvert.SerializeObject(list);
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(output);

            return output;
        }

        //create products
        [HttpPost]
        public ActionResult CreatesProducts([FromForm] ProductModel prd)
        {
            Console.WriteLine(prd);
            string name = prd.name;

            string atid = prd.atid;

            string category = prd.category;

            string cs = @"server=localhost;userid=root;password=;database=testa";

            try
            {
                Console.WriteLine("vitu");
                Console.WriteLine(name);
                Console.WriteLine(atid);
                Console.WriteLine(category);

                using var con = new MySqlConnection(cs);
                con.Open();

                using var cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"Insert INTO products (name,atid,  category) VALUES (" + '"' + name + '"' + ',' + '"' + atid + '"' + ',' + '"' + category + '"'  + ")";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(new { status = true, message = "successfully saved" });
        }

        //update productname
        [HttpPut]
        public ActionResult UpdateProducts([FromForm] ProductModel prd)
        {
            string name = prd.name;

            Console.WriteLine(name);

            string category = prd.category;

            string cs = @"server=localhost;userid=root;password=;database=testa";

            using var con = new MySqlConnection(cs);
            con.Open();

            using var cmd = new MySqlCommand();
            cmd.Connection = con;

            cmd.CommandText = $"UPDATE products set name=" +'"'+ name + '"' + "where id=" + '"' + category + '"';
            cmd.ExecuteNonQuery();

            return Ok(new { status = true, message = "successfully saved" });
        }

        //get from params
        [HttpGet("{categoryid}")]
        public string GetQuery(string categoryid)
        {
            string cs = @"server=localhost;userid=root;password=;database=testa";
            using var con = new MySqlConnection(cs);
            con.Open();
            var stm = $"SELECT * FROM products LEFT JOIN categories ON products.`category` = categories.`catid` LEFT JOIN attributes ON products.`atid` = attributes.`attrid` WHERE products.category =" + '"' + categoryid + '"';
            var cmd = new MySqlCommand(stm, con);

            MySqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            list.Clear();
            while (rdr.Read())
            {
                product prod = new product()
                {
                    recid = rdr.GetInt32(0),
                    name = rdr.GetString(1),
                    category = rdr.GetString(5),
                    size = rdr.GetString(7),
                    color = rdr.GetString(8),
                    price = rdr.GetString(9),
                    categoryname = rdr.GetString(6),

                };
                string jsonval = JsonConvert.SerializeObject(prod);
                Console.WriteLine(jsonval);
                list.Add(prod);
            }
            var output = JsonConvert.SerializeObject(list);
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(output);
            con.Close();
            return output;
        }
    }
}
