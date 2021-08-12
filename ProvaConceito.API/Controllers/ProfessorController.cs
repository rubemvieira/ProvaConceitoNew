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
    public class ProfessorController : ControllerBase
    {
        private readonly ProfessorService _professorService;
        public ProfessorController(ProfessorService professorService)
        {
            _professorService = professorService;
        }

        // GET: api/<ProfessorController>
        
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<List<Professor>>> Get() =>
            await _professorService.GetAll();

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<Professor>> GetById(int? id)
        {
            if(id == null)
            {
                return BadRequest("O código da professor é inválido");
            }
            Professor professor = null;
            professor = await _professorService.GetById(id);
            if(professor == null)
            {
                return NotFound();
            }

            return Ok(professor);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest("O código da professor é inválido");
            }

            bool isDeleted = await _professorService.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Professor>> Create([FromBody] Professor newProfessor)
        {
            var professor = await _professorService.Create(newProfessor);

            return CreatedAtAction(nameof(GetById), new { id = professor.ProfessorId }, professor);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Professor newProfessor)
        {
            bool isUpdated = await _professorService.Update(newProfessor);

            if (!isUpdated)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetById), new { id = newProfessor.ProfessorId }, newProfessor);
        }
    }
}
