using AutenticacaoUsuario.Domain.Interfaces.Repositorios;
using AutenticacaoUsuario.Domain.Models;
using AutenticacaoUsuario.Infrastructure.Data;

namespace AutenticacaoUsuario.Infrastructure.Repositorios
{
    public class RepositorioDeUsuario : IRepositorioDeUsuario
    {
        private readonly UsuarioDataContext _context;

        public RepositorioDeUsuario(UsuarioDataContext context)
        {
            _context = context;
        }

        public bool UsuarioCadastrado(string email)
        {
            return _context.Usuarios.Any(u => u.Email == email);
        }

        public async void CadastrarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public Usuario ConsultarUsuarioComEmail(string email)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public async void AtualizarModificacoes(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public Usuario ConsultarUsuarioPorToken(string token)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return _context.Usuarios.FirstOrDefault(u => u.VerificacaoToken == token);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public Usuario ConsultarUsuarioPorResetToken(string resetToken)
        {
            #pragma warning disable CS8603 // Possible null reference return.
            return _context.Usuarios.FirstOrDefault(u => u.SenhaResetToken == resetToken);
            #pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
