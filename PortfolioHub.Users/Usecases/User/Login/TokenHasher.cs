﻿namespace PortfolioHub.Users.Usecases.User.Login;

internal sealed class TokenHasher
{
    public string HashToken(string token)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public bool VerifyToken(string token, string hashedToken)
    {
        var hashedProvidedToken = HashToken(token);
        return hashedProvidedToken == hashedToken;
    }
}