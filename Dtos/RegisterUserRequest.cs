using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.MicrosoftExtensions;

namespace GroveStart.Dtos
{
    public class RegisterUserRequest
    {
         [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Range(1, 150, ErrorMessage = "Idade deve estar entre 1 e 150.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Necessario senha")]
        [MinLength(6, ErrorMessage = "Necessario senha de 6 digitos")]
        public required string Password { get; set; }
    }
}