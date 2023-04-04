using WebApi.Extensions;
using SwaggerOptions = Swashbuckle.AspNetCore.Swagger.SwaggerOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.InstallServicesInAssembly(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    var swaggerOptions = new WebApi.Options.SwaggerOptions();
    app.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
    app.UseSwagger(options => options.RouteTemplate = swaggerOptions.RouteTemplate);
    app.UseSwaggerUI(options => options.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description));
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();