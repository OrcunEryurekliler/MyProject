using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interfaces;
using MyProject.Application.Mappings;
using MyProject.Application.Services.Generic;
using MyProject.Application.Services.Specific;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data;
using MyProject.Infrastructure.Data.Repositories.Generic;
using MyProject.Infrastructure.Data.Repositories.Specific;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.MigrationsAssembly("MyProject.Infrastructure")));

// Identity (opsiyonel, eðer API'de de login yapacaksan)
//builder.Services.AddIdentity<User, IdentityRole<int>>()...
//    .AddEntityFrameworkStores<AppDbContext>();

// Repos ve Servisler
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorProfileRepository, DoctorProfileRepository>();
builder.Services.AddScoped<IDoctorProfileService, DoctorProfileService>();
builder.Services.AddScoped<IDoctorUnavailabilityRepository, DoctorUnavailabilityRepository>();
builder.Services.AddScoped<IDoctorUnavailabilityService, DoctorUnavailabilityService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AppointmentProfile).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseHttpsRedirection();
app.UseAuthorization();
// Swagger (Sadece Development'da açýk kalsýn)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
