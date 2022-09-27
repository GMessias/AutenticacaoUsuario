using AutenticacaoUsuario.API.Dtos;
using AutenticacaoUsuario.Domain.Interfaces.Servicos;
using AutenticacaoUsuario.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutenticacaoUsuario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUsuarioController : ControllerBase
    {
        private readonly IServicoDeUsuario _servicoDeUsuario;
        private readonly IServicoDeSenha _servicoDeSenha;
        private readonly IServicoDeToken _servicoDeToken;
        private readonly IConfiguration _configuration;

        public AuthUsuarioController(IServicoDeUsuario servicoDeUsuario, IServicoDeSenha servicoDeSenha, IServicoDeToken servicoDeToken, IConfiguration configuration)
        {
            _servicoDeUsuario = servicoDeUsuario;
            _servicoDeSenha = servicoDeSenha;
            _servicoDeToken = servicoDeToken;
            _configuration = configuration;
        }

        [HttpPost("registro")]
        public IActionResult Registrar(DtoRegistroUsuario dtoRegistroUsuario)
        {
            if (_servicoDeUsuario.VerificarExistenciaDeUsuario(dtoRegistroUsuario.Email))
            {
                return BadRequest("Usuário já existe.");
            }

            var usuario = _servicoDeUsuario.RegistrarUsuarioComSenhaGerada(dtoRegistroUsuario.Nome, dtoRegistroUsuario.Email, dtoRegistroUsuario.Senha);
            _servicoDeUsuario.AdicionarUsuarioNoBancoDeDados(usuario);

            return Ok($"Usuário {usuario.Nome} criado com sucesso!");
        }

        [HttpPost("login")]
        public IActionResult Login(DtoLoginUsuario dtoLoginUsuario)
        {
            var usuario = _servicoDeUsuario.ConsultarUsuarioPorEmail(dtoLoginUsuario.Email);

            if (usuario == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            if (!_servicoDeSenha.VerificarSenhaHash(dtoLoginUsuario.Senha, usuario.SenhaHash, usuario.SenhaSalt))
            {
                return BadRequest("Senha incorreta.");
            }

            if (usuario.VerificadoEm == null)
            {
                return BadRequest("Usuário não verificado.");
            }

            string token = _servicoDeToken.CriarToken(usuario, _configuration);

            AtribuirNoCookieRefreshToken(usuario);
            _servicoDeUsuario.SalvarModificacoes(usuario);

            return Ok($"Bem vindo, {usuario.Nome}! Token do usuário: {token}");
        }

        [HttpPost("verifica")]
        public IActionResult VerificarUsuario(string token)
        {
            var usuario = _servicoDeUsuario.ConsultarUsuarioPorToken(token);

            if (usuario == null)
            {
                return BadRequest("Token inválido.");
            }

            _servicoDeUsuario.VerificarTokenDoUsuario(usuario);
            _servicoDeUsuario.SalvarModificacoes(usuario);

            return Ok("Usuário verificado com sucesso.");
        }

        [HttpPost("esqueci-a-senha")]
        public IActionResult RedefinirSenha(string email)
        {
            var usuario = _servicoDeUsuario.ConsultarUsuarioPorEmail(email);

            if (usuario == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            usuario.SenhaResetToken = _servicoDeToken.CriarTokenAleatorio();
            usuario.ResetTokenExpira = DateTime.Now.AddDays(1);
            _servicoDeUsuario.SalvarModificacoes(usuario);

            return Ok("Você pode redefinir sua senha.");
        }

        [HttpPost("reset-senha")]
        public IActionResult ResetarSenha(DtoRedefinirSenha dtoRedefinirSenha)
        {
            var usuario = _servicoDeUsuario.ConsultarUsuarioUtilizandoResetToken(dtoRedefinirSenha.Token);
            if (usuario == null || usuario.ResetTokenExpira < DateTime.Now)
            {
                return BadRequest("Token inválido.");
            }

            _servicoDeUsuario.FazerResetDaSenha(usuario, dtoRedefinirSenha.Senha);
            _servicoDeUsuario.SalvarModificacoes(usuario);

            return Ok("Senha resetado com sucesso.");
        }

        private void AtribuirNoCookieRefreshToken(Usuario usuario)
        {
            var novoRefreshToken = _servicoDeToken.GerarRefreshToken();
            var dtoRefreshToken = ConverterObjetoParaDto(novoRefreshToken);
            var cookieOptions = CriarCookieOptions(dtoRefreshToken);

            Response.Cookies.Append("refreshToken", dtoRefreshToken.Token, cookieOptions);
            _servicoDeUsuario.AtribuirRefreshTokenNoUsuario(novoRefreshToken, usuario);
        }

        private DtoRefreshToken ConverterObjetoParaDto(RefreshToken refreshToken)
        {
            var dtoRefreshToken = new DtoRefreshToken
            {
                Token = refreshToken.Token,
                Criado = refreshToken.Criado,
                Expira = refreshToken.Expira,
            };
            
            return dtoRefreshToken;
        }

        private CookieOptions CriarCookieOptions(DtoRefreshToken dtoRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = dtoRefreshToken.Expira
            };

            return cookieOptions;
        }
    }
}
