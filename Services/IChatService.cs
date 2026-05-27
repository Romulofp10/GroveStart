using System.Data.Common;
using GroveStart.Model;

namespace GroveStart.Services
{
    public interface IChatService
    {
        public Task<Chat> Create(Chat chat);
    }
}
 