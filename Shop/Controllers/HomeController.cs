using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var employee = new User {  UserName = "robin", Password = "robin", Role = "employee" };
            var manager = new User {  UserName = "batman", Password = "batman", Role = "manager" };
            var category = new Category { Title = "Informatica" };
            var product = new Product {  Category = category , CategoryId = 1 , Descriptopn = "mouse",Title="mouse",Price= 10};
            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return Ok( new { message = "dados configurados" });
        }
    }
}
