namespace SafeVault.Models;

public sealed record AuthenticatedUser
{
	public string Username { get; }

	public string Role { get; }

	internal AuthenticatedUser(string username, string role)
	{
		Username = username;
		Role = role;
	}
}