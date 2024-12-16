﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class AuthentificationServiceExtension
{
    public static IServiceCollection AddAuthentificationConfig(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authSettingsToken = configuration["AuthSettings:SecretKey"];
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(authSettingsToken)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }
}