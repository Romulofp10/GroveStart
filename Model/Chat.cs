using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroveStart.Model
{
    public class Chat
    {   [Key]
          public int Id { get; set; }

        public int UserId { get; set; }
        public int CustomerId { get; set; }

        public int OrderId { get; set; }

        public Chat(int id, int userId, int customerId, int orderId)
        {
            Id = id;
            UserId = userId;
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}