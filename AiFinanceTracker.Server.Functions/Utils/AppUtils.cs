using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;


namespace AiFinanceTracker.Server.Functions.Utils
{
    public static class AppUtils
    {
        public static async Task<string?> GetUserId(HttpContext httpContext)
        {
            try
            {

                string token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                  return null;
                }

                string? azureClientId = Environment.GetEnvironmentVariable("AzureAdClientId");
                string? policyName = Environment.GetEnvironmentVariable("AzurePolicyName");
                string? tenantName = Environment.GetEnvironmentVariable("AzureTenantName");
                if (string.IsNullOrEmpty(azureClientId))
                {
                    throw new ArgumentException("azureClientId cannot be null or empty");
                }

                if (string.IsNullOrEmpty(policyName))
                {
                    throw new ArgumentException("policyName cannot be null or empty");
                }

                if (string.IsNullOrEmpty(tenantName))
                {
                    throw new ArgumentException("tenantName cannot be null or empty");
                }

               
                string stsDiscoveryEndpoint = $"https://{tenantName}.b2clogin.com/{tenantName}.onmicrosoft.com/v2.0/.well-known/openid-configuration?p={policyName}";

                var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever()); //1. need the 'new OpenIdConnect...'  
                OpenIdConnectConfiguration config = await configManager.GetConfigurationAsync();

                TokenValidationParameters validationParameters = new()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    ValidAudience = azureClientId,
                    IssuerSigningKeys = config.SigningKeys,
                    ValidIssuer = config.Issuer
                };

                JwtSecurityTokenHandler tokendHandler = new();
                SecurityToken jwt;
                var validatedToken = (SecurityToken)new JwtSecurityToken();
                var result = tokendHandler.ValidateToken(token, validationParameters, out jwt);
                var dataToValidate = jwt as JwtSecurityToken;
                string? userId = dataToValidate?.Claims.Where(c => c.Type == "oid")?.FirstOrDefault()?.Value;
                return userId;
            }
            catch (Exception ex) { 
                return null;
             }
        } 
    }
}
