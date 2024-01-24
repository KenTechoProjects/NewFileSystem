

namespace Application.Common.Enums
{
    public enum ResponseCodeEnum
    {
        [Description("00")]
        Success = 1,
        [Description("05")]
        ValidationError = 2,
        [Description("06")]
        ApplicationError = 3,
        [Description("07")]
        AuthorizationError = 4,
        [Description("08")]
        UnHandledException = 5,


    }
}
