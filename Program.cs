using System.Text;
using AmIAuthorised.DataAccessLayer.Database;
using AmIAuthorised.Repository;
using AmIAuthorised.Service;
using AmIAuthorised.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<AmIAuthorisedContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AmIAuthorisedConnectionString")));

builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(AbstractRepository).Assembly)
    .AddClasses(c => c.AssignableTo<AbstractRepository>())
    .AsSelf()
    .WithScopedLifetime());

builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(AbstractService).Assembly)
    .AddClasses(c => c.AssignableTo<AbstractService>())
    .AsSelf()
    .WithScopedLifetime());

builder.Services.AddScoped<CurrentUser>();
builder.Services.AddScoped<JwtToken>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddLocalization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        //ValidAudience = builder.Configuration["JwtSettings:Audience"],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidIssuer = "balasubramanya",
        ValidAudience = "wholeworldglobe",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("balasubramanyaauthorisemebalasubramanya")),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("USER_CREATE", policy => policy.RequireClaim("permissions", "USER_CREATE"));
    options.AddPolicy("USER_VIEW_ALL", policy => policy.RequireClaim("permissions", "USER_VIEW_ALL"));
    options.AddPolicy("ROLE_VIEW_ALL", policy => policy.RequireClaim("permissions", "ROLE_VIEW_ALL"));

    options.AddPolicy("APPLICATION_VIEW", policy => policy.RequireClaim("permissions", "APPLICATION_VIEW"));
    options.AddPolicy("APPLICATION_VIEW_ALL", policy => policy.RequireClaim("permissions", "APPLICATION_VIEW_ALL"));
    options.AddPolicy("APPLICATION_CREATE", policy => policy.RequireClaim("permissions", "APPLICATION_CREATE"));
    options.AddPolicy("APPLICATION_EDIT", policy => policy.RequireClaim("permissions", "APPLICATION_EDIT"));
    options.AddPolicy("APPLICATION_SUBMIT", policy => policy.RequireClaim("permissions", "APPLICATION_SUBMIT"));
    options.AddPolicy("APPLICATION_ACTION", policy => policy.RequireClaim("permissions", "APPLICATION_ACTION"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "n.balasubramanya", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "n.balasubramanya please enter 'Bearer' followed by your JWT token"
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "n.balasubramanya v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
