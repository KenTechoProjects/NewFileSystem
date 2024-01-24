 

namespace Application.Common.Helpers
{
    public class ConfigureSwaggerGenOptionsHelper
    {
        public static void AddOptions(SwaggerGenOptions o)
        {
           XmlDocument.AddSwaggerXmlCommentsHelper(o);
            o.OperationFilter<SwaggerDefaultValues>();

            o.OperationFilter<SwaggerDefaultValue>();

        }
    }
}
