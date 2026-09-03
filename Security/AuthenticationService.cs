using SafeVault.Data;
using SafeVault.Models;

namespace SafeVault.Security;

public sealed class AuthenticationService
{
    private readonly IUserRepository userRepository;
    private readonly PasswordHasher passwordHasher;

    public AuthenticationService(IUserRepository userRepository, PasswordHasher passwordHasher)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        ArgumentNullException.ThrowIfNull(passwordHasher);
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
    }

    public AuthenticatedUser? Authenticate(string? username, string? password)
    {
        if (!InputValidator.IsValidUsername(username) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        var user = userRepository.GetAuthUserByUsername(username!);
        if (user is null || !passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        return new AuthenticatedUser(user.Username, user.Role);
    }
}