using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Domain;
using Repository;
using SecurityJwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAppDomain();
builder.Services.AddAppRepository(builder.Configuration);
builder.Services.AddAppSecurity(builder.Configuration);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DFR API",
        Version = "v1",
        Description = "API Rest presentando un JWT Token generado en Auth.",
        Contact = new OpenApiContact
        {
            Name = "Dario Ramirez",
            Email = "dfr80@hotmail.com",
            Url = new Uri("https://github.com/dframirez80")
        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

    var fileXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var routeXML = Path.Combine(AppContext.BaseDirectory, fileXML);
    c.IncludeXmlComments(routeXML);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "api/docs";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "dfr80@hotmail.com");
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
