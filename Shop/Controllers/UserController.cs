using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            return await context.Users.AsNoTracking().ToListAsync();
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                model.Role = "employee";
                context.Users.Add(model);
                await context.SaveChangesAsync();
                model.Password = "";
                return Ok( model);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Não foi possivel " });
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound(new { message = "Usuario / Senha invalidos" });
            }
            var token = TokenService.GenerateToken(user);
            model.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }


        [HttpPut]
        [Route("{id::int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put([FromBody] User model, [FromServices] DataContext context , int id)
        {
            if (id != model.Id)
                return NotFound(new { message = "Usuario não encontrado" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Entry<User>(model).State = EntityState.Modified;
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
        [Route("{id::int}")]
        [Authorize (Roles = "manager")]
        public async Task<ActionResult> Delete([FromServices] DataContext context, int id)
        {
            try {
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                }
                else {
                    return NotFound(new { message = "User não encontrado" });
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
