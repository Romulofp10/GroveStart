using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroveStart.Model;
using GroveStart.Repository;

namespace GroveStart.Services
{
    public class UserService : IUserService
    {
          private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task <User> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task <User[]> List()
        {
            throw new NotImplementedException();
        }

        public Task<User> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Register(User user)
        {
            try
            {
                return await _userRepository.AddAsync(user); 
            }
            catch (System.Exception error)
            {
                Console.WriteLine("Error", error);
                throw;
            }
           
        }

        public async  Task Update(User user)
        {
             _userRepository.Update(user);
        }
    }
}