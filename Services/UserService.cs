using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroveStart.Dtos;
using GroveStart.Exceptions;
using GroveStart.Model;
using GroveStart.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GroveStart.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }
        public async  Task Delete(int id)
        {
           await _userRepository.DeleteByIdAsync(id);
            
        }

        public async Task<User[]> List()
        {
            var users = await _userRepository.GetAllAsync();
            return users.ToArray();
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
                throw new KeyNotFoundException("User not found");

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                _logger.LogWarning("Login: senha não definida no banco Email={Email}", email);
                throw new UnauthorizedAccessException("E-mail ou senha incorretos.");
            }

            PasswordVerificationResult verification;
            try
            {
                verification = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            }
            catch (FormatException ex)
            {
                // Hash armazenado não está no formato do Identity (ex.: texto plano ou outro algoritmo).
                _logger.LogWarning(
                    ex,
                    "Login: hash de senha inválido ou legado no banco Email={Email}. Recadastre ou atualize a senha.",
                    email);
                throw new UnauthorizedAccessException(
                    "Conta com formato de senha desatualizado. Use PUT para definir uma nova senha ou cadastre-se de novo.");
            }

            if (verification == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("E-mail ou senha incorretos.");

            return user;
        }

        public async Task<User?> Get(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> Register(User user)
        {
            _logger.LogInformation(
                "UserService.Register: iniciando cadastro Email={Email}, Name={Name}, Age={Age}",
                user.Email,
                user.Name,
                user.Age);
            try
            {
                if (await _userRepository.GetByEmailAsync(user.Email) is not null)
                {
                    _logger.LogWarning("UserService.Register: e-mail duplicado Email={Email}", user.Email);
                    throw new DuplicateEmailException(user.Email);
                }

                // Senha em texto vem do JSON; persistimos apenas o hash (PBKDF2 via Identity).
                var plainPassword = user.Password;
                var newUser = new User(user.Name, user.Email, user.Age, password: string.Empty);
                var hash = _passwordHasher.HashPassword(newUser, plainPassword);
                newUser.SetPasswordHash(hash);

                var created = await _userRepository.AddAsync(newUser);
                _logger.LogInformation(
                    "UserService.Register: usuário criado com Id={UserId} Email={Email}",
                    created.Id,
                    created.Email);
                return created;
            }
            catch (DuplicateEmailException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserService.Register falhou para Email={Email}", user.Email);
                throw;
            }
        }

        public async Task<User> Update(int id, UpdateUserRequest request)
        {
            var existingUser = await Get(id);
            if (existingUser == null)
                throw new KeyNotFoundException("user not Found");

            string? passwordHash = null;
            if (!string.IsNullOrWhiteSpace(request.Password))
                passwordHash = _passwordHasher.HashPassword(existingUser, request.Password);

            existingUser.Update(request.Name, request.Email, request.Age, passwordHash);
            await _userRepository.UpdateAsync(existingUser);
            return existingUser;
        }
    }
}