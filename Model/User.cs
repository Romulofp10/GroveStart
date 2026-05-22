using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GroveStart.Model
{

    public enum Role
    {
        Admin,
        Client,
        User

    }
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Role Role {get;private set;}

        public int Age { get; private set; }

        [JsonIgnore]
        public string Password {get;  private set;}

        public DateTime CreatedAt {get; private set;}
        public DateTime? UpdatedAt {get; private set;}

        public DateTime? DeletedAt {get; private set;}

        [JsonConstructor]
        public User(string name, string email, int age, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            Age = age;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>Define o hash persistido (resultado de IPasswordHasher). Usado após HashPassword no cadastro.</summary>
        public void SetPasswordHash(string passwordHash)
        {
            Password = passwordHash;
        }

        /// <param name="passwordHash">Se informado, substitui por novo hash já calculado; caso contrário mantém a senha atual.</param>
        public void Update(string name, string email, int age, string? passwordHash = null)
        {
            Name = name;
            Email = email;
            Age = age;
            UpdatedAt = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(passwordHash))
                Password = passwordHash;
        }
    }
}