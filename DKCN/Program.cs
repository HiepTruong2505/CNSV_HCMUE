using DKCN.DataAccess;
using DKCN.Models;
using DKCN.Services;
using DKCN.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "CNSV";
    options.IdleTimeout = new TimeSpan(0, 10, 0);
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// For Identity  

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{

    opts.Password.RequiredLength = 8;
    opts.Password.RequiredUniqueChars = 3;
    opts.Lockout.MaxFailedAccessAttempts = 5;
    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); ;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireStaffRole",
        policy => policy.RequireRole("Staff"));
    options.AddPolicy("RequireManagerRole",
        policy => policy.RequireRole("Manager"));
    options.AddPolicy("CreateRole", policy =>
        policy.RequireRole("Admin", "Manager"));
    options.AddPolicy("DeleteRole", policy =>
         policy.RequireRole("Admin", "Manager"));
    options.AddPolicy("ElevatedRights", policy =>
          policy.RequireRole("Admin", "Staff", "Manager"));

});

builder.Services.AddMvcCore(options =>
{
    //var policy = new AuthorizationPolicyBuilder()
    //                 .RequireAuthenticatedUser()
    //                 .Build();
    //options.Filters.Add(new AuthorizeFilter(policy));
}).AddXmlSerializerFormatters();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "DKCN";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(36000);
    options.LoginPath = new PathString("/Admin/Login");
    options.AccessDeniedPath = new PathString("/Admin/AccessDenied");
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<IChungnhanRepository, ChungnhanRepositoryService>();
builder.Services.AddScoped<IChungnhanKhacRepository, ChungnhanKhacRepositoryService>();


var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
