using System.ComponentModel.DataAnnotations;
using GroveStart.Dtos.Validation;
using GroveStart.Model;

namespace GroveStart.Dtos.Order;

public class RegisterOrderRequest
{
    [Required(ErrorMessage = "ID do usuário é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "ID do usuário inválido.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "ID do cliente é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "ID do cliente inválido.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Título é obrigatório.")]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Período é obrigatório.")]
    [EnumDataType(typeof(Period), ErrorMessage = "Período inválido (0=Diario, 1=Semanal, 2=Mensal).")]
    public Period Period { get; set; } = Period.Mensal;

    [Required(ErrorMessage = "Data de início é obrigatória.")]
    [UtcDateTime]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "Data de fim é obrigatória.")]
    [UtcDateTime]
    public DateTime? EndDate { get; set; }
}
