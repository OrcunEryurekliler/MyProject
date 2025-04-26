using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Application.Services.Specific;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data;
using MyProject.Infrastructure.Data.Seed;
using MyProject.Infrastructure.Data.Repositories.Generic;
using MyProject.Infrastructure.Data.Repositories.Specific;
using MyProject.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("MyProject.Infrastructure")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IDoctorUnavailabilityRepository, DoctorUnavailabilityRepository>();
builder.Services.AddScoped<IDoctorUnavailabilityService, DoctorUnavailabilityService>();
builder.Services.AddScoped<IDoctorProfileRepository, DoctorProfileRepository>();
builder.Services.AddScoped<IDoctorProfileService, DoctorProfileService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.SlidingExpiration = true; // Her istekle süreyi uzatýr
});

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Giriþ sayfanýzýn yolu
});
// DTO Mapping Ayarlarý
builder.Services.AddAutoMapper(typeof(AppointmentProfile).Assembly);
builder.Services.AddAutoMapper(typeof(AppointmentViewProfile).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Program.cs içinde, WebUI katmanýnýzda:
builder.Services
    .AddHttpClient<IAppointmentApiClient, AppointmentApiClient>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
    });

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Roller oluþtur
    await IdentitySeed.SeedRolesAsync(services);
    // Ýsteðe baðlý: Admin kullanýcýsý oluþtur
    await IdentitySeed.SeedAdminUserAsync(services);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // BU SATIRI EKLE

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
