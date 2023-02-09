using FilmesFinal.Data;
using FilmesFinal.Dtos.GeneroDto;
using FilmesFinal.Services;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FilmesFinal.Controllers
{
        [ApiController]
        [Route("[controller]")]
    public class GeneroController : ControllerBase
    {
        private GeneroService _generoService;

        public GeneroController(GeneroService generoService)
        {
            _generoService = generoService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AdicionaGenero([FromBody] CreateGeneroDto generoDto)
        {
            var genero = _generoService.AdicionaGenero(generoDto);
            return CreatedAtAction(nameof(RecuperaGenerosPorId), new { Id = genero.Id }, genero);
        }

        [HttpGet]
        public IActionResult RecuperaGeneros()
        {
            var generos = _generoService.RecuperaGeneros();
            if (generos == null) return NotFound();
            return Ok(generos);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaGenerosPorId(int id) 
        {
            var generodto = _generoService.RecuperaGenerosPorId(id);
            if (generodto == null) return NotFound();
            return Ok(generodto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult AtualizaGeneros(int id, [FromBody] UpdateGeneroDto generoDto)
        {
            var resultado = _generoService.AtualizaGeneros(id, generoDto);
            if (resultado.IsFaulted) return NotFound();
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeletaFilme(int id)
        {
            var resultado = _generoService.DeletaGenero(id);
            if (resultado.IsFaulted) return NotFound();
            return Ok(resultado);
        }
    }
}
