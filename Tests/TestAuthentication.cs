using SafeVault.Data;
using SafeVault.Models;
using SafeVault.Security;

namespace SafeVault.Tests;

public class TestAuthentication
{
    private const string Username = "marko123";
    private const string Password = "CorrectPassword!123";

    [Test]
    public void ValidCredentialsAuthenticate()
    {
        var user = CreateUser();
        var service = CreateService(user);

        var authenticatedUser = service.Authenticate(Username, Password);

        Assert.That(authenticatedUser, Is.EqualTo(new AuthenticatedUser(Username, "user")));
    }

    [Test]
    public void PasswordIsStoredAsAHash()
    {
        var passwordHasher = new PasswordHasher();
        var hash = passwordHasher.HashPassword(Password);

        Assert.Multiple(() =>
        {
            Assert.That(hash, Is.Not.EqualTo(Password));
            Assert.That(passwordHasher.VerifyPassword(Password, hash), Is.True);
        });
    }

    [Test]
    public void IncorrectPasswordIsRejected()
    {
        var service = CreateService(CreateUser());

        Assert.That(service.Authenticate(Username, "WrongPassword!123"), Is.Null);
    }

    [Test]
    public void NonexistentUsernameIsRejected()
    {
        var service = CreateService(null);

        Assert.That(service.Authenticate(Username, Password), Is.Null);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("ab")]
    public void InvalidUsernameIsRejected(string? username)
    {
        var service = CreateService(CreateUser());

        Assert.That(service.Authenticate(username, Password), Is.Null);
    }

    [Test]
    public void EmptyPasswordIsRejected()
    {
        var service = CreateService(CreateUser());

        Assert.That(service.Authenticate(Username, ""), Is.Null);
    }

    [Test]
    public void InvalidStoredHashIsRejected()
    {
        var service = CreateService(new AuthUser
        {
            Username = Username,
            PasswordHash = "not-a-bcrypt-hash",
            Role = "user"
        });

        Assert.That(service.Authenticate(Username, Password), Is.Null);
    }

    private static AuthenticationService CreateService(AuthUser? user)
    {
        return new AuthenticationService(
            new FakeUserRepository(user),
            new PasswordHasher());
    }

    private static AuthUser CreateUser()
    {
        return new AuthUser
        {
            Username = Username,
            PasswordHash = new PasswordHasher().HashPassword(Password),
            Role = "user"
        };
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        private readonly AuthUser? user;

        public FakeUserRepository(AuthUser? user)
        {
            this.user = user;
        }

        public AuthUser? GetAuthUserByUsername(string username)
        {
            return user is not null && user.Username == username ? user : null;
        }
    }
}