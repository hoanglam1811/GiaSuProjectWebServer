using AutoMapper;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.OData.ModelBuilder;
using PRN231.DAL;
using PRN231.Models;
using PRN231.Models.AutoMapper;
using PRN231.Repository.Implementations;
using PRN231.Repository.Interfaces;
using PRN231.Services.Implementations;
using PRN231.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Level>("Levels");
modelBuilder.EntitySet<Booking>("Bookings");
modelBuilder.EntitySet<BookingUser>("BookingUsers");
modelBuilder.EntitySet<Credential>("Credentials");
modelBuilder.EntitySet<Feedback>("Feedbacks");
modelBuilder.EntitySet<Post>("Posts");
modelBuilder.EntitySet<Role>("Roles");
modelBuilder.EntitySet<Schedule>("Schedules");
modelBuilder.EntitySet<Subject>("Subjects");
modelBuilder.EntitySet<User>("Users");
var edmModel = modelBuilder.GetEdmModel();

builder.Services.AddControllers()
    .AddOData(options =>
        options.EnableQueryFeatures(null).AddRouteComponents("odata", edmModel));

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient(typeof(IGenericService<,>), typeof(GenericService<,>));
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
