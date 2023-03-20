using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;
using System;

public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public CustomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserService userService) : base(options, logger, encoder, clock)
    {
        _userService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        var token = authorizationHeader.ToString().Split(' ')[1];
        var user = await _userService.GetByToken(token);

        if (user == null)
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Name, user.username),
            new Claim(ClaimTypes.Email, user.email),
            new Claim("Firstname", user.firstname),
            new Claim("Lastname", user.lastname),
            new Claim("Role", user.Role.name),
            new Claim("role_id", user.Role.id.ToString()),
        };
        if (user.Team != null)
        {
            claims.Add(new Claim("Team", user.Team.name));
            claims.Add(new Claim("team_id", user.Team.id.ToString()));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

public static class CustomPolicies
{
    public static AuthorizationPolicy isEmployee = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .RequireClaim("role_id", "1").Build();

    public static AuthorizationPolicy isHr = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .RequireClaim("role_id", "3").Build();

    public static AuthorizationPolicy isManager = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .RequireClaim("role_id", "2").Build();

    public static AuthorizationPolicy isHrOrManager = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .RequireAssertion(context =>
    {
        return context.User.HasClaim(c =>
                (c.Type == "role_id" && c.Value == "3") ||
                (c.Type == "role_id" && c.Value == "2")
            );
    }).Build();

}
