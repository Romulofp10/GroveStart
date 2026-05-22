using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroveStart.Model
{
    public enum Period
    {
        Diario,
        Semanal,
        Mensal
    }

    public class Order
    {
        [Key]
        public int Id { get; private set; }

        public int UserId { get; private set; }
        public int CustomerId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public User User { get; private set; } = null!;
        public User Customer { get; private set; } = null!;

        public Chat? Chat { get; private set; }

        public Period Period { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        

        public Order(int userId, string title, string description, Period period, DateTime startDate, DateTime endDate, int customerId)
        {
            UserId = userId;
            CustomerId = customerId;
            Title = title;
            Description = description;
            Period = period;
            StartDate = startDate;
            EndDate = endDate;
        }

        // Método para atualizar os dados
        public void Update(int userId, int customerId,string title, string description, Period period, DateTime startDate, DateTime endDate)
        {
            UserId = userId;
            CustomerId = customerId;
            Title = title;
            Description = description;
            Period = period;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}