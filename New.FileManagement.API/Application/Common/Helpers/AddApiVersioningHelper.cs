 
namespace Application.Common.Helpers
{
    public class AddApiVersioningHelper
    {
        public static void AddApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);               
                o.ReportApiVersions = true;
            }).AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
