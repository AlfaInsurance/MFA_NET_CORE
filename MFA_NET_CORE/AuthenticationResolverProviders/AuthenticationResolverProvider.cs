using MFA_NET_CORE.AuthenticationResolvers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MFA_NET_CORE.AuthenticationResolverProviders;

public class AuthenticationResolverProvider : IAuthenticationResolverProvider
{
    /// <summary>
    /// Обработчики разрешений аутентификации
    /// </summary>
    private readonly IEnumerable<IAuthenticationResolver> _authenticationResolvers;

    /// <inheritdoc cref="AuthenticateResolveOptions"/>
    private readonly IOptions<AuthenticateResolveOptions> _options;

    /// <summary>
    /// Словарь с инициализированными обработчиками
    /// </summary>
    private readonly Dictionary<string, IAuthenticationResolver> _resolverrMap
        = new Dictionary<string, IAuthenticationResolver>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="authenticationResolvers">Обработчики разрешений аутентификации</param>
    /// <param name="options">Параметры разрешения аутентификации</param>
    public AuthenticationResolverProvider(
        IEnumerable<IAuthenticationResolver> authenticationResolvers,
        IOptions<AuthenticateResolveOptions> options)
    {
        _authenticationResolvers = authenticationResolvers;
        _options = options;
    }

    /// <inheritdoc/>
    public async ValueTask<IAuthenticationResolver> GetResolverAsync(
        string resolverName,
        AuthenticateResult authenticateResult,
        HttpContext context)
    {
        if (_resolverrMap.TryGetValue(resolverName, out IAuthenticationResolver initHandler))
            return initHandler;

        AuthenticateResolveOptions options = _options.Value;

        if (!options.AuthenticationResolveHandlerMap.TryGetValue(resolverName, out Type resolverType))
            throw new InvalidOperationException($"Обработчик {resolverName} не зарегистрирован в AuthenticateResolveOptions");

        foreach (IAuthenticationResolver resolver in _authenticationResolvers)
        {
            if (resolverType != resolver.GetType())
                continue;

            await resolver.InitializeAsync(authenticateResult, context);

            _resolverrMap[resolverName] = resolver;

            return resolver;
        }

        throw new InvalidOperationException($"Обработчик {resolverName} не зарегистрирован в DI"); ;
    }
}
