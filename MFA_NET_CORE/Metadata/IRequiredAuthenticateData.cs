namespace MFA_NET_CORE.Metadata;

/// <summary>
/// Параметры обязательной аутентификации
/// </summary>
public interface IRequiredAuthenticateData
{
    /// <summary>
    /// Тип аутентификации
    /// </summary>
    string AuthenticateType { get; }
}
