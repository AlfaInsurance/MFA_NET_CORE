using System.Security.Authentication;
using MFA_NET_CORE.AuthenticateResolve;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace MFA_NET_CORE.PolicyEvaluators;

/// <inheritdoc />
public class CustomPolicyEvaluator : PolicyEvaluator
{
    private readonly IAuthenticateResolveService _resolveService;

    public CustomPolicyEvaluator(IAuthorizationService authorization, IAuthenticateResolveService resolveService)
        : base(authorization)
    {
        _resolveService = resolveService;
    }

    /// <inheritdoc />
    public override async Task<AuthenticateResult> AuthenticateAsync(
        AuthorizationPolicy policy,
        HttpContext context)
    {
        AuthenticateResult authenticateResult = await base.AuthenticateAsync(policy, context);

        return await _resolveService.ResolveAuthenticationAsync(authenticateResult, context);
    }

    /// <inheritdoc />
    public override async Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context,
        object resource)
    {
        // По хорошему нужно переопределить AuthorizationMiddlewareResultHandler и делать это там,
        // Но для нагладности хватит и этого
        if (authenticationResult.Failure is AuthenticationException)
        {
            await _resolveService.ChallengeAsync(context, authenticationResult.Properties);

            return PolicyAuthorizationResult.Challenge();
        }

        PolicyAuthorizationResult authorizationResult = await base.AuthorizeAsync(
                policy,
                authenticationResult,
                context,
                resource);

        return authorizationResult;
    }
}
