using System.ComponentModel.DataAnnotations;

namespace GroveStart.Model
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int? CustomerId { get; private set; }

        public int? UserId { get; private set; }

        public int ChatId{ get; private set; }

        public string? Content { get; private set; }

        public string? Image { get; set; }


        /// <summary>Cliente do pedido (FK para <see cref="User"/> via <see cref="CustomerId"/>).</summary>
        public User? Customer { get; private set; }

        /// <summary>Usuário do pedido (FK para <see cref="User"/> via <see cref="UserId"/>).</summary>
        public User? User { get; private set; }


        public Message(int? userId, int? customerId, int chatId, string image, string content)
        {
            UserId = userId;
            CustomerId = customerId;
            ChatId = chatId;
            Content = content;
            Image = image;
        }
    }
}