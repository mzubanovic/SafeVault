using SafeVault.Security;

namespace SafeVault.Tests;

public class TestInputValidation
{
    [TestCase("marko123")]
    [TestCase("marko_123")]
    [TestCase("alice_123")]
    [TestCase("UserName")]
    public void ValidUsernamesAreAccepted(string username)
    {
        Assert.That(InputValidator.IsValidUsername(username), Is.True);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("ab")]
    [TestCase("user-name")]
    [TestCase("user name")]
    [TestCase("<script>alert('XSS')</script>")]
    [TestCase("valid_user\n")]
    public void InvalidUsernamesAreRejected(string? username)
    {
        Assert.That(InputValidator.IsValidUsername(username), Is.False);
    }

    [Test]
    public void UsernameLongerThan50CharactersIsRejected()
    {
        Assert.That(InputValidator.IsValidUsername(new string('a', 51)), Is.False);
    }

    [TestCase("alice@example.com")]
    [TestCase("user.name+tag@example.co.uk")]
    public void ValidEmailAddressesAreAccepted(string email)
    {
        Assert.That(InputValidator.IsValidEmail(email), Is.True);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("not-an-email")]
    [TestCase("<script>alert('XSS')</script>")]
    [TestCase("<img src=x onerror=alert('XSS')>")]
    [TestCase("<svg onload=alert('XSS')>")]
    public void InvalidEmailAddressesAreRejected(string? email)
    {
        Assert.That(InputValidator.IsValidEmail(email), Is.False);
    }

    [TestCase("' OR 1=1 --")]
    [TestCase("' OR '1'='1")]
    [TestCase("admin' --")]
    public void SqlInjectionAttemptsAreRejectedAsUsernames(string username)
    {
        Assert.Multiple(() =>
        {
            Assert.That(InputValidator.IsValidUsername(username), Is.False);
            Assert.That(InputValidator.IsValidEmail(username), Is.False);
        });
    }

    [TestCase("<script>alert('XSS')</script>")]
    [TestCase("<img src=x onerror=alert('XSS')>")]
    [TestCase("<svg onload=alert('XSS')>")]
    public void XssAttemptsAreRejected(string maliciousInput)
    {
        Assert.Multiple(() =>
        {
            Assert.That(InputValidator.IsValidUsername(maliciousInput), Is.False);
            Assert.That(InputValidator.IsValidEmail(maliciousInput), Is.False);
        });
    }
}