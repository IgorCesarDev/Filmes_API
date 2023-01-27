using FilmesFinal.Dtos.UsuarioDto;
using FilmesFinal.Models;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FilmesFinal.Data.Request;
using System;
using System.Linq;
using System.Web;

namespace FilmesFinal.Services
{
    public class CadastroService
    {
        private IMapper _mapper;
        private UserManager<CustomIdentityUser> _userManager;
        private EmailService _emailService;
        private RoleManager<IdentityRole<int>> _roleManager;

        public CadastroService(IMapper mapper, UserManager<CustomIdentityUser> userManager, EmailService emailService, RoleManager<IdentityRole<int>> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        public Result CadastraUsuario(CreateUsuarioDto createDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(createDto);
            CustomIdentityUser usuarioIdentity = _mapper.Map<CustomIdentityUser>(usuario);
            Task<IdentityResult> resultadoIdentity = _userManager.CreateAsync(usuarioIdentity, createDto.Password);
            _userManager.AddToRoleAsync(usuarioIdentity, "regular");
            if (resultadoIdentity.Result.Succeeded) 
            {
                var codigo = _userManager.GenerateEmailConfirmationTokenAsync(usuarioIdentity).Result;
                var encodedCode = HttpUtility.UrlEncode(codigo);
                _emailService.EnviarEmail(new[] { usuarioIdentity.Email }, "link de Ativação" , usuarioIdentity.Id, encodedCode);
                return Result.Ok().WithSuccess(codigo); 
            }
            return Result.Fail("Cadastro falhou");

        }

        public Result AtivaContaUsuario(AtivaContaRequest request)
        {
            var identityUser = _userManager.Users.FirstOrDefault(u => u.Id == request.UsuarioId);
            var identityResult = _userManager.ConfirmEmailAsync(identityUser, request.CodigoDeAtivacao).Result;
            if (identityResult.Succeeded)
            {
                return Result.Ok();
            }
            return Result.Fail("Falha ao ativar conta do usuario");
        }
    }
}
    