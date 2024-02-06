using Microsoft.AspNetCore.Authentication;

namespace MFA_NET_CORE.AuthenticateResolve;

/// <summary>
/// Сервис, разрешающий аутентификацию
/// </summary>
public interface IAuthenticateResolveService
{
    /// <summary>
    /// Разрешает <paramref name="authenticateResult"/>
    /// </summary>
    /// <param name="authenticateResult">Результат аутентификации</param>
    /// <param name="context">HTTP контекст</param>
    /// <returns>Результат разрешения</returns>
    ValueTask<AuthenticateResult> ResolveAuthenticationAsync(AuthenticateResult authenticateResult,
                                                             HttpContext context);

    /// <summary>
    /// Вызывает поведение при неудачной аутентификации
    /// </summary>
    /// <param name="context">HTTP контекст</param>
    /// <param name="properties">Свойства аутентификации</param>
    ValueTask ChallengeAsync(HttpContext context, AuthenticationProperties properties);
}
