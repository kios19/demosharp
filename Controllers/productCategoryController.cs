using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace restsharp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class productCategoryController : Controller
    {
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

            cmd.CommandText = $"Insert INTO categories (category_name) VALUES (" + '"' + name + '"' + ")";
            cmd.ExecuteNonQuery();
            return Ok(new { status = true, message = "successfully saved" });
        }

        
    }
}
