using System.Security.Authentication;
using MFA_NET_CORE.AuthenticationResolverProviders;
using MFA_NET_CORE.AuthenticationResolvers;
using MFA_NET_CORE.Metadata;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;

namespace MFA_NET_CORE.AuthenticateResolve;

/// <inheritdoc cref="IAuthenticateResolveService"/>
public class AuthenticateResolveService : IAuthenticateResolveService
{
    /// <inheritdoc cref="IAuthenticationResolverProvider"/>
    private readonly IAuthenticationResolverProvider _resolverProvider;

    /// <summary>
    /// Используемые обработчики разрешений
    /// </summary>
    private readonly List<IAuthenticationResolver> _resolvers;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="resolverProvider">Поставщик обработчиков разрешений аутентификации</param>
    public AuthenticateResolveService(IAuthenticationResolverProvider resolverProvider)
    {
        _resolverProvider = resolverProvider;
        _resolvers = new List<IAuthenticationResolver>();
    }

    /// <inheritdoc />
    public async ValueTask<AuthenticateResult> ResolveAuthenticationAsync(AuthenticateResult authenticateResult, HttpContext context)
    {
        bool isAutenticated = true;

        foreach (string resolveName in GetResolveNames(context))
        {
            IAuthenticationResolver resolver = await _resolverProvider
                .GetResolverAsync(resolveName, authenticateResult, context);

            _resolvers.Add(resolver);

            bool resolved = await resolver.ResolveAuthenticationAsync();

            if (!resolved)
                isAutenticated = false;
        }

        return isAutenticated ? authenticateResult : AuthenticateResult.Fail(new AuthenticationException());
    }

    /// <inheritdoc />
    public async ValueTask ChallengeAsync(HttpContext context, AuthenticationProperties properties)
    {
        foreach (IAuthenticationResolver resolver in _resolvers)
            await resolver.ChallengeAsync(properties);
    }

    /// <summary>
    /// Возвращает набор данных разрешений аутентификаций
    /// </summary>
    /// <param name="context">HTTP контекст</param>
    /// <returns>Набор данных разрешений аутентификаций</returns>
    private IEnumerable<string> GetResolveNames(HttpContext context)
    {
        Endpoint endpoint = context.Features.Get<IEndpointFeature>()
              ?.Endpoint;

        if (endpoint?.Metadata == null)
            return Enumerable.Empty<string>();

        // Извлекаем требуемые типы аутентификации из метаданных в конечной точке
        return endpoint
            .Metadata
            .OfType<IAuthenticationResolveData>()
            .Select(data => data.ResolverName)
            .Distinct();
    }
}
