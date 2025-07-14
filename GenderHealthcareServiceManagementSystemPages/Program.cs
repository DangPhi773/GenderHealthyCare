using BusinessObjects.Models;
using DataAccessObjects;
using GenderHealthcareServiceManagementSystemPages.Hubs;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;
using Repositories;
using Repositories.Interfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);

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

// Register Repositories
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IMenstrualCycleRepository, MenstrualCycleRepository>();
builder.Services.AddScoped<IConsultantInfoRepository, ConsultantInfoRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
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
builder.Services.AddScoped<IQuestionService, QuestionService>();

builder.Services.AddDbContext<GenderHealthcareContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


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

app.Run();