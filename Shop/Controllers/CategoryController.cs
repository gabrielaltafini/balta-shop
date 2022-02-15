﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Shop.Controllers
{
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        
        //para não usar cash se o responde cash estiver ativo
        //[ResponseCache(Duration = 0 , Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            return Ok(await (context.Categories.AsNoTracking().ToListAsync()));
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(int id,[FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound("Categoria não encontrada!");

            return Ok(category);

        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles ="employee")]
        public async Task<ActionResult<Category>> Post([FromBody]Category model,
            [FromServices] DataContext context)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                context.Categories.Add(model);
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
        public async Task<ActionResult<Category>> Put(int id 
            ,[FromBody] Category model
            ,[FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { message = "Categoria não encontrada!" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
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
        [Authorize(Roles = "employee")]
        public async Task<ActionResult> Delete(int id , [FromServices] DataContext context)
        {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound("Categoria não encontrada!");

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok("Categoria removida com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao excluir categoria" });
            }
        }
    }
}
