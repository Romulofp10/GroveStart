using System.ComponentModel.DataAnnotations;

namespace GroveStart.Dtos.Validation;

/// <summary>
/// Exige <see cref="DateTime"/> com <see cref="DateTimeKind.Utc"/> (JSON com sufixo Z).
/// Necessário para colunas <c>timestamptz</c> no PostgreSQL via Npgsql.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class UtcDateTimeAttribute : ValidationAttribute
{
    public UtcDateTimeAttribute()
        : base("Informe a data em UTC com sufixo Z (ex.: 2026-05-21T14:30:00Z).")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        if (value is not DateTime dateTime)
            return new ValidationResult("Valor de data inválido.");

        if (dateTime == default)
            return new ValidationResult(ErrorMessage ?? "Data inválida.");

        if (dateTime.Kind == DateTimeKind.Local)
            return new ValidationResult("Não use horário local; envie em UTC com sufixo Z.");

        if (dateTime.Kind != DateTimeKind.Utc)
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
