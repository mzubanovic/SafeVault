# SafeVault

## Project Overview

SafeVault is a sample application created for a Coursera secure coding assignment. It demonstrates secure coding practices for protecting user accounts and sensitive data, including input validation, SQL injection prevention, password hashing, authentication, role-based authorization, and automated security testing.

## Technologies

- C#
- .NET 10
- NUnit
- Dapper
- BCrypt.Net-Next
- SQL

## Activity 1 - Secure Coding

### Input Validation

`InputValidator` validates usernames and email addresses before they are used by application or database code.

Username validation:

- Rejects null, empty, and whitespace-only values.
- Requires between 3 and 50 characters.
- Allows only letters, numbers, and underscores.
- Rejects characters and payloads commonly associated with malicious input.

Email validation uses .NET `EmailAddressAttribute` and `MailAddress`, rejects blank values, and limits the input to 254 characters.

Validation reduces the accepted input surface, but it is not a replacement for output encoding or other security controls.

### SQL Injection Prevention

`UserRepository` uses Dapper parameterized queries. Usernames are passed as the `@Username` parameter:

```csharp
WHERE Username = @Username
```

The username is supplied separately through a parameter object. User input is never concatenated directly into SQL strings. This ensures that SQL injection attempts are treated as parameter values rather than executable SQL syntax.

### XSS Testing

The Activity 1 tests include common XSS payloads such as:

- `<script>alert('XSS')</script>`
- `<img src=x onerror=alert('XSS')>`
- `<svg onload=alert('XSS')>`

The current project has no HTML rendering layer or web UI. These payloads are rejected by the input validation tests, but input validation alone is not a complete XSS defense. If a web UI is added, user-controlled data must be encoded for its specific output context before it is rendered.

### Security Tests

Activity 1 contains NUnit tests for:

- Valid and invalid usernames
- Valid and invalid email addresses
- Null, empty, whitespace, short, long, and malformed usernames
- SQL injection attempts
- XSS attempts

## Activity 2 - Authentication and Authorization

### Authentication

The authentication flow accepts a username and password, retrieves the authentication record through the repository, and verifies the supplied password against the stored password hash.

`BCrypt.Net-Next` is used for password hashing and verification. Passwords are never stored as plaintext. `PasswordHasher` creates bcrypt hashes and verifies supplied passwords using BCrypt verification rather than manual string comparison.

Authentication returns a generic failure result for invalid credentials, including an incorrect password or a non-existent username. This helps reduce username enumeration through different login responses.

### Authorization / RBAC

The project uses role-based access control through `AuthorizationService`.

- Supported roles in the assignment are `admin` and `user`.
- Admin functionality is restricted to the `admin` role.
- Normal users cannot access admin functionality.
- Unknown, empty, or missing roles are denied by default.
- Role comparison is case-insensitive for the admin role.

Authorization is separate from authentication: a user must first authenticate, then the authenticated identity and role must be checked for each protected operation.

### Authentication Tests

`TestAuthentication` covers:

- Valid username and password
- Password hashes differing from plaintext passwords
- Successful BCrypt password verification
- Incorrect passwords
- Non-existent usernames
- Null, empty, and short usernames
- Empty passwords
- Invalid stored password hashes

### Authorization Tests

`TestAuthorization` covers:

- Admin users accessing admin functionality
- Normal users being denied admin access
- Null and empty roles being denied
- Unknown roles being denied
- Case-insensitive handling of the `admin` role

## Security Design

The main security principles used are:

- Strict input validation
- Parameterized SQL queries
- Secure bcrypt password hashing
- Generic authentication failures
- Deny-by-default authorization
- Automated NUnit security testing

## Testing

The current test suite contains 44 NUnit test cases. All 44 tests pass.

Run the build with:

```text
dotnet build
```

Run the tests with:

```text
dotnet test
```

Expected successful result:

```text
44/44 tests passed
```

## Project Structure

```text
SafeVault/
|-- Data/
|   |-- IUserRepository.cs       Repository abstraction for authentication data
|   `-- UserRepository.cs        Dapper queries and database access
|-- Models/
|   |-- User.cs                  Basic user model
|   |-- AuthUser.cs              User data needed for authentication
|   `-- AuthenticatedUser.cs      Identity returned after successful login
|-- Security/
|   |-- InputValidator.cs        Username and email validation
|   |-- PasswordHasher.cs        BCrypt password hashing and verification
|   |-- AuthenticationService.cs Login and credential verification
|   `-- AuthorizationService.cs  Admin role authorization
|-- Tests/
|   |-- TestInputValidation.cs   Activity 1 validation and attack tests
|   |-- TestAuthentication.cs    Activity 2 authentication tests
|   `-- TestAuthorization.cs     Activity 2 RBAC tests
|-- SafeVault.csproj             Project configuration and NuGet packages
`-- README.md                    Project documentation
```

## Limitations

SafeVault is a learning and secure-coding assignment, not a production-ready authentication system.

The current implementation does not provide:

- A web UI or output encoding layer
- Session management
- CSRF protection
- Rate limiting or account lockout
- Production identity or token management
- Production secret management

Database connection security, least-privilege database credentials, schema constraints, deployment configuration, logging policy, and operational monitoring would also need to be designed for a production system.

## Future Work

Activity 3 will further debug and secure the application.