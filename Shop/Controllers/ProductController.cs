using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            //join
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            //join
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            //join
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id).ToListAsync();
            return product;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context, [FromBody] Product model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                context.Products.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Product>> Put(
            [FromServices] DataContext context, [FromBody] Product model, int id)
        {
            try
            {
                if (id != model.Id)
                    return NotFound(new { message = "Produto não encontrado" });


                if (ModelState.IsValid)
                {
                    context.Entry<Product>(model).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Ok(model);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Erro! Este registro ja foi alterado" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Nao foi possivel alterar registro" });
            }

        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult> Delete([FromServices] DataContext context, int id)
        {
            try
            {
                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product != null)
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    return Ok(new { message = "Produto excluido com sucesso" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Erro! Este registro foi alterado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Nao foi possivel alterar registro" });
            }
            return Ok();
        }

    }
}
