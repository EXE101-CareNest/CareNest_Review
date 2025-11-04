using CareNest_Review.API.Middleware;
using CareNest_Review.Application.Common;
using CareNest_Review.Application.Common.Options;
using CareNest_Review.Application.Features.Commands.Create;
using CareNest_Review.Application.Features.Commands.Delete;
using CareNest_Review.Application.Features.Commands.Update;
using CareNest_Review.Application.Features.Queries.GetAllPaging;
using CareNest_Review.Application.Features.Queries.GetById;
using CareNest_Review.Application.Interfaces.CQRS;
using CareNest_Review.Application.Interfaces.CQRS.Commands;
using CareNest_Review.Application.Interfaces.CQRS.Queries;
using CareNest_Review.Application.Interfaces.Services;
using CareNest_Review.Application.Interfaces.UOW;
using CareNest_Review.Application.UseCases;
using CareNest_Review.Domain.Repositories;
using CareNest_Review.Infrastructure.Persistences.Configuration;
using CareNest_Review.Infrastructure.Persistences.Database;
using CareNest_Review.Infrastructure.Persistences.Repository;
using CareNest_Review.Infrastructure.Services;
using CareNest_Review.Infrastructure.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Ưu tiên dùng DATABASE_URL; nếu không có thì fallback cấu hình DB_*
var config = builder.Configuration;
string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
string connectionString;
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfoParts = uri.UserInfo.Split(':', 2);
    var username = Uri.UnescapeDataString(userInfoParts.ElementAtOrDefault(0) ?? string.Empty);
    var password = Uri.UnescapeDataString(userInfoParts.ElementAtOrDefault(1) ?? string.Empty);
    var host = uri.Host;
    var port = uri.IsDefaultPort ? 5432 : uri.Port;
    var database = uri.AbsolutePath.TrimStart('/');

    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};Ssl Mode=Require;Trust Server Certificate=true;Pooling=true;Maximum Pool Size=5;Minimum Pool Size=0;Timeout=15;";
}
else
{
    DatabaseSettings dbSettings = new DatabaseSettings
    {
        Ip       = Environment.GetEnvironmentVariable("DB_HOST") ?? config["DatabaseSettings:Ip"],
        Port     = int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var port)
                    ? port
                    : (config.GetSection("DatabaseSettings").GetValue<int?>("Port") ?? 5432),
        User     = Environment.GetEnvironmentVariable("DB_USER") ?? config["DatabaseSettings:User"],
        Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? config["DatabaseSettings:Password"],
        Database = Environment.GetEnvironmentVariable("DB_NAME") ?? config["DatabaseSettings:Database"]
    };
    dbSettings.Display();
    connectionString = dbSettings.GetConnectionString() + ";Pooling=true;Maximum Pool Size=5;Minimum Pool Size=0;Timeout=15;";
}


// Đăng ký DbContext với PostgreSQL
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    }));

builder.Services.AddTransient<DatabaseSeeder>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Đăng ký service thêm chú thích cho api
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    //ADD JWT BEARER SECURITY DEFINITION
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        //Type = SecuritySchemeType.ApiKey,
        Type = SecuritySchemeType.Http,//ko cần thêm token phía trước
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            },
            new List<string>()
        }
    });
});

// Đăng ký các repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//command
builder.Services.AddScoped<ICommandHandler<CreateCommand, ReviewResponse>, CreateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCommand, ReviewResponse>, UpdateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteCommand>, DeleteCommandHandler>();
//query
builder.Services.AddScoped<IQueryHandler<GetAllPagingQuery, PageResult<ReviewResponse>>, GetAllPagingQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetByIdQuery, ReviewResponse>, GetByIdQueryHandler>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.Configure<APIServiceOption>(builder.Configuration.GetSection("APIService"));

builder.Services.AddHttpClient();

builder.Services.AddScoped<IServiceDetailService, ServiceDetailService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
var swaggerEnabled = app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled");
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
    if (!string.IsNullOrWhiteSpace(runMigrations) && runMigrations.Equals("true", StringComparison.OrdinalIgnoreCase))
    {
        context.Database.Migrate();
    }

    // Log tên các cột thực tế trong database để debug
    try
    {
        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = @"
            SELECT column_name 
            FROM information_schema.columns 
            WHERE table_name = 'Appointments' 
              AND table_schema = 'public'
            ORDER BY ordinal_position";
        
        context.Database.OpenConnection();
        using var reader = command.ExecuteReader();
        var columnNames = new List<string>();
        while (reader.Read())
        {
            columnNames.Add(reader.GetString(0));
        }
        context.Database.CloseConnection();

        logger.LogInformation("=== COLUMN NAMES IN DATABASE ===");
        logger.LogInformation("Table: Appointments");
        foreach (var columnName in columnNames)
        {
            logger.LogInformation("  - {ColumnName}", columnName);
        }
        logger.LogInformation("=== END COLUMN NAMES ===");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while logging column names: {Message}", ex.Message);
    }
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();