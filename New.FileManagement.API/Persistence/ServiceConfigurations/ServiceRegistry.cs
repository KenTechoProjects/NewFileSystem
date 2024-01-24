
using Application.Common.DTOs;
using Application.Common.Models;
using GlobalPay.FileSystemManager.Application.Helpers;
using GlobalPay.FileSystemManager.Application.Implementations;
using GlobalPay.FileSystemManager.Application.Interfacses.FileSystems;

namespace Persistence.ServiceConfigurations
{
    public static class ServiceRegistry
    {
        public static IServiceCollection PushService(this IServiceCollection services, IConfiguration conf)
        {
            //Add services here

            #region Mapster 
            TypeAdapterConfig.GlobalSettings.Default
           .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
           .IgnoreNullValues(true)
           .AddDestinationTransform((string x) => x.Trim())
           .AddDestinationTransform((string x) => x ?? "")
           .AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
            services.RegisterMapsterConfiguration();

            #endregion

            #region Other services

            var conString = conf["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<IAppDbContext, AppDbContext>(options => options.UseSqlServer(conString));
       
          


            services.AddHttpContextAccessor();
          //  services.AddSingleton(Log.Logger);
            services.AddFluentValidationClientsideAdapters(fv =>
            {
                //Register for global validations
                // fv.RegisterValidatorsFromAssemblyContaining<RegisterAppCommand>();
            });

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<ILanguageConfigurationProvider, LanguageConfigurationProvider>();
            services.AddScoped<IMessageProvider, MessageProvider>();
            services.AddScoped<IFileSystemManagerService, FileSystemManagerService>();
            services.AddHttpClient();
            services.AddScoped<IAzureCloudMailHelper,AzureCloudMailHelper >();

            
            #endregion

            #region Static Files
            services.Configure<LanguageSettings>(options => conf.GetSection("LanguageSettings").Bind(options));

            //services.AddSingleton(conf.GetSection("APIBaseSettings").Get<APIBaseSettings>());
            #endregion

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            #region Plocies and Cors
          
            services.AddCors(options =>
            {
             
               options.AddPolicy("AllowedPolicy", builder =>
                   builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());

            });

            #endregion
            return services;
        }
    }
}
