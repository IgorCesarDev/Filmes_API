using FilmesFinal.Data;
using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Models;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FilmesFinal.Services
{
    public class FilmeService
    {
        private UserDbContext _context;
        private IMapper _mapper;

        public FilmeService(UserDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public ReadFilmeDto AdicionaFilme(CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return _mapper.Map<ReadFilmeDto>(filme);
        }
        public List<ReadFilmeDto> RecuperaFilmes(int skip, int take, bool genero)
        {
            List<Filme> filmes;
            if (genero) { filmes = _context.Filmes.Include(p => p.Generos).Skip(skip).Take(take).ToList(); }
            else
            {
                 filmes = _context.Filmes.Skip(skip).Take(take).ToList();
            }
            if(filmes != null)
            {
                List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(filmes);
                return filmesDto;
            }
            return null;
        }
        public ReadFilmeDto RecuperaFilmesPorId(int id)
        {
            Filme filme = _context.Filmes.Include(p => p.Generos).FirstOrDefault(p => p.Id == id);
            if (filme != null)
            { 
                ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme); 
                return filmeDto;
            }
            return null;
        }
        public Result AtualizaFilme(int id, UpdateFilmeDto filmeDto)
        {
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null)
            {
                return Result.Fail("Erro, Filme não encontrado");
            }
            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();
            return Result.Ok();
        }
        public Result DeletaFilme(int id)
        {
            Filme filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
            if(filme == null) { return Result.Fail("Erro, Filme não encontrado"); }
            _context.Filmes.Remove(filme);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}
