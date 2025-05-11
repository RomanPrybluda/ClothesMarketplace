using ClothesMarketPlace.Infrastructure.Helpers;
using ClothesMarketPlace.Infrastructure.Services.Email;
using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesMarketPlace.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEmailProvider, EmailProvider>();
            services.AddScoped<IHtmlTemplatesReader, HtmlTemplatesReader>();
            return services;
        }
    }
}
