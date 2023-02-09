using FilmesFinal.Data;
using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Models;
using FilmesFinal.Services;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmesFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeService _filmeService;

        public FilmeController(FilmeService filmeService)
        {
           _filmeService = filmeService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            var dto = _filmeService.AdicionaFilme(filmeDto);
            return CreatedAtAction(nameof(RecuperaFilmesPorId), new { Id = dto.Id }, dto);
        }

        [HttpGet]
        public IActionResult RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 15)
        {
            var filmesDto = _filmeService.RecuperaFilmesAsync(skip, take);
            if (filmesDto == null) return NotFound();
            return Ok(filmesDto);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaFilmesPorId(int id)
        {
            var filmeDto = _filmeService.RecuperaFilmesPorIdAsync(id);
            if (filmeDto == null) { return NotFound(); }
            return Ok(filmeDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            var resultado = _filmeService.AtualizaFilme(id, filmeDto);
            if (resultado.IsFaulted) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeletaFilme(int id)
        {
            var resultado = _filmeService.DeletaFilme(id);
            if (resultado.IsFaulted) return NotFound();
            return NoContent();
        }
    }
}

