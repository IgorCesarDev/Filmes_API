using FilmesFinal.Data;
using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Models;
using FilmesFinal.Services;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Claims;

namespace FilmesFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private HomeService _homeService;

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        [Authorize(Roles = "regular")]
        public IActionResult RecuperaFilmesParaVer()
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int intUserId = int.Parse(usuarioId);
            var filmesDto = _homeService.RecuperaFilmesParaVer(intUserId);
            if(filmesDto == null) { return NotFound(); }
            return Ok(filmesDto);
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "regular")]
        public IActionResult AdicinaFilmeFavorito(int id)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var filmeDto = _homeService.AdicinaFilmeFavorito(id, usuarioId);
            if (filmeDto == null) { return NotFound(); }
            return Ok(filmeDto);
        }

        [HttpGet]
        [Route("filmes-favoritos")]
        [Authorize(Roles = "regular")]
        public IActionResult RecuperaFilmesFavoritos() 
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int intUserId = int.Parse(usuarioId);
            var filmesDto = _homeService.RecuperaFilmesFavoritos(intUserId);
            return Ok(filmesDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "regular")]
        public IActionResult DeletaFilmeFavorito(int id)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var resultado = _homeService.DeletaFilmeFavorito(id, usuarioId);
            if (resultado.IsFaulted) return NotFound();
            return NoContent();
        }

        [HttpGet]
        [Route("recomendacoes")]
        [Authorize(Roles = "regular")]
        public IActionResult RecomendacoesFilmesPorIdade()
        {
            var nascimentoUsuario = User.FindFirst(ClaimTypes.DateOfBirth).Value;
           var filmesDto = _homeService.RecomendacoesFilmesPorIdade(nascimentoUsuario);
            return Ok(filmesDto);
        }
    }
}
