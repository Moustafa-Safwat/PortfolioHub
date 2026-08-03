using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortfolioHub.Users.Domain.Entities.Users;
using PortfolioHub.Users.Domain.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PortfolioHub.Users.Usecases.User.Create;

internal sealed partial class UsernameGenerator(
    UserManager<ApplicationUser> userManager
) : IUsernameGenerator
{
    private const int MaxUsernameLength = 50;
    private const int MaxSuffixAttemptsPerBase = 20;

    public async Task<Result<string>> GenerateUniqueUsernameAsync(
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        var baseCandidates = BuildBaseCandidates(email, firstName, lastName);

        if (baseCandidates.Count == 0)
        {
            baseCandidates.Add($"user{Guid.NewGuid():N}"[..12]);
        }

        var allCandidates = BuildCandidateUsernames(baseCandidates, MaxSuffixAttemptsPerBase);

        if (allCandidates.Count == 0)
        {
            return Result.Error("Failed to generate a valid username candidate.");
        }

        var normalizedCandidates = allCandidates
            .Select(NormalizeUserName)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var existingNormalizedUserNames = await userManager.Users
            .AsNoTracking()
            .Where(u => u.NormalizedUserName != null && normalizedCandidates.Contains(u.NormalizedUserName))
            .Select(u => u.NormalizedUserName!)
            .ToListAsync(cancellationToken);

        var taken = existingNormalizedUserNames
            .ToHashSet(StringComparer.Ordinal);

        var availableUserName = allCandidates.FirstOrDefault(candidate =>
            !taken.Contains(NormalizeUserName(candidate)));

        if (!string.IsNullOrWhiteSpace(availableUserName))
        {
            return Result.Success(availableUserName);
        }

        // Very rare fallback: add a random suffix and check once more
        var fallback = SanitizeUsername($"user{Guid.NewGuid():N}"[..16]);
        var fallbackNormalized = NormalizeUserName(fallback);

        var fallbackExists = await userManager.Users
            .AsNoTracking()
            .AnyAsync(u => u.NormalizedUserName == fallbackNormalized, cancellationToken);

        if (!fallbackExists)
        {
            return Result.Success(fallback);
        }

        return Result.Error("Failed to generate a unique username.");
    }

    private static List<string> BuildBaseCandidates(
        string email,
        string firstName,
        string lastName)
    {
        var candidates = new List<string>();

        AddIfValid(candidates, SanitizeUsername($"{firstName}{lastName}"));

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            AddIfValid(candidates, SanitizeUsername($"{firstName}{lastName[0]}"));
        }

        var emailLocalPart = ExtractEmailLocalPart(email);
        AddIfValid(candidates, SanitizeUsername(emailLocalPart));

        return candidates
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static List<string> BuildCandidateUsernames(
        IReadOnlyCollection<string> baseCandidates,
        int maxSuffixAttemptsPerBase)
    {
        var candidates = new List<string>();

        foreach (var baseCandidate in baseCandidates)
        {
            if (string.IsNullOrWhiteSpace(baseCandidate))
                continue;

            candidates.Add(baseCandidate);

            for (int i = 1; i <= maxSuffixAttemptsPerBase; i++)
            {
                var suffixed = AppendSuffix(baseCandidate, i);
                if (!string.IsNullOrWhiteSpace(suffixed))
                {
                    candidates.Add(suffixed);
                }
            }
        }

        return candidates
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string AppendSuffix(string baseUsername, int suffix)
    {
        var suffixText = suffix.ToString(CultureInfo.InvariantCulture);
        var maxBaseLength = MaxUsernameLength - suffixText.Length;

        if (maxBaseLength <= 0)
            return suffixText[..Math.Min(suffixText.Length, MaxUsernameLength)];

        var truncatedBase = baseUsername.Length > maxBaseLength
            ? baseUsername[..maxBaseLength]
            : baseUsername;

        return $"{truncatedBase}{suffixText}";
    }

    private string NormalizeUserName(string userName)
        => userManager.NormalizeName(userName);

    private static string ExtractEmailLocalPart(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;

        var atIndex = email.IndexOf('@');
        return atIndex > 0 ? email[..atIndex] : email;
    }

    private static void AddIfValid(ICollection<string> list, string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            list.Add(value);
        }
    }

    /// <summary>
    /// Sanitizes input into a username-friendly value.
    /// </summary>
    private static string SanitizeUsername(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var normalized = input.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        var result = builder.ToString().Normalize(NormalizationForm.FormC);

        result = InvalidUsernameCharsRegex().Replace(result, "");
        result = RepeatedSeparatorRegex().Replace(result, "_");
        result = result.Trim('.', '_');

        if (result.Length > MaxUsernameLength)
        {
            result = result[..MaxUsernameLength];
        }

        return result.ToLowerInvariant();
    }

    [GeneratedRegex(@"[^a-zA-Z0-9._]")]
    private static partial Regex InvalidUsernameCharsRegex();

    [GeneratedRegex(@"[._]{2,}")]
    private static partial Regex RepeatedSeparatorRegex();
}

