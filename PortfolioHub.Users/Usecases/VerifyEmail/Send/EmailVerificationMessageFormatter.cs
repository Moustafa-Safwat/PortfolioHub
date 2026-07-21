using System.Net;
using PortfolioHub.Users.Domain.Interfaces;

namespace PortfolioHub.Users.Usecases.VerifyEmail.Send;

/// <summary>
/// Creates the branded, email-client-safe message used to verify a new account.
/// </summary>
public sealed class EmailVerificationMessageFormatter
    : IEmailVerificationMessageFormatter
{
    private const string EmailTemplate = """
        <!doctype html>
        <html lang="en">
        <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <meta name="color-scheme" content="light dark">
            <meta name="supported-color-schemes" content="light dark">
            <title>Verify your email address</title>
            <style>
                @media only screen and (max-width: 620px) {
                    .email-shell { width: 100% !important; }
                    .mobile-padding { padding-left: 24px !important; padding-right: 24px !important; }
                    .mobile-button { display: block !important; width: auto !important; }
                }
                @media (prefers-color-scheme: dark) {
                    .page { background-color: #0f172a !important; }
                    .card { background-color: #111827 !important; border-color: #334155 !important; }
                    .surface { background-color: #172033 !important; border-color: #334155 !important; }
                    .title, .body-copy, .feature-title { color: #f8fafc !important; }
                    .muted { color: #cbd5e1 !important; }
                    .divider { border-color: #334155 !important; }
                }
            </style>
        </head>
        <body class="page" style="margin:0;padding:0;background-color:#f3f6f9;font-family:'Segoe UI',Arial,sans-serif;color:#172033;">
            <div style="display:none;max-height:0;overflow:hidden;opacity:0;">
                Confirm your email to join a community advancing AEC through AI, automation, and shared knowledge.
            </div>

            <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0" style="background-color:#f3f6f9;">
                <tr>
                    <td align="center" style="padding:32px 16px;">
                        <table role="presentation" class="email-shell card" width="600" cellspacing="0" cellpadding="0" border="0" style="width:600px;max-width:600px;background-color:#ffffff;border:1px solid #d8e1ea;border-radius:12px;overflow:hidden;">
                            <tr>
                                <td style="height:6px;background-color:#0078d4;font-size:0;line-height:0;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="mobile-padding" style="padding:30px 40px 24px;border-bottom:1px solid #e2e8f0;">
                                    <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td>
                                                <div style="font-size:21px;font-weight:700;letter-spacing:-0.4px;color:#172033;">Moustafa Safwat</div>
                                                <div style="padding-top:5px;font-size:12px;font-weight:600;letter-spacing:0.08em;text-transform:uppercase;color:#0078d4;">Building the intelligent future of AEC</div>
                                            </td>
                                            <td align="right" style="font-size:12px;color:#64748b;">Account verification</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td class="mobile-padding" style="padding:42px 40px 18px;">
                                    <div style="display:inline-block;padding:6px 10px;border-radius:999px;background-color:#e8f3ff;color:#0067b8;font-size:11px;font-weight:700;letter-spacing:0.08em;text-transform:uppercase;">Welcome to the community</div>
                                    <h1 class="title" style="margin:18px 0 14px;font-size:30px;line-height:1.22;letter-spacing:-0.7px;color:#172033;">Verify your email and start exploring</h1>
                                    <p class="body-copy" style="margin:0;font-size:16px;line-height:1.75;color:#334155;">
                                        Thank you for joining. This is a place for engineers, developers, and companies interested in practical AI, automation workflows, and digital tools that improve productivity across the AEC industry.
                                    </p>
                                </td>
                            </tr>

                            <tr>
                                <td class="mobile-padding" style="padding:18px 40px 34px;">
                                    <table role="presentation" class="surface" width="100%" cellspacing="0" cellpadding="0" border="0" style="background-color:#f7fbff;border:1px solid #c7e0f4;border-radius:8px;">
                                        <tr>
                                            <td align="center" style="padding:30px 24px;">
                                                <div style="width:44px;height:44px;line-height:44px;border-radius:50%;background-color:#0078d4;color:#ffffff;font-size:22px;font-weight:700;text-align:center;">&#10003;</div>
                                                <h2 class="feature-title" style="margin:16px 0 8px;font-size:19px;line-height:1.4;color:#172033;">Confirm this email address</h2>
                                                <p class="muted" style="margin:0 0 22px;font-size:14px;line-height:1.6;color:#64748b;">Use the secure link below to activate your account.</p>
                                                <a href="{VerificationLink}" class="mobile-button" style="display:inline-block;padding:13px 24px;border-radius:4px;background-color:#0078d4;color:#ffffff;font-size:15px;font-weight:600;line-height:1.4;text-decoration:none;">Verify email address</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td class="mobile-padding" style="padding:0 40px 34px;">
                                    <h2 class="feature-title" style="margin:0 0 16px;font-size:17px;color:#172033;">What you can discover after verification</h2>
                                    <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td valign="top" style="width:24px;padding:2px 10px 12px 0;color:#0078d4;font-weight:700;">01</td>
                                            <td class="muted" style="padding-bottom:12px;font-size:14px;line-height:1.6;color:#475569;"><strong class="feature-title" style="color:#172033;">Applied AEC innovation</strong><br>Projects and ideas connecting engineering, AI, BIM, and automation.</td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="width:24px;padding:2px 10px 12px 0;color:#0078d4;font-weight:700;">02</td>
                                            <td class="muted" style="padding-bottom:12px;font-size:14px;line-height:1.6;color:#475569;"><strong class="feature-title" style="color:#172033;">Practical knowledge</strong><br>Blogs that explain workflows, lessons learned, and implementation patterns.</td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="width:24px;padding:2px 10px 0 0;color:#0078d4;font-weight:700;">03</td>
                                            <td class="muted" style="font-size:14px;line-height:1.6;color:#475569;"><strong class="feature-title" style="color:#172033;">Open collaboration</strong><br>Tools and open-source work designed to help developers and companies move faster.</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td class="mobile-padding" style="padding:0 40px 34px;">
                                    <div class="divider" style="border-top:1px solid #e2e8f0;padding-top:24px;">
                                        <p class="muted" style="margin:0 0 10px;font-size:13px;line-height:1.6;color:#64748b;">If the button does not work, copy and paste this address into your browser:</p>
                                        <p style="margin:0;word-break:break-all;font-size:12px;line-height:1.6;color:#0067b8;">{VerificationLink}</p>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="mobile-padding surface" style="padding:24px 40px;background-color:#f8fafc;border-top:1px solid #e2e8f0;">
                                    <p class="muted" style="margin:0 0 8px;font-size:12px;line-height:1.6;color:#64748b;">
                                        For your security, ignore this message if you did not create an account. Never share your password or verification link with anyone.
                                    </p>
                                    <p class="muted" style="margin:0;font-size:12px;line-height:1.6;color:#64748b;">
                                        &copy; {CurrentYear} Moustafa Safwat. Advancing AEC through AI, automation, and shared knowledge.
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </body>
        </html>
        """;

    public Task<string> FormatEmailMessageAsync(
        string verificationLink,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(verificationLink);
        cancellationToken.ThrowIfCancellationRequested();

        var safeVerificationLink = WebUtility.HtmlEncode(verificationLink);
        var emailContent = EmailTemplate
            .Replace("{VerificationLink}", safeVerificationLink, StringComparison.Ordinal)
            .Replace("{CurrentYear}", DateTime.UtcNow.Year.ToString(), StringComparison.Ordinal);

        return Task.FromResult(emailContent);
    }
}
