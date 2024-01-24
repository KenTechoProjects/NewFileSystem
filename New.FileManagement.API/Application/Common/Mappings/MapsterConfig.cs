 
namespace Application.Mappings
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            #region   the mappings start here
    TypeAdapterConfig conf=new TypeAdapterConfig();


            #endregion Mapping ends here
        


            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
