using BAI5_CONGD.Data;
using BAI5_CONGD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BAI5_CONGD
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Đăng ký DbContext
            builder.Services.AddDbContext<BookDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký Identity với ApplicationUser
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BookDbContext>()
                .AddDefaultTokenProviders();

            // Cấu hình đường dẫn khi bị từ chối truy cập
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // Đăng ký dịch vụ gửi email giả
            builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

            var app = builder.Build();

            // Tạo role và user mặc định
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Admin", "Member" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Tạo tài khoản Admin nếu chưa có
                var adminEmail = "admin@email.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Admin User",
                        Address = "Admin Address"
                    };
                    await userManager.CreateAsync(adminUser, "Admin@123");
                }
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                // Tạo tài khoản Member nếu chưa có
                var memberEmail = "member@email.com";
                var memberUser = await userManager.FindByEmailAsync(memberEmail);
                if (memberUser == null)
                {
                    memberUser = new ApplicationUser
                    {
                        UserName = "member",
                        Email = memberEmail,
                        EmailConfirmed = true,
                        FullName = "Member User",
                        Address = "Member Address"
                    };
                    await userManager.CreateAsync(memberUser, "Member@123");
                }
                if (!await userManager.IsInRoleAsync(memberUser, "Member"))
                {
                    await userManager.AddToRoleAsync(memberUser, "Member");
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }

    // FakeEmailSender để tránh lỗi khi chưa cấu hình gửi email thật
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"SendEmailAsync to {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}