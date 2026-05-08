using System.ComponentModel.DataAnnotations;

namespace GroveStart.Dtos
{
    /// <summary>
    /// Corpo da requisição PUT /api/users/{id}. A senha é opcional; se omitida ou null, a senha atual permanece.
    /// </summary>
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, 150, ErrorMessage = "Idade deve estar entre 1 e 150.")]
        public int Age { get; set; }

        /// <summary>Nova senha; opcional. Se não enviada, o campo pode ser omitido no JSON.</summary>
        public string? Password { get; set; }
    }
}
