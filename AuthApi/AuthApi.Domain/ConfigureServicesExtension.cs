using AuthApi.Domain.Interfaces;
using AuthApi.Domain.Interfaces.Repositories;
using AuthApi.Domain.Repositories;
using AuthApi.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthApi.Domain
{
    public static class ConfigureServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IApplicationService, ApplicationService>();
        }
    }
}
