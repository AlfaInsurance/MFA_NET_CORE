using MFA_NET_CORE.Metadata;
using Microsoft.AspNetCore.Http.Features;

namespace MFA_NET_CORE.AuthenticationResolvers;

/// <summary>
/// Обработчик, разрешающий аутентификацию на основе типа аутентификации
/// </summary>
public class RequiredAuthenticateTypeResolver : BaseAuthenticationResolver
{
    /// <inheritdoc />
    protected override Task<bool> HandleResolveAuthenticationAsync()
    {
        Endpoint endpoint = Context.Features.Get<IEndpointFeature>()
              ?.Endpoint;

        // Если эндпоинт не содержит метаданные, то считаем, что ограничения не настроены
        if (endpoint?.Metadata == null)
            return Task.FromResult(true);

        // Извлекаем требуемые типы аутентификации из метаданных конечной точке
        IEnumerable<IRequiredAuthenticateData> requiredTypes
            = endpoint.Metadata.OfType<IRequiredAuthenticateData>();

        // Убираем дубликаты типов чтобы лишний раз по ним не ходить
        IEnumerable<string> distinctRequiredTypes = requiredTypes.Select(r => r.AuthenticateType)
           .Distinct();

        // Ищем требуемые типы аутентификации, которые пользователь не проходил или прошел не успешно
        List<string> missingTypes = distinctRequiredTypes
           .Where(
                requiredType => User
                   ?.Identities
                   ?.Where(i => i.AuthenticationType == requiredType)
                   .All(i => !i.IsAuthenticated)
                   ?? true)
           .ToList();

        return Task.FromResult(missingTypes.Count == 0);
    }
}
