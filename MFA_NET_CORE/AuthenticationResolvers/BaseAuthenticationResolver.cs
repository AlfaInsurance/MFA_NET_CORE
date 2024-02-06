using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace MFA_NET_CORE.AuthenticationResolvers;

/// <summary>
/// Базовый класс для обработчиков разрешения аутентификации
/// </summary>
public abstract class BaseAuthenticationResolver : IAuthenticationResolver
{
    /// <summary>
    /// Признак того, что обработчик вызывался
    /// </summary>
    private bool _called;

    /// <summary>
    /// Результат <see cref="ResolveAuthenticationAsync"/>
    /// </summary>
    private Task<bool> _resolveAuthenticationAsyncTask;

    /// <summary>
    /// Результат аутентификации
    /// </summary>
    protected AuthenticateResult AuthenticateResult { get; private set; }

    /// <summary>
    /// Http контекст
    /// </summary>
    protected HttpContext Context { get; private set; }

    /// <summary>
    /// Http запрос
    /// </summary>
    protected HttpRequest Request => Context.Request;

    /// <summary>
    /// Аутентификационные данные пользователя
    /// </summary>
    protected ClaimsPrincipal User => AuthenticateResult?.Principal;

    /// <inheritdoc/>
    public ValueTask InitializeAsync(AuthenticateResult authenticateResult, HttpContext context)
    {
        AuthenticateResult = authenticateResult;
        Context = context;
        _called = false;

        return HandleInitializeAsync();
    }

    /// <inheritdoc/>
    public Task<bool> ResolveAuthenticationAsync()
    {
        return HandleResolveAuthenticationOnceAsync();
    }

    /// <summary>
    /// Вызывает <see cref="HandleResolveAuthenticationAsync"/> один раз
    /// </summary>
    /// <inheritdoc cref="ResolveAuthenticationAsync"/>
    protected Task<bool> HandleResolveAuthenticationOnceAsync()
    {
        if (_called)
            return _resolveAuthenticationAsyncTask;

        _called = true;

        return _resolveAuthenticationAsyncTask = HandleResolveAuthenticationAsync();
    }

    /// <summary>
    /// Обрабатывает разрешения аутентификации
    /// </summary>
    /// <inheritdoc cref="ResolveAuthenticationAsync"/>
    protected abstract Task<bool> HandleResolveAuthenticationAsync();

    /// <inheritdoc/>
    public virtual ValueTask ChallengeAsync(AuthenticationProperties properties)
    {
        return default;
    }

    /// <summary>
    /// Обрабатывает инициализацию обработчика
    /// </summary>
    protected virtual ValueTask HandleInitializeAsync()
    {
        return default;
    }
}
