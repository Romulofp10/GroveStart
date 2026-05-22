using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroveStart.Model;

namespace GroveStart.Services
{
    public interface ITokenService
    {
    string GenerateToken(User user);
    }
}