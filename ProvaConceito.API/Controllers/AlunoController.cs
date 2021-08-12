using Microsoft.AspNetCore.Mvc;
using ProvaConceito.DataAccess.Services;
using ProvaConceito.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProvaConceito.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly AlunoService _alunoService;
        public AlunoController(AlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        // GET: api/<AlunoController>
        
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<List<Aluno>>> Get() =>
            await _alunoService.GetAll();

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<Aluno>> GetById(int? id)
        {
            if(id == null)
            {
                return BadRequest("O código da aluno é inválido");
            }
            Aluno aluno = null;
            aluno = await _alunoService.GetById(id);
            if(aluno == null)
            {
                return NotFound();
            }

            return Ok(aluno);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest("O código da aluno é inválido");
            }

            bool isDeleted = await _alunoService.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Aluno>> Create(Aluno newAluno)
        {
            var aluno = await _alunoService.Create(newAluno);

            return CreatedAtAction(nameof(GetById), new { id = aluno.AlunoId }, aluno);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Aluno newAluno)
        {
            bool isUpdated = await _alunoService.Update(newAluno);

            if (!isUpdated)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetById), new { id = newAluno.AlunoId }, newAluno);
        }
    }
}
