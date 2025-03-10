using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeoMarket.Application.Interfaces;
using NeoMarket.Application.Services;
using System;
using System.Net.Http.Headers;

namespace NeoMarket.Infrastructure.DependencyInjection
{
    public static class HttpClientConfiguration
    {
        public static IServiceCollection AddMelhorEnvioHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IMelhorEnvioService, MelhorEnvioService>(client =>
            {
                client.BaseAddress = new Uri(configuration["MelhorEnvio:BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NeoMarket (gui14511@gmail.com)");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["MelhorEnvio:AccessToken"]);
            });

            return services;
        }
    }
}
