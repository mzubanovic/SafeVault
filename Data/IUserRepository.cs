using SafeVault.Models;

namespace SafeVault.Data;

public interface IUserRepository
{
    AuthUser? GetAuthUserByUsername(string username);
}