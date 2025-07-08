using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;
using Repositories;
using Repositories.Interfaces;
using DataAccessObjects;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UserDAO>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
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

// Configure the HTTP request pipeline.
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
app.MapRazorPages();

app.Run();