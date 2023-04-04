using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Options;
using SwaggerOptions = Swashbuckle.AspNetCore.Swagger.SwaggerOptions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

var connectionString = config.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException(string.Format(
                           "[Class]: '{0}' [Message]: 'Failed to retrieve connection string from the configuration file'",
                           nameof(Program)));

services.AddDbContext<DataContext>(options => options.ConfigureDbContext(connectionString, env.IsDevelopment()));
services
    .AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<DataContext>();

services.AddMvc();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RESTful Web API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Email = "olsak.marek@outlook.cz",
            Name = "Marek Olšák",
            Url = new Uri("https://www.linkedin.com/in/marek-ol%C5%A1%C3%A1k-1715b724a/")
        },
        Description = "RESTful Web API to demonstrate .NET knowledge"
    });
});

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