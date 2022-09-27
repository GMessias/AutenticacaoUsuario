using AutenticacaoUsuario.Domain.Interfaces.Repositorios;
using AutenticacaoUsuario.Domain.Interfaces.Servicos;
using AutenticacaoUsuario.Domain.Models;

namespace AutenticacaoUsuario.Domain.Servicos
{
    public class ServicoDeUsuario : IServicoDeUsuario
    {
        private readonly IRepositorioDeUsuario _repositorioDeUsuario;
        private readonly IServicoDeSenha _servicoDeSenha;
        private readonly IServicoDeToken _servicoDeToken;

        public ServicoDeUsuario(IRepositorioDeUsuario repositorioDeUsuario, IServicoDeSenha servicoDeSenha, IServicoDeToken servicoDeToken)
        {
            _repositorioDeUsuario = repositorioDeUsuario;
            _servicoDeSenha = servicoDeSenha;
            _servicoDeToken = servicoDeToken;
        }

        public bool VerificarExistenciaDeUsuario(string email)
        {
            return _repositorioDeUsuario.UsuarioCadastrado(email);
        }

        public Usuario RegistrarUsuarioComSenhaGerada(string nome, string email, string senha)
        {
            var usuario = CriarUsuarioPrivado();
            usuario.Nome = nome;
            usuario.Email = email;
            _servicoDeSenha.CriarSenhaHash(senha, out byte[] senhaHash, out byte[] senhaSalt);
            usuario.SenhaHash = senhaHash;
            usuario.SenhaSalt = senhaSalt;
            usuario.TokenDeVerificacao = _servicoDeToken.CriarTokenAleatorio();

            return usuario;
        }

        public void AdicionarUsuarioNoBancoDeDados(Usuario usuario)
        {
            _repositorioDeUsuario.CadastrarUsuario(usuario);
        }

        public Usuario ConsultarUsuarioPorEmail(string email)
        {
            return _repositorioDeUsuario.ConsultarUsuarioComEmail(email);
        }

        public void AtribuirRefreshTokenNoUsuario(RefreshToken refreshToken, Usuario usuario)
        {
            usuario.RefreshToken = refreshToken.Token;
            usuario.TokenCriado = refreshToken.Criado;
            usuario.TokenExpira = refreshToken.Expira;
        }

        public void SalvarModificacoes(Usuario usuario)
        {
            _repositorioDeUsuario.AtualizarModificacoes(usuario);
        }

        public Usuario ConsultarUsuarioPorToken(string token)
        {
            return _repositorioDeUsuario.ConsultarUsuarioPorToken(token);
        }

        public void VerificarTokenDoUsuario(Usuario usuario)
        {
            usuario.VerificadoEm = DateTime.Now;
        }

        public Usuario ConsultarUsuarioUtilizandoResetToken(string resetToken)
        {
            return _repositorioDeUsuario.ConsultarUsuarioPorResetToken(resetToken);
        }

        public void FazerResetDaSenha(Usuario usuario, string resetSenha)
        {
            _servicoDeSenha.CriarSenhaHash(resetSenha, out byte[] senhaHash, out byte[] senhaSalt);

            usuario.SenhaHash = senhaHash;
            usuario.SenhaSalt = senhaSalt;
            usuario.SenhaResetToken = null;
            usuario.ResetTokenExpira = null;
        }

        private Usuario CriarUsuarioPrivado()
        {
            return new Usuario();
        }
    }
}
