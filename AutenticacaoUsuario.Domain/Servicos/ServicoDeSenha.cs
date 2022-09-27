using AutenticacaoUsuario.Domain.Interfaces.Servicos;
using System.Security.Cryptography;

namespace AutenticacaoUsuario.Domain.Servicos
{
    public class ServicoDeSenha : IServicoDeSenha
    {
        public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key;
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            }
        }

        public bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return computeHash.SequenceEqual(senhaHash);
            }
        }
    }
}
