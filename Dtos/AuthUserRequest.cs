using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroveStart.Dtos
{
    public class AuthUserRequest
    {
         [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Necessario senha")]
        [MinLength(6, ErrorMessage = "Necessario senha de 6 digitos")]
        public required string Password { get; set; }
    }
}