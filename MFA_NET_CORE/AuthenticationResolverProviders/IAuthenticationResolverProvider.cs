using MFA_NET_CORE.AuthenticationResolvers;
using Microsoft.AspNetCore.Authentication;

namespace MFA_NET_CORE.AuthenticationResolverProviders;

/// <summary>
/// Поставщик обработчиков разрешений аутентификации
/// </summary>
public interface IAuthenticationResolverProvider
{
    /// <summary>
    /// Возвращает обработчик разрешений аутентификации по имени
    /// </summary>
    /// <param name="resolverName">Наименование обработчика</param>
    /// <param name="authenticateResult">Результат обработки первичных обработчиков</param>
    /// <param name="context">Http контекст</param>
    /// <returns>Обработчик разрешений аутентификации</returns>
    ValueTask<IAuthenticationResolver> GetResolverAsync(
        string resolverName,
        AuthenticateResult authenticateResult,
        HttpContext context);
}
