using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Repositories;
using Presentation.ActionFilters;
using Domain.Interfaces;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<PollDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<PollDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<VotesActionFilter>();

            int pollSetting = 1;
            try
            {
                pollSetting = builder.Configuration.GetValue<int>("PollSetting");
            }
            catch
            {
                pollSetting = 1;
            }

            switch (pollSetting)
            {
                case 1:
                    builder.Services.AddScoped<IPollRepository, PollRepository>();
                    break;
                case 2:
                    builder.Services.AddScoped<IPollRepository, PollFileRepository>();
                    break;
                default:
                    builder.Services.AddScoped<IPollRepository, PollRepository>();
                    break;
            }

            builder.Services.AddScoped<UserVoteRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
