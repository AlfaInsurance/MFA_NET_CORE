namespace MFA_NET_CORE.Metadata;

/// <summary>
/// Данные разрешения аутентификации
/// </summary>
public interface IAuthenticationResolveData
{
    /// <summary>
    /// Наименования обработчика аутентификации
    /// </summary>
    string ResolverName { get; }
}
