using Domain.Constants;
using Domain.Models;
using Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SecurityJwt
{
    public static class SecurityIoC
    {
        public static IServiceCollection AddAppSecurity(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SecuritySettings>(config.GetSection(SettingsConfig.SettingsJwt));
            services.AddTransient<ISecurityJwt, SecurityJwt>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = config.GetValue<bool>(SettingsConfig.RequireHttpsMetadata);
                    options.SaveToken = config.GetValue<bool>(SettingsConfig.SaveToken);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = config.GetValue<bool>(SettingsConfig.ValidateIssuer),
                        ValidateAudience = config.GetValue<bool>(SettingsConfig.ValidateAudience),
                        ValidAudience = config[SettingsConfig.ValidAudience],
                        ValidIssuer = config[SettingsConfig.ValidIssuer],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config[SettingsConfig.Secret])),
                    };
                });
            return services;
        }

    }
}
