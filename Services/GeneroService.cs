using FilmesFinal.Data;
using FilmesFinal.Dtos.GeneroDto;
using FilmesFinal.Models;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FilmesFinal.Services
{
    public class GeneroService
    {
        private UserDbContext _context;
        private IMapper _mapper;

        public GeneroService(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ReadGeneroDto> RecuperaGeneros()
        {
            List<Genero> generos = _context.Generos.Include(p => p.Filmes).ToList();
            List<ReadGeneroDto> generosDto =_mapper.Map<List<ReadGeneroDto>>(generos);
            return generosDto;
        }

        public ReadGeneroDto AdicionaGenero(CreateGeneroDto generoDto)
        {
            Genero genero = _mapper.Map<Genero>(generoDto);
            _context.Generos.Add(genero);
            _context.SaveChanges();
            return _mapper.Map<ReadGeneroDto>(genero);
        }

        public ReadGeneroDto RecuperaGenerosPorId(int id)
        {
            Genero genero = _context.Generos.Include(f => f.Filmes).FirstOrDefault(g => g.Id == id);
            if (genero == null) { return null; }
            return _mapper.Map<ReadGeneroDto>(genero);
        }

        public Result AtualizaGeneros(int id, UpdateGeneroDto generoDto)
        {
            Genero genero = _context.Generos.FirstOrDefault(g => g.Id == id);
            if (genero == null) { return Result.Fail("Erro, Genero não encontrado"); }
            _mapper.Map(generoDto, genero);
            _context.SaveChanges();
            return Result.Ok();
        }

        public Result DeletaGenero(int id)
        {
            Genero genero = _context.Generos.FirstOrDefault(g => g.Id==id);
            if (genero == null) { return Result.Fail("Erro, Genero não encontrado"); }
            _context.Generos.Remove(genero);
            _context.SaveChanges();
            return Result.Ok();

        }
    }
}
