﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeoMarket.Application.Interfaces;
using NeoMarket.Application.Services;
using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Infrastructure.Data;
using NeoMarket.Infrastructure.Repositories;

namespace NeoMarket.Infrastructure.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuração do contexto do banco de dados
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IRepository<Store>, StoreRepository>();
            services.AddScoped<IRepository<Product>, Repository<Product>>();

            return services;
        }
    }
}
