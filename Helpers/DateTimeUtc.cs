namespace GroveStart.Helpers;

public static class DateTimeUtc
{
    /// <summary>Garante <see cref="DateTimeKind.Utc"/> antes de persistir em <c>timestamptz</c>.</summary>
    public static DateTime EnsureUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => throw new ArgumentException(
                "Data deve estar em UTC (envie no JSON com sufixo Z, ex.: 2026-05-21T12:00:00Z).",
                nameof(value))
        };
}
