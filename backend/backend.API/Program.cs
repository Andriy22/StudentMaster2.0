using backend.API.Middleware;
using backend.API.Services;
using backend.BLL.Common.DTOs.Account;
using backend.BLL.Common.Mappings;
using backend.BLL.Services.Implementation;
using backend.BLL.Services.Interfaces;
using backend.DAL;
using backend.DAL.Entities;
using backend.DAL.Implementation;
using backend.DAL.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
{
    if (expires != null)
    {
        return DateTime.UtcNow < expires;
    }
    return false;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => { b.EnableRetryOnFailure(); b.MigrationsAssembly("backend.API"); }));

builder.Services.AddDefaultIdentity<User>()
                          .AddRoles<IdentityRole>()
                          .AddEntityFrameworkStores<DataContext>();

var identityBuilder = builder.Services.AddIdentityCore<User>(o =>
{
    // configure identity options
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
});


identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);
identityBuilder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

var KEY = builder.Configuration.GetSection("JWT").GetValue<string>("KEY");
var SSK = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = false;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidIssuer = builder.Configuration.GetSection("JWT").GetValue<string>("ISSUER"),
               ValidateAudience = true,
               ValidAudience = builder.Configuration.GetSection("JWT").GetValue<string>("AUDIENCE"),
               ValidateLifetime = true,
               LifetimeValidator = CustomLifetimeValidator,
               IssuerSigningKey = SSK,
               ValidateIssuerSigningKey = true,
           };
       });


builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<RegistrationDTOValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRazorRenderService, RazorRenderService>();
builder.Services.AddScoped<IRandomService, RandomService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(path))
{
    Directory.CreateDirectory(path);
}

FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
provider.Mappings[".json"] = "application/json";
provider.Mappings[".webmanifest"] = "application/manifest+json";

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = new PathString("/static"),
    ContentTypeProvider = provider
});

app.Run();
