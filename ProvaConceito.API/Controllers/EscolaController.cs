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
    public class EscolaController : ControllerBase
    {
        private readonly EscolaService _escolaService;
        public EscolaController(EscolaService escolaService)
        {
            _escolaService = escolaService;
        }

        // GET: api/<EscolaController>
        
        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<List<Escola>>> Get() =>
            await _escolaService.GetAll();

        [HttpGet("{id}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<Escola>> GetById(int? id)
        {
            if(id == null)
            {
                return BadRequest("O código da escola é inválido");
            }
            Escola escola = null;
            escola = await _escolaService.GetById(id);
            if(escola == null)
            {
                return NotFound();
            }

            return Ok(escola);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return BadRequest("O código da escola é inválido");
            }

            bool isDeleted = await _escolaService.Delete(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Escola>> Create(Escola newEscola)
        {
            var escola = await _escolaService.Create(newEscola);

            return CreatedAtAction(nameof(GetById), new { id = escola.EscolaId }, escola);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Escola newEscola)
        {
            bool isUpdated = await _escolaService.Update(newEscola);

            if (!isUpdated)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetById), new { id = newEscola.EscolaId }, newEscola);
        }
    }
}
