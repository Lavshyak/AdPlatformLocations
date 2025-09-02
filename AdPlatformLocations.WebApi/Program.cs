using AdPlatformLocations.Lib;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ValidatorNormalizer>();
builder.Services.AddSingleton<AdPlatformsAndLocationsFromStreamParser>();
builder.Services.AddSingleton<AdPlatformsLocationsRepository>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            var scheme = httpReq.Headers["X-Forwarded-Proto"];
            if (string.IsNullOrWhiteSpace(scheme))
                scheme = httpReq.Scheme;

            swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer
                    { Url = $"{scheme}://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}" },
                new OpenApiServer
                    { Url = $"http://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}" },
                new OpenApiServer
                    { Url = $"https://{httpReq.Host.Value}/{httpReq.Headers["X-Forwarded-Prefix"]}" }
            };
        });
    });
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("v1/swagger.json", "My API V1"); });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();