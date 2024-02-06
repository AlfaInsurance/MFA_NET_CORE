using MFA_NET_CORE.AuthenticationResolvers;

namespace MFA_NET_CORE.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class SerivceCollectionExtensions
{
    /// <summary>
    /// Добавляет обработчик разрешений <typeparamref name="TResolver"/> в DI
    /// и регистрирует его в сервисе разрешений
    /// </summary>
    /// <typeparam name="TResolver">Тип добавляемого обработчика разрешений</typeparam>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="resolveName">Наименование обработчика разрешений</param>
    /// <returns><paramref name="services"/></returns>
    public static IServiceCollection AddAuthenticationResolver<TResolver>(
        this IServiceCollection services,
        string resolveName)
        where TResolver : class, IAuthenticationResolver
    {
        services.Configure<AuthenticateResolveOptions>(
            o => { o.AddResolver<TResolver>(resolveName); });

        services.AddScoped<IAuthenticationResolver, TResolver>();

        return services;
    }
}
