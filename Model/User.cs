using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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

        public string Password {get;  private set;}

        public DateTime CreatedAt {get; private set;}
        public DateTime? UpdatedAt {get; private set;}

        public DateTime? DeletedAt {get; private set;}

        public User(string name, string email, int age, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            Age = age;
            CreatedAt = DateTime.Now;
        }
    }


}