using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetForemost.Data;
using NetForemost.Middleware;
using NetForemost.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

/* Json Settings File */
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

/* DataContext */
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

/* Services */
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AssignmentService>();

/* Loggin Configuration */
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();


/* Adding services to container */
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();

/* Swagger UI configuration */
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetForemost", Version = "v1" });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. ApiKey: X-API-KEY",
        In = ParameterLocation.Header,
        Name = "x-api-key",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                Scheme = "ApiKeyScheme",
                Name = "x-api-key",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    /* Adding comments and examples to EndPoints */
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

/* Cors policy configuration */
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

/* Configure the HTTP request pipeline */
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetForemost v1");
    });
}

app.UseCors("AllowReactApp");

/* Middlewares */
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
