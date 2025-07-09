using BusinessObjects.Models;
using DataAccessObjects;
using GenderHealthcareServiceManagementSystemPages.Hubs;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;
using Repositories;
using Repositories.Interfaces;
using DataAccessObjects;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<ClinicDAO>();
builder.Services.AddScoped<ServiceDAO>();
builder.Services.AddScoped<MenstrualCycleDAO>();
builder.Services.AddScoped<ConsultantInfoDAO>();
builder.Services.AddScoped<ConsultationDAO>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IMenstrualCycleRepository, MenstrualCycleRepository>();
builder.Services.AddScoped<IConsultantInfoRepository, ConsultantInfoRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IMenstrualCycleService, MenstrualCycleService>();
builder.Services.AddScoped<IConsultantInfoService, ConsultantInfoService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();

builder.Services.AddDbContext<GenderHealthcareContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapHub<SignalRServer>("/SignalRServer");

app.MapRazorPages();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IAccountService>();
    await SeedAccountsAsync(service);
}
app.Run();
static async Task SeedAccountsAsync(IAccountService service)
{
    var defaultUsers = new List<User>
    {
        new User
        {
            Username = "admin",
            Email = "admin1@example.com",
            PasswordHash = "1",
            FullName = "Quản trị viên A",
            Phone = "0909000001",
            Role = "Admin",
        },
        new User
        {
            Username = "manager",
            Email = "manager1@example.com",
            PasswordHash = "1",
            FullName = "Trưởng phòng B",
            Phone = "0909000002",
            Role = "Manager",
        },
        new User
        {
            Username = "staff",
            Email = "staff1@example.com",
            PasswordHash = "1",
            FullName = "Nhân viên C",
            Phone = "0909000003",
            Role = "Staff",
        },
        new User
        {
            Username = "consultant",
            Email = "consultant1@example.com",
            PasswordHash = "1",
            FullName = "BS. Nguyễn Văn Tư",
            Phone = "0909000004",
            Role = "Consultant",
        }
    };

    foreach (var user in defaultUsers)
    {
        var exists = await service.LoginAsync(user.Username, user.PasswordHash);
        if (exists == null)
        {
            var result = await service.RegisterAsync(user);
            Console.WriteLine($"[SEED] Tạo tài khoản {user.Username}: {(result ? "✅ Thành công" : "❌ Thất bại")}");
        }
        else
        {
            Console.WriteLine($"[SEED] Tài khoản {user.Username} đã tồn tại.");
        }
    }
}
