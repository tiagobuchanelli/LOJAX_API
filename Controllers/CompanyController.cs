using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lojax.Data;
using Lojax.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Lojax.Controllers
{
    [Route("v1/companies")]
    public class CompanyController : ControllerBase
    {

        //=======GET=======
        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Company>>> Get([FromServices] DataContext context)
        {


            var entities = await context
            .Companies
            .AsNoTracking()
            .ToListAsync();

            if (entities.Count == 0)
                return NotFound(new { message = "Nenhuma empresa encontrada." });

            return Ok(entities);
        }


        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Company>> GetByID(
            int id,
            [FromServices] DataContext context)
        {


            var entity = await context
            .Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return NotFound(new { message = "Empresa não encontrada" });


            return Ok(entity);

        }

        //=======POST=======
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Company>> Post(
            [FromBody] Company model,
            [FromServices] DataContext context)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
            try
            {
                //Valida o model
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                //Add Categoria
                model.CpnyUid = user;
                context.Companies.Add(model);

                //Salvar no banco e gerar ID
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception)
            {

                return BadRequest(new { message = "Não foi possível cadastrar uma empresa" });
            }
        }


        //=======PUT=======
        [HttpPut]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<Company>> Put(

            [FromBody] Company model,
            [FromServices] DataContext context)
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;

            var checkCompany = await context
                .Companies
                .AsNoTracking()
                .Where(x => x.CpnyUid == user)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (checkCompany == null)
                return NotFound(new { message = "Nenhuma empresa encontrada" });


            try
            {


                //valida ID da categoria
                // if (user != model.CpnyUid)
                //     return NotFound(new { message = "Usuario que esta tentando alterar é diferente do token" });


                //Valida o model
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                //Atualizar empresa
                model.CpnyUid = user;
                context.Entry<Company>(model).State = EntityState.Modified;

                //Salvar no banco 
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (DbUpdateConcurrencyException) //Verifica se existe um dado sendo atualizado ao mesmo tempo.
            {

                return BadRequest(new { message = "Esse registro já foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o usuário" });
            }
        }







    }
}