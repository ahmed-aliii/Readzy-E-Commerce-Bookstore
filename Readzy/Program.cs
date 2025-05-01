using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Readzy.DataAccess.Data;
using Readzy.DataAccess.Repository.CategoryRepository;
using Readzy.DataAccess.Repository.ProductRepository;
using Readzy.Utility;
using Microsoft.AspNetCore.Identity;
using Readzy.Models.Entities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Readzy.DataAccess.Repository.ShoppingCartRepository;
using Readzy.DataAccess.Repository.ApplicationUserRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Readzy.DataAccess.Repository.OrderHeaderRepository;
using Readzy.DataAccess.Repository.OrderDetailRepository;
using Stripe;
using Microsoft.AspNetCore.Authentication.Facebook;


namespace Readzy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the Ioc Container.
           
            #region Built-In Services

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ReadzyContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ReadzyContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/User/Account/AccessDenied"; // Update for your area/controller
            });


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Account/Login";
                options.AccessDeniedPath = "/User/Account/AccessDenied";
            });



            //Stripe Settings
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            #endregion

            #region Custom Services
            //Registration 
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<IOrderHeaderRepository , OrderHeaderRepository>();
            builder.Services.AddScoped<IOrderDetailRepository , OrderDetailRepository>();
            #endregion


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

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
