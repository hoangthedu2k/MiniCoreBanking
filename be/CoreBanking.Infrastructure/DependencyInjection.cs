using CoreBanking.Application.Common.Interfaces;
using CoreBanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Cấu hình các dịch vụ của Infrastructure tại đây (nếu có)
            services.AddDbContext<CoreBankingDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

           services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<CoreBankingDbContext>());

            services.AddSignalR();
            services.AddScoped<INotificationService, Services.SignalRNotificationService>();
            return services;
        }
    }
}
