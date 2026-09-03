using SafeVault.Models;
using SafeVault.Security;

namespace SafeVault.Tests;

public class TestAuthorization
{
    [Test]
    public void AdminUserCanAccessAdminFunctionality()
    {
        var user = new AuthenticatedUser("marko123", "admin");

        Assert.That(AuthorizationService.CanAccessAdmin(user), Is.True);
    }

    [Test]
    public void NormalUserCannotAccessAdminFunctionality()
    {
        var user = new AuthenticatedUser("marko123", "user");

        Assert.That(AuthorizationService.CanAccessAdmin(user), Is.False);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("manager")]
    [TestCase("superuser")]
    public void UnknownOrMissingRolesAreDenied(string? role)
    {
        var user = role is null ? null : new AuthenticatedUser("marko123", role);

        Assert.That(AuthorizationService.CanAccessAdmin(user), Is.False);
    }

    [Test]
    public void AdminRoleComparisonIsCaseInsensitive()
    {
        var user = new AuthenticatedUser("marko123", "ADMIN");

        Assert.That(AuthorizationService.CanAccessAdmin(user), Is.True);
    }
}