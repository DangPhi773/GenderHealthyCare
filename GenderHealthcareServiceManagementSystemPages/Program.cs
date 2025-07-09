using BusinessObjects.Models;
using DataAccessObjects;
using GenderHealthcareServiceManagementSystemPages.Hubs;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<ConsultantInfoDAO>();
builder.Services.AddScoped<ConsultationDAO>();
builder.Services.AddScoped<IConsultantInfoRepository, ConsultantInfoRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IConsultantInfoService, ConsultantInfoService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddDbContext<GenderHealthcareContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapHub<SignalRServer>("/SignalRServer");

app.MapRazorPages();

app.Run();
