using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GroveStart.Model;

namespace GroveStart.Dtos.Orders
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage ="Numero/Nome do pedido.")]
        public string Title {get; set;} = string.Empty;

        [MinLength(15,ErrorMessage ="Descricao de no minimo 16 caracters.")]
        public string Description {get;set;} = string.Empty;

        [Required]
        [Range (1,9999,ErrorMessage ="Id do Customer;")]
        public int CustomerId {get;set;} 

        [Required]
        [Range (1,9999,ErrorMessage ="Id do Customer;")]
        public int UserId {get;set;}

        [Required]
        [Range(0,2)]
        public Period Period {get;set;}

         [Required]
         public DateTime StartDate { get; private set; }
         [Required]
         
        public DateTime EndDate { get; private set; }
    }
}