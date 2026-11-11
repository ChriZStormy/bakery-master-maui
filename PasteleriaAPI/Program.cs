using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasteleriaAPI.Authorization;
using PasteleriaAPI.Data;
using PasteleriaAPI.Entities;
using PasteleriaAPI.Hubs;
using PasteleriaAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- Database ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- ASP.NET Core Identity ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// --- JWT Authentication ---
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key not configured. Set 'Jwt:Key' in appsettings.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // TODO: Set true in production with valid cert
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

// --- Authorization Policies ---
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, LocalNetworkAuthorizationHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LocalNetworkOnly", policy =>
        policy.Requirements.Add(new LocalNetworkRequirement()));
});

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5105",
                "https://localhost:7022",
                "http://10.0.2.2:5105"  // Android emulator
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// --- SignalR Real-Time Communications ---
builder.Services.AddSignalR();

// --- Options Pattern ---
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// --- Application Services ---
builder.Services.AddScoped<IPastelService, PastelService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// --- Controllers ---
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddOpenApi();

// --- Kestrel TLS ---
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        listenOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
    });
});

var app = builder.Build();

// --- Seed Database ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedAsync(scope.ServiceProvider, app.Configuration);
}

// --- Middleware Pipeline ---
app.UseCors("DefaultPolicy");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection(); // TODO: Enable in production

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<PedidoHub>("/hubs/pedidos");
app.MapControllers();

app.Run();