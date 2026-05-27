using System.Runtime.InteropServices;
using GroveStart.Model;
using GroveStart.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GroveStart.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        ChatService(IChatRepository chatRepository) {
            _chatRepository = chatRepository;
        }

        public async Task<Chat> Create(Chat chat)
        {
            var newChat = new Chat(chat.UserId, chat.CustomerId, chat.OrderId);
           return await _chatRepository.Create(newChat);
        }
    }
}
 