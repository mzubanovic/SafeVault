using System.Data;
using Dapper;
using SafeVault.Models;
using SafeVault.Security;

namespace SafeVault.Data;

public sealed class UserRepository : IUserRepository
{
    private const string GetByUsernameSql = """
        SELECT UserID, Username, Email
        FROM Users
        WHERE Username = @Username;
        """;

    private const string GetAuthUserByUsernameSql = """
        SELECT UserID, Username, PasswordHash, Role
        FROM Users
        WHERE Username = @Username;
        """;

    private readonly IDbConnection connection;

    public UserRepository(IDbConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        this.connection = connection;
    }

    public User? GetUserByUsername(string username)
    {
        if (!InputValidator.IsValidUsername(username))
        {
            throw new ArgumentException("Username is invalid.", nameof(username));
        }

        return connection.QuerySingleOrDefault<User>(
            GetByUsernameSql,
            new { Username = username });
    }

    public AuthUser? GetAuthUserByUsername(string username)
    {
        if (!InputValidator.IsValidUsername(username))
        {
            throw new ArgumentException("Username is invalid.", nameof(username));
        }

        return connection.QuerySingleOrDefault<AuthUser>(
            GetAuthUserByUsernameSql,
            new { Username = username });
    }
}