using AutoMapper;
using PRN231.Repository.Implementations;
using PRN231.Repository.Interfaces;
using PRN231.Services.Implementations;
using PRN231.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231.DAL;
using System.Text.Json.Serialization;
using PRN231.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using PRN231.Models.AutoMapper;
using PRN231.Repositories.Implementations;
using PRN231.Models;
using Microsoft.AspNetCore.Identity;
using PRN231.API;
using Microsoft.Extensions.FileProviders;
using PRN231.Repositories.Interfaces;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => options.SuppressInputFormatterBuffering = true)
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 64;
    })
    .AddOData(opt => {
        var odataBuilder = new ODataConventionModelBuilder();

        odataBuilder.EntitySet<Booking>("BookingOData");
        odataBuilder.EntitySet<BookingUser>("BookingUserOData");
        odataBuilder.EntitySet<Credential>("CredentialOData");
        odataBuilder.EntitySet<Feedback>("FeedbackOData");
        odataBuilder.EntitySet<Level>("LevelOData");
        odataBuilder.EntitySet<Post>("PostOData");
        odataBuilder.EntitySet<Role>("RoleOData");
        odataBuilder.EntitySet<Schedule>("ScheduleOData");
        odataBuilder.EntitySet<Subject>("SubjectOData");
        odataBuilder.EntitySet<User>("UserOData");

        opt.AddRouteComponents("odata", odataBuilder.GetEdmModel());
        opt.Select().Filter().Expand().OrderBy().Count().SetMaxTop(100);
    });

builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddDbContext<SmartHeadContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db")));
builder.Services.AddSingleton<IMapper>(sp =>
{
    var config = new MapperConfiguration(cfg =>
    {
        // Configure your mapping profiles
        cfg.AddProfile<MappingProfile>();
    });

    return config.CreateMapper();
});

builder.Services.AddScoped<JWTService>();
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddSingleton<IMapper>(sp =>
{
    var config = new MapperConfiguration(cfg =>
    {
        // Configure your mapping profiles
        cfg.AddProfile<MappingProfile>();
    });

    return config.CreateMapper();
});

//Otp
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<OtpService>();
builder.Services.AddDistributedMemoryCache(); // Add in-memory distributed cache

builder.Services.AddIdentityCore<User>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    //password config
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    //email config
})
.AddRoles<Role>()
.AddRoleManager<RoleManager<Role>>()
.AddEntityFrameworkStores<SmartHeadContext>()
.AddSignInManager<SignInManager<User>>()
.AddUserManager<UserManager<User>>()
.AddDefaultTokenProviders(); //token for email confirmation

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IFileStorageService, FileStorageService>();
builder.Services.AddTransient(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure cookies are sent over HTTPS
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HttpContext
builder.Services.AddHttpContextAccessor();

// Repositories DI
//builder.Services.AddScoped<IGenericRepository<Booking>, BookingRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingUserRepository, BookingUserRepository>();

// Services DI
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c => {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
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
    }
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
            )),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddCors(options => {
    options.AddPolicy(name: "MyAllowPolicy",
        policy => {
            policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "UploadedFiles")),
    RequestPath = ""
});

app.UseHttpsRedirection();
app.UseCors("MyAllowPolicy");

app.UseAuthentication();

app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();
