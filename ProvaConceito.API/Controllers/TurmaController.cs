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
    public class TurmaController : ControllerBase
    {
        private readonly TurmaService _turmaService;
        public TurmaController(TurmaService turmaService)
        {
            _turmaService = turmaService;
        }

        // GET: api/<TurmaController>
        
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<List<Turma>>> Get() =>
            await _turmaService.GetAll();

        [HttpGet("withchildren")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<List<Turma>>> GetWithChildren() =>
                    await _turmaService.GetAllWithChildren();

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<Turma>> GetById(int? id)
        {
            if(id == null)
            {
                return BadRequest("O código da turma é inválido");
            }
            Turma turma = null;
            turma = await _turmaService.GetById(id);
            if(turma == null)
            {
                return NotFound();
            }

            return Ok(turma);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest("O código da turma é inválido");
            }

            bool isDeleted = await _turmaService.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Turma>> Create(Turma newTurma)
        {
            var turma = await _turmaService.Create(newTurma);

            return CreatedAtAction(nameof(GetById), new { id = turma.TurmaId }, turma);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Turma newTurma)
        {
            bool isUpdated = await _turmaService.Update(newTurma);

            if (!isUpdated)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetById), new { id = newTurma.TurmaId }, newTurma);
        }
    }
}
