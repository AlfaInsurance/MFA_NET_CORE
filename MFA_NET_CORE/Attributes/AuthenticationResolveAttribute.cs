using MFA_NET_CORE.Metadata;

namespace MFA_NET_CORE.Attributes;

/// <inheritdoc cref="IRequiredAuthenticateData"/>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthenticationResolveAttribute : Attribute, IAuthenticationResolveData
{
    /// <inheritdoc/>
    public string ResolverName { get; }

    public AuthenticationResolveAttribute(string resolverName)
    {
        ResolverName = resolverName;
    }
}
