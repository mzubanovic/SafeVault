using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SafeVault.Security;

public static partial class InputValidator
{
    private static readonly EmailAddressAttribute EmailAttribute = new();

    public static bool IsValidUsername(string? username)
    {
        return !string.IsNullOrWhiteSpace(username) && UsernamePattern().IsMatch(username);
    }

    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email) || email.Length > 254 || !EmailAttribute.IsValid(email))
        {
            return false;
        }

        try
        {
            var parsedEmail = new MailAddress(email);
            return string.Equals(parsedEmail.Address, email, StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    [GeneratedRegex(@"^[\p{L}\p{Nd}_]{3,50}\z", RegexOptions.CultureInvariant)]
    private static partial Regex UsernamePattern();
}