using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroveStart.Model;

namespace GroveStart.Services
{
    public interface  IUserService
    {
        public  Task<User> Login(string email, string password);

        public  Task<User> Register(User user);

        public Task Update(User user);

        public Task <User[]> List();

        public Task <User> Get(int id);

        public Task Delete(int id);
         
    }
}