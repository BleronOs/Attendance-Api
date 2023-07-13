using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AttendanceTracker.Api.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace AttendanceTracker.Api.Middlewares;
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenConfiguration _tokenConfig;

    public JwtMiddleware(RequestDelegate next, IOptions<TokenConfiguration> tokenConfig)
    {
        _next = next;
        _tokenConfig = tokenConfig.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            AttachUserToContext(context, token);
        }
        else
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null && RequiresAuthorization(endpoint))
            {
                // Handle unauthorized request
                var unauthorizedMessage = "You are not authorized to access this resource.";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(unauthorizedMessage);
                return;
            }
        }

        await _next(context);


    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenConfig.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var user = jwtToken.Claims.First(x => x.Type == "id").Value;
            // attach user to context on successful jwt validation
            context.Items["User"] = user;
        }
        catch (Exception ex)
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
    private static bool RequiresAuthorization(Endpoint endpoint)
    {
        var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (controllerActionDescriptor != null)
        {
            var authorizeAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttribute<AuthorizeAttribute>()
                ?? controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();

            if (authorizeAttribute != null)
            {
                // Authorization required
                return true;
            }
        }

        return false;
    }

}