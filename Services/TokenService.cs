using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GroveStart.Model;
using Microsoft.IdentityModel.Tokens;

namespace GroveStart.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // Mesmos valores que o Program.cs usa para VALIDAR o token depois.
            var issuer = _configuration["JwtIssuer"]!;
            var audience = _configuration["JwtAudience"]!;
            var key = _configuration["JwtKey"]!;

            // Dados que vão DENTRO do token (quem é o usuário).
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Chave para ASSINAR o token (prova que foi sua API que gerou).
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Monta o JWT (objeto) — parâmetros explicados abaixo no guia.
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            // Transforma o objeto em string (o que o cliente envia no header).
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
