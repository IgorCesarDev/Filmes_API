using AutoMapper;
using FilmesFinal.Data;
using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace FilmesFinal.Services
{
    public class HomeService
    {
        private UserDbContext _context;
        private IMapper _mapper;
        private UserManager<CustomIdentityUser> _userManager;
        public HomeService(UserDbContext context, IMapper mapper, UserManager<CustomIdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public List<ReadFilmeDto> RecuperaFilmesPorUsuario(int userId)
        {      
            CustomIdentityUser usuarioOk = _context.Users
                     .Include(f => f.FilmesFavoritos)
                        .ThenInclude(g => g.Generos)
                            .FirstOrDefault(i => i.Id == userId);
            List<Filme> filmes = usuarioOk.FilmesFavoritos.ToList();
            if(filmes != null) 
            {
                List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(filmes);
                return filmesDto;
            }
            return null;
        }

        public  List<ReadFilmeDto> RecomendacoesFilmesPorIdade(string nascimentoUsuario)
        {
            int idade = CalcularIdade(nascimentoUsuario);
            List<Filme> listaDeFilmes = null;
            if (idade <= 10) { listaDeFilmes = _context.Filmes.Where(a => a.FaixaEtaria == 10)
                    .Include(g => g.Generos).ToList(); }
            if (idade == 15 & idade > 10 & idade < 18) { listaDeFilmes = _context.Filmes.Where(a => a.FaixaEtaria == 15)
                    .Include(g => g.Generos).ToList(); }
            if (idade >= 18) { listaDeFilmes = _context.Filmes.Where(a => a.FaixaEtaria == 18)
                    .Include(g => g.Generos).ToList(); }
            List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(listaDeFilmes);
            return filmesDto;
        }
        public ReadFilmeDto AdicinaFilmeFavorito(int id, string usuarioId)
        {
            var usuario = _userManager.FindByIdAsync(usuarioId).Result;
            Filme filme = _context.Filmes.Include(f => f.Usuarios).Include(g => g.Generos).FirstOrDefault(i => i.Id == id);
            if (filme == null) { return null; }
            filme.Usuarios.Add(usuario);
            _context.SaveChanges();
            return _mapper.Map<ReadFilmeDto>(filme);
        }
        public Result DeletaFilmeDeUsuario(int id, string usuarioId)
        {
            Filme filme = _context.Filmes.Include(f => f.Usuarios).FirstOrDefault(i => i.Id == id);
            if (filme == null) { return Result.Fail("Erro, Filme não encontrado"); }
            var usuario = _userManager.FindByIdAsync(usuarioId).Result;
            filme.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return Result.Ok();


        }
        private static int CalcularIdade(string nascimentoUsuario)
        {
            DateTime dateUser = DateTime.Parse(nascimentoUsuario);
            DateTime dataAtual = DateTime.Now;
            int idade = dataAtual.Year - dateUser.Year;
            return idade;
        }

        public List<ReadFilmeDto> RecuperaFilmes(int userId)
        {
            CustomIdentityUser usuarioOk = _context.Users
                     .Include(f => f.FilmesFavoritos)
                        .ThenInclude(g => g.Generos)
                            .FirstOrDefault(i => i.Id == userId);
            List<Filme> filmesUser = usuarioOk.FilmesFavoritos.ToList();
            List<Filme> filmes = _context.Filmes.Include(g => g.Generos).ToList();
            List<Filme> filmeEnd = filmes.Where(e => !filmesUser.Any(f =>f.Id == e.Id)).ToList();
            if (filmeEnd != null)
            {
                List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(filmeEnd);
                return filmesDto;
            }
            return null;
        }
    }

}

