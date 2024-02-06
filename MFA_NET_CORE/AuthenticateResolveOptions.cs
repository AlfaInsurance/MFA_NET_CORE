using MFA_NET_CORE.AuthenticationResolvers;

namespace MFA_NET_CORE;

/// <summary>
/// Параметры разрешения аутентификации
/// </summary>
public class AuthenticateResolveOptions
{
    /// <summary>
    /// Информация об обработчиках разрешений аутентификации
    /// </summary>
    public IDictionary<string, Type> AuthenticationResolveHandlerMap { get; }
        = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Добавляет информацию об обработчике разрешений аутентификации
    /// </summary>
    /// <typeparam name="TResolver">Тип обработчика</typeparam>
    /// <param name="name">Наименование обработчика</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddResolver<TResolver>(string name)
        where TResolver : class, IAuthenticationResolver
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        if (AuthenticationResolveHandlerMap.ContainsKey(name))
            throw new InvalidOperationException($"Обработчик {name} уже содержится");

        AuthenticationResolveHandlerMap[name] = typeof(TResolver);
    }
}
