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
            ReadFilmeDto dto = _filmeService.AdicionaFilme(filmeDto);
            return CreatedAtAction(nameof(RecuperaFilmesPorId), new {Id = dto.Id}, dto);
        }
        [HttpGet]
        public IActionResult RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 15, [FromQuery]bool genero=true) 
        {
            List<ReadFilmeDto> filmesDto = _filmeService.RecuperaFilmes(skip, take, genero);
            if (filmesDto == null) return NotFound();
            return Ok(filmesDto);
        }
        [HttpGet("{id}")]
        public IActionResult RecuperaFilmesPorId(int id)
        {
            ReadFilmeDto filmeDto = _filmeService.RecuperaFilmesPorId(id);
            if (filmeDto == null) { return NotFound(); }
            return Ok(filmeDto);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            Result resultado = _filmeService.AtualizaFilme(id, filmeDto);
            if (resultado.IsFailed) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeletaFilme(int id) 
        {
            Result resultado = _filmeService.DeletaFilme(id);
            if (resultado.IsFailed) return NotFound();
            return NoContent();

        }
    }
}

