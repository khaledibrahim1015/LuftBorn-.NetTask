using MyApp.Api.Middleware;

namespace MyApp.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHanding(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandling>();
        //app.UseExceptionHandler(errorApp =>
        //{
        //    errorApp.Run(async context =>
        //    {
        //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        context.Response.ContentType = "application/json";
        //        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        //        if (contextFeature != null)
        //        {
        //            var exception = contextFeature.Error;
        //            var response = new ErrorResponse
        //            {
        //                Message = exception.Message
        //            };
        //            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        //        }
        //    });
        //});
        return app;
    }
}


