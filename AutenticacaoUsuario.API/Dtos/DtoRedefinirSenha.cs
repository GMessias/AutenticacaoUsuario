using System.ComponentModel.DataAnnotations;

namespace AutenticacaoUsuario.API.Dtos
{
    public class DtoRedefinirSenha
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, MinLength(8, ErrorMessage = "Por favor, precise ser no mínimo 8 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required, Compare("Senha")]
        public string SenhaDeConfirmacao { get; set; } = string.Empty;
    }
}
