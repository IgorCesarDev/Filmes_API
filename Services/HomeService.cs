using AutoMapper;
using FilmesFinal.Data;
using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Models;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FilmesFinal.Services
{
    public class HomeService
    {
        private UserDbContext _context;
        private IMapper _mapper;
        private UserManager<CustomIdentityUser> _userManager;
        private const string HOMEff_KEY = "FilmesFavoritos";
        private const string HOME_KEY = "Filmes";
        private IMemoryCache _memoryCache;
        MemoryCacheEntryOptions memoryCachEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
            SlidingExpiration = TimeSpan.FromSeconds(1200)
        };
        public HomeService(UserDbContext context, IMapper mapper, UserManager<CustomIdentityUser> userManager
            , IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _memoryCache = memoryCache;
        }

        public async Task<List<ReadFilmeDto>> RecuperaFilmesFavoritos(int userId)
        {      
            if(_memoryCache.TryGetValue(HOMEff_KEY, out List<Filme> filmesFavorios))
            {
                List<ReadFilmeDto> filmesFavoriosDto = _mapper.Map<List<ReadFilmeDto>>(filmesFavorios);
                return filmesFavoriosDto;
            }
            CustomIdentityUser usuario = await _context.Users
                     .Include(f => f.FilmesFavoritos)
                        .ThenInclude(g => g.Generos)
                            .FirstOrDefaultAsync(i => i.Id == userId);
            filmesFavorios = usuario.FilmesFavoritos.ToList();
            if(filmesFavorios != null) 
            {
                _memoryCache.Set(HOMEff_KEY,filmesFavorios, memoryCachEntryOptions);
                List<ReadFilmeDto> filmesFavoriosDto = _mapper.Map<List<ReadFilmeDto>>(filmesFavorios);
                return filmesFavoriosDto;
            }
            return null;
        }

        public async Task<List<ReadFilmeDto>> RecomendacoesFilmesPorIdade(string nascimentoUsuario)
        {
            int idade = CalcularIdade(nascimentoUsuario);
            if (_memoryCache.TryGetValue(HOME_KEY, out List<Filme> filme))
            {
                List<Filme> listaDeFilmesCache = BuscaFilmePorIdade(idade, filme);
                List<ReadFilmeDto> filmesDtoCache = _mapper.Map<List<ReadFilmeDto>>(listaDeFilmesCache);
                return filmesDtoCache;
            }
            List<Filme> filmes = await _context.Filmes.Include(f => f.Generos).ToListAsync();
            _memoryCache.Set(HOME_KEY, filmes, memoryCachEntryOptions);

            List<Filme> listaDeFilmes = BuscaFilmePorIdade(idade, filmes);
            List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(listaDeFilmes);
            return filmesDto;
        }

        public async Task<ReadFilmeDto> AdicinaFilmeFavorito(int id, string usuarioId)
        {
            var usuario = _userManager.FindByIdAsync(usuarioId).Result;
            Filme filme = await _context.Filmes.Include(f => f.Usuarios).Include(g => g.Generos)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (filme == null) { return null; }
            filme.Usuarios.Add(usuario);
            await AtualizaCacheFilmesFavoritos();
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadFilmeDto>(filme);
        }

        public async Task<Result> DeletaFilmeFavorito(int id, string usuarioId)
        {
            var usuario = _userManager.FindByIdAsync(usuarioId).Result;
            Filme filme = await _context.Filmes.Include(f => f.Usuarios).FirstOrDefaultAsync(i => i.Id == id);
            if (filme == null) { return Result.Fail("Erro, Filme não encontrado"); }
            filme.Usuarios.Remove(usuario);
            await AtualizaCacheFilmesFavoritos();
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<List<ReadFilmeDto>> RecuperaFilmesSemFavoritos(int userId)
        {
            CustomIdentityUser usuarioOk = await RecuperaUsuario(userId);
            List<Filme> filmesFavoritos = usuarioOk.FilmesFavoritos.ToList();
            List<Filme> filmes = await _context.Filmes.Include(g => g.Generos).ToListAsync();
            List<Filme> filmeEnd = filmes.Where(e => !filmesFavoritos.Any(f => f.Id == e.Id)).ToList();

            if (filmeEnd != null)
            {
                List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(filmeEnd.Take(15));
                return filmesDto;
            }
            return null;
        }

        private async Task<CustomIdentityUser> RecuperaUsuario(int userId)
        {
            return await _context.Users
                                 .Include(f => f.FilmesFavoritos)
                                    .ThenInclude(g => g.Generos)
                                        .FirstOrDefaultAsync(i => i.Id == userId);
        }

        private static int CalcularIdade(string nascimentoUsuario)
        {
            DateTime dateUser = DateTime.Parse(nascimentoUsuario);
            DateTime dataAtual = DateTime.Now;
            int idade = dataAtual.Year - dateUser.Year;
            return idade;
        }
        private static List<Filme> BuscaFilmePorIdade(int idade, List<Filme> filmes)
        {
            int faixaEtaria = idade <= 10 ? 10 : (idade >= 18 ? 18 : 15);
            List<Filme> filmesIdade = filmes.Where(f => f.FaixaEtaria == faixaEtaria).Take(15).ToList();
            return filmesIdade;
        }
        private async Task AtualizaCacheFilmesFavoritos()
        {
            var updateValue = await _context.Filmes.Include(f => f.Usuarios).Include(f => f.Generos).
                ToListAsync();
            _memoryCache.Remove(HOMEff_KEY);
            _memoryCache.Set(HOME_KEY, updateValue, memoryCachEntryOptions);
        }
    }

}

