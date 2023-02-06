using FilmesFinal.Data.Request;
using FilmesFinal.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace FilmesFinal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private LoginService _loginService;
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult LogaUsuario(LoginRequest request)
        {
           Result resultado = _loginService.LogaUsuario(request);
            if (resultado.IsFailed) return Unauthorized(resultado.Reasons);
            return Ok(resultado.Reasons);
        }

        [HttpPost("/solicita-reset")]
        public IActionResult SoliticaResetSenhaUsuario(SolicitaResetRequest request)
        {
            Result resultado = _loginService.SoliticaResetSenhaUsuario(request);
            if (resultado.IsFailed) return Unauthorized(resultado.Reasons);
            return Ok(resultado.Reasons);
        }

        [HttpPost("/efetua-reset")]
        public IActionResult ResetaSenhaUsuario(EfetuaResetRequest request)
        {
            Result resultado = _loginService.ResetaSenhaUsuario(request);
            if (resultado.IsFailed) return Unauthorized(resultado.Reasons);
            return Ok(resultado.Reasons);
        }
    }
}
