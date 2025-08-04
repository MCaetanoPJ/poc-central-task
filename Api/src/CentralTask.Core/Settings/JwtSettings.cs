namespace CentralTask.Core.Settings;

public class JwtSettings
{
    public static string Section => nameof(JwtSettings);

    public string? SecretKey { get; set; }
    public int ExpiracaoHoras { get; set; }
    public string? Emissor { get; set; }
    public string? ValidoEm { get; set; }
}
