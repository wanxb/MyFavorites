using Microsoft.Extensions.Configuration;
using MyFavorites.Core.Models;
using MyFavorites.Core.Services.Favorites;
using MyFavorites.Core.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyFavorites(this IServiceCollection services, IConfiguration configuration, Action<DataBaseSettings> configure = null, string sectionName = null)
        {
            sectionName ??= DataBaseSettings.SectionName;

            services.AddOptions<DataBaseSettings>()
                .Bind(configuration.GetSection(sectionName))
                .ValidateDataAnnotations();

            return services.AddService(configure);
        }

        public static IServiceCollection AddMyFavorites(this IServiceCollection services, Action<DataBaseSettings> configure)
        {
            services.AddOptions<DataBaseSettings>()
                .Configure(configure)
                .ValidateDataAnnotations();
            return services.AddService(configure);
        }

        private static IServiceCollection AddService(this IServiceCollection services, Action<DataBaseSettings> configure)
        {
            services.PostConfigure<DataBaseSettings>(x =>
            {
                configure?.Invoke(x);
                //var defaultOption = DataBaseSettings.GenDefault();
                //if (string.IsNullOrWhiteSpace(x.LoginAcount) || string.IsNullOrWhiteSpace(x.LoginPassword))
                //{
                //    x.LoginAcount = defaultOption.LoginAcount;
                //    x.LoginPassword = defaultOption.LoginPassword;
                //}
                //if (!x.CacheCountLimit.HasValue)
                //{
                //    x.CacheCountLimit = defaultOption.CacheCountLimit;
                //}
                if (x.DatabaseType == DatabaseType.File)
                {
                    services.AddTransient<IFavoritesService, FileService>();
                }
            });

            //services.AddHttpContextAccessor();

            //services.AddTransient<IShortLinkService, ShortLinkService>();
            //services.AddTransient<IShortLinkRepository, ShortLinkRepository>();

            //services.AddTransient<IApplicationService, ApplicationService>();
            //services.AddTransient<IApplicationRepository, ApplicationRepository>();

            //services.AddTransient<ILogService, LogService>();
            //services.AddTransient<ILogRepository, LogRepository>();

            //services.AddSingleton<IMemoryCaching, MemoryCaching>();
            //services.AddTransient<Base62Converter>();

            //services.AddTransient<ChartFactory>();
            //services.AddTransient<IChart, HourChart>();
            //services.AddTransient<IChart, DayChart>();
            //services.AddTransient<IChart, WeekChart>();
            //services.AddTransient<IChart, MonthChart>();

            //services.AddTransient<IFavoritesService, FavoritesFromFileService>();

            //using IHost host = Host.CreateApplicationBuilder(args).Build();
            //IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
            //string? databaseType = config.GetValue<string>("Database:DatabaseType");
            //if (databaseType == DatabaseType.MongoDB.ToString())
            //    services.AddTransient<IFavoritesService, FavoritesFromMongoDBService>();
            //else if (databaseType == DatabaseType.MySQL.ToString())
            //    services.AddTransient<IFavoritesService, FavoritesFromMySQLService>();
            //else if (databaseType == DatabaseType.File.ToString())
            //    services.AddTransient<IFavoritesService, FavoritesFromFileService>();

            return services;
        }
    }
}