namespace SafeVault.Models;

public sealed class AuthUser
{
    public int UserID { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}