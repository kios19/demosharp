using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace restsharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class productAttributes : Controller
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
            var stm = "SELECT * FROM attributes";
            var cmd = new MySqlCommand(stm, con);
            MySqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                attributes prod = new attributes()
                {
                    attrid = rdr.GetInt32(0),

                    attname = rdr.GetString(4),

                    size = rdr.GetString(1),

                    color = rdr.GetString(2),

                    price = rdr.GetString(3),

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



        //create attributes
        [HttpPost]
        public ActionResult CreatesProducts([FromForm] attributes prd)
        {
            Console.WriteLine(prd);
                    
            string color = prd.color;

            string size = prd.size;

            string price = prd.price;

            string attname = prd.attname;

            string cs = @"server=localhost;userid=root;password=;database=testa";

            try
            {
                Console.WriteLine("vitu");
                Console.WriteLine(attname);
                using var con = new MySqlConnection(cs);
                con.Open();

                using var cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = $"Insert INTO attributes (size ,color, price, attname ) VALUES (" + '"' + size + '"' + ',' + '"' + color + '"' + ',' + '"' + price + '"' +',' + '"' + attname + '"' + ")";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(new { status = true, message = "successfully saved" });
        }
    }
}
