using SafeVault.Models;

namespace SafeVault.Security;

public static class AuthorizationService
{
    public static bool CanAccessAdmin(AuthenticatedUser? user)
    {
        return user is not null &&
            string.Equals(user.Role, "admin", StringComparison.OrdinalIgnoreCase);
    }
}