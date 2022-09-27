namespace AutenticacaoUsuario.Domain.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] SenhaHash { get; set; } = new byte[32];
        public byte[] SenhaSalt { get; set; } = new byte[32];
        public string? TokenDeVerificacao { get; set; }
        public DateTime? VerificadoEm { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCriado { get; set; }
        public DateTime TokenExpira { get; set; }
        public string? VerificacaoToken { get; set; }
        public string? SenhaResetToken { get; set; }
        public DateTime? ResetTokenExpira { get; set; }
    }
}
