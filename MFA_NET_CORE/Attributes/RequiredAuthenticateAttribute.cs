using MFA_NET_CORE.Metadata;

namespace MFA_NET_CORE.Attributes;

public class RequiredAuthenticateAttribute : AuthenticationResolveAttribute, IRequiredAuthenticateData
{
    /// <inheritdoc />
    public string AuthenticateType { get; }

    public RequiredAuthenticateAttribute(string authenticateType) : base(Consts.RequiredAuthenticate)
    {
        AuthenticateType = authenticateType;
    }
}
