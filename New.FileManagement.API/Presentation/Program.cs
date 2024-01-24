
using GlobalPay.FileSystemManager.Presentation.Filters;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


#region  Add services to the container.

builder.Services.PushService(builder.Configuration);
//builder.Services.AddJwtAuthentication(builder.Configuration);

ConfigureLogs();
builder.Host.UseSerilog();
builder.Services.AddControllers(options =>
{
    //Add global filters
    //options.Filters.Add(new ApiExceptionFilter());

}).AddMvcOptions(options =>
{
    options.Filters.Add(
    new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None

    });
    //options.Filters.Add<LanguageFilter>();
    options.Filters.Add<SessionFilter>();
}
       ).AddNewtonsoftJson()
          .AddJsonOptions(options =>
          {
              options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
          });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
#endregion

#region App Configuartions
var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
var logFactory = app.Services.GetRequiredService<ILoggerFactory>();


var er = app.Services.GetService<IConfiguration>() ?? throw new ArgumentNullException("config");
SwaggerOptionsHelper.SwaggerOptionsHelperConfifure(er);

 
    //app.UseSwagger();
    //app.UseSwaggerUI();

    app.UseSwagger();

app.UseSwaggerUI();
     
 

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowedPolicy");
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion


#region Elastic Configuration

void ConfigureLogs()
{

    //get the environment which the app is running on
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    //get configuration files

    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build();

    //Logger
  
        Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .Enrich.WithExceptionDetails()//add exception details
              .WriteTo.Debug()
              .WriteTo.Console()
              .WriteTo.File($"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}ServiceLogs/globalpay-", rollingInterval: RollingInterval.Day)
             // .WriteTo.Elasticsearch(ConfigureElasticSearch(configuration, env))
              .CreateLogger();

    


}

  


#endregion