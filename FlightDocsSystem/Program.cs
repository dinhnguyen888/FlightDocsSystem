using FlightDocsSystem.Data;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Services;
using FlightDocsSystem.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using FlightDocsSystem.Filters;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using FlightDocsSystem.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
    );

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your token."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    options.MapType<FlightStatuses>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(FlightStatuses))
                    .Select(name => new OpenApiString(name))
                    .Cast<IOpenApiAny>()
                    .ToList()
    });


    options.ResolveConflictingActions(a => a.First());



});



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IDocsTypeService, DocsTypeService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICheckInput, CheckInputService>();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),

            //Do mac dinh JWT su dung Sub lam NameIdentifier nen gio dung thang nay ben context
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    });



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin"); 
    });

    options.AddPolicy("PilotOnly", policy =>
    {
        policy.RequireRole("Pilot"); 
    });

    options.AddPolicy("PilotAndAdminOnly", policy =>
    {
        policy.RequireRole("Admin", "Pilot"); 
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.RequireAuthenticatedUser(); // Phai co token
    });


    options.AddPolicy("ActiveToken", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var isActiveClaim = context.User.FindFirst(c => c.Type == "IsActive");
            return isActiveClaim != null && isActiveClaim.Value == "True";
        });
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/uploads"
});

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.CanConnect())
    {
        Console.WriteLine("Database connection OK.");
    }
    else
    {
        Console.WriteLine("Database connection failed.");
    }
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<CheckTokenMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
