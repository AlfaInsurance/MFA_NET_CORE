using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MFA_NET_CORE.Handlers;

public class ExampleAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public ExampleAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identities = new List<ClaimsIdentity>();

        if (Request.Headers.ContainsKey(Consts.AuthorizationX))
            identities.Add(new ClaimsIdentity(Consts.AuthenticateX));

        if (Request.Headers.ContainsKey(Consts.AuthorizationY))
            identities.Add(new ClaimsIdentity(Consts.AuthenticateY));

        if (identities.Count == 0)
            return Task.FromResult(AuthenticateResult.NoResult());

        return Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(
                new ClaimsPrincipal(identities), Consts.ExampleScheme)));
    }
}
