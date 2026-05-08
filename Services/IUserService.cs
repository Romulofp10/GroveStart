using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroveStart.Dtos;
using GroveStart.Model;

namespace GroveStart.Services
{
    public interface  IUserService
    {
        public  Task<User> Login(string email, string password);

        public  Task<User> Register(User user);

        public Task<User> Update(int id, UpdateUserRequest request);

        public Task <User[]> List();

        public Task <User?> Get(int id);

        public Task Delete(int id);
         
    }
}