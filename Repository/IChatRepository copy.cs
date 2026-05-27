using GroveStart.Infra;
using GroveStart.Model;
using Microsoft.EntityFrameworkCore;

namespace GroveStart.Repository
{
    public class ChatRepository : IChatRepository
    {
          private readonly ConnectionContext _context;
        private readonly DbSet<Chat> _dbSet;

        public ChatRepository(ConnectionContext context)
        {
              _context = context;
            _dbSet = _context.Set<Chat>();
        }
        public async Task<Chat> Create(Chat chat)
        {
            await _dbSet.AddAsync(chat);
            await _context.SaveChangesAsync();
            return chat;
        }
    }
}