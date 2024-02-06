using MFA_NET_CORE;
using MFA_NET_CORE.AuthenticateResolve;
using MFA_NET_CORE.AuthenticationResolverProviders;
using MFA_NET_CORE.AuthenticationResolvers;
using MFA_NET_CORE.Extensions;
using MFA_NET_CORE.Handlers;
using MFA_NET_CORE.PolicyEvaluators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

services.AddControllers();

services.AddScoped<IPolicyEvaluator, CustomPolicyEvaluator>();
services.AddScoped<IAuthenticateResolveService, AuthenticateResolveService>();
services.AddScoped<IAuthenticationResolverProvider, AuthenticationResolverProvider>();
services.AddAuthenticationResolver<RequiredAuthenticateTypeResolver>(Consts.RequiredAuthenticate);

services.AddAuthentication(Consts.ExampleScheme)
    .AddScheme<AuthenticationSchemeOptions, ExampleAuthenticationHandler>(Consts.ExampleScheme, _ => { });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
