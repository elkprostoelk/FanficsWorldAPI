using FanficsWorldAPI.WebApi.Extensions;
using Newtonsoft.Json.Converters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    // Add services to the container.

    builder.Configuration
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

    builder.AddSerilogLogging();

    builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.RegisterSwagger();
    builder.RegisterServices();

    var app = builder.Build();
    app.UseSerilogRequestLogging();

    app.UseSwagger(opts => opts.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FanficsWorldAPI V1");
    });

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAppExceptionHandler();

    app.UseCors(x => x.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "A critical error on the application startup");
}
finally
{
    await Log.CloseAndFlushAsync();
}
