using GroveStart.Model;

namespace GroveStart.Repository
{
    public interface IChatRepository
    {
        public Task<Chat> Create(Chat chat);
    }
}