using BusinessObjects.Models;
using DataAccessObjects;
using GenderHealthcareServiceManagementSystemPages.Hubs;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Repositories;
using Repositories.Interfaces;
using Services;
using BusinessObjects.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages + SignalR
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Register DAO
builder.Services.AddScoped<TestDAO>();
builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<ClinicDAO>();
builder.Services.AddScoped<ServiceDAO>();
builder.Services.AddScoped<FeedbackDAO>();
builder.Services.AddScoped<QuestionDAO>();
builder.Services.AddScoped<MenstrualCycleDAO>();
builder.Services.AddScoped<ConsultantInfoDAO>();
builder.Services.AddScoped<ConsultationDAO>();
builder.Services.AddScoped<ReminderDAO>();

// Register Repository
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IMenstrualCycleRepository, MenstrualCycleRepository>();
builder.Services.AddScoped<IConsultantInfoRepository, ConsultantInfoRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

// Register Services
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IMenstrualCycleService, MenstrualCycleService>();
builder.Services.AddScoped<IConsultantInfoService, ConsultantInfoService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();

// Database
builder.Services.AddDbContext<GenderHealthcareContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Email Settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddTransient<IEmailService, EmailService>();

//Reminder Service
builder.Services.AddHostedService<ReminderEmailService>();

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware
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

// ===== SEED ACCOUNT + CONSULTANT INFO =====
using (var scope = app.Services.CreateScope())
{
    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
    var consultantInfoService = scope.ServiceProvider.GetRequiredService<IConsultantInfoService>();
    await SeedAccountsAsync(accountService, consultantInfoService);
}

app.Run();

// ===== Seed Function =====
static async Task SeedAccountsAsync(IAccountService accountService, IConsultantInfoService consultantInfoService)
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
        var exists = await accountService.LoginAsync(user.Username, user.PasswordHash);
        if (exists == null)
        {
            var result = await accountService.RegisterAsync(user);
            Console.WriteLine($"[SEED] Tạo tài khoản {user.Username}: {(result ? "✅ Thành công" : "❌ Thất bại")}");
            exists = await accountService.LoginAsync(user.Username, user.PasswordHash);
        }
        else
        {
            Console.WriteLine($"[SEED] Tài khoản {user.Username} đã tồn tại.");
        }

        // Tạo ConsultantInfo nếu là tư vấn viên và chưa có
        if (exists != null && user.Role == "Consultant")
        {
            var existingInfo = await consultantInfoService.GetConsultantInfoByIdAsync(exists.UserId);
            if (existingInfo == null)
            {
                var consultantInfo = new ConsultantInfo
                {
                    ConsultantId = exists.UserId,
                    Qualifications = "Bác sĩ đa khoa",
                    ExperienceYears = 5,
                    Specialization = "Sức khỏe giới tính",
                    CreatedAt = DateTime.Now
                };
                await consultantInfoService.CreateConsultantInfoAsync(consultantInfo);
                Console.WriteLine($"[SEED] ✅ Tạo ConsultantInfo cho {user.Username}");
            }
            else
            {
                Console.WriteLine($"[SEED] 🔁 ConsultantInfo của {user.Username} đã tồn tại");
            }
        }
    }
}
