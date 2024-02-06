using Microsoft.AspNetCore.Authentication;
using System.Text;

namespace MFA_NET_CORE.AuthenticationResolvers;

/// <summary>
/// Обработчик, определяющий является ли аутентификация в целом успешной
/// </summary>
public interface IAuthenticationResolver
{
    /*
    * Подход с инициализацией выбран для того чтобы можно было реализовать запоминание результата ответа
    * независимо от входных параметров и его дальнейшие обработки в цепочки обратных вызовов
    */
    /// <summary>
    /// Инициализирует обработчик
    /// </summary>
    /// <param name="authenticateResult">Результат обработки первичных обработчиков</param>
    /// <param name="context">Http контекст</param>
    ValueTask InitializeAsync(AuthenticateResult authenticateResult, HttpContext context);

    /// <summary>
    /// Разрешает аутентификацию
    /// </summary>
    /// <param name="args">Параметры разрешения</param>
    /// <returns>Результат разрешения аутентификации</returns>
    Task<bool> ResolveAuthenticationAsync();

    /// <summary>
    /// Вызывает поведение на неудачную аутентификацию
    /// </summary>
    /// <param name="properties">Свойства аутентификации</param>
    ValueTask ChallengeAsync(AuthenticationProperties properties);
}
