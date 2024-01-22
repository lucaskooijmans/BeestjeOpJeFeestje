using Domain.Data;
using Domain.Models;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using PROG6_BeestjeOpJeFeestje.Mappings;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PROG6_BeestjeOpJeFeestje.Authorization;

namespace PROG6_BeestjeOpJeFeestje;
[ExcludeFromCodeCoverage]
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        var connectionString = Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        // Register IUserRepository and UserRepository
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        
        services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        services.AddControllersWithViews();
        
        services.AddMvc()
            .AddSessionStateTempDataProvider();

        services.AddDistributedMemoryCache();
        services.AddSession();
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.Requirements.Add(new AdminRequirement()));
        });

        services.AddSingleton<IAuthorizationHandler, AdminAuthorizationHandler>();


        // Configure the HTTP request pipeline.

        services.AddSingleton(Configuration);
        
        services.AddAutoMapper(typeof(Maps));

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
    {
        if (env.IsDevelopment())
        {
            context.Database.Migrate();
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
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
        


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}