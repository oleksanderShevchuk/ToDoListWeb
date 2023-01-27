using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ToDoListWeb.Authorize;
using ToDoListWeb.Data;
using ToDoListWeb.Filters;
using ToDoListWeb.Service;

namespace ToDoListWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders().AddDefaultUI();
            services.AddTransient<IEmailSender, MailJetEmailSender>(); 
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
            });
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
            //});
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "661653268910622";
                options.AppSecret = "ff8cc93759443ddc6dd9d1c6b56543ee";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyAccess.UserClaimOrAdmin, policy => policy.RequireAssertion(context =>
                AuthorizeUser(context)));

            });
            services.AddControllersWithViews(option => option.Filters.Add(new ValidationFilter()));
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    // authentication
            app.UseAuthorization();     // authorization

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private bool AuthorizeUser(AuthorizationHandlerContext context)
        {
            return ( context.User.HasClaim(c => c.Type == "Create" && c.Value == bool.TrueString)
                        || context.User.HasClaim(c => c.Type == "Edit" && c.Value == bool.TrueString)
                        || context.User.HasClaim(c => c.Type == "Delete" && c.Value == bool.TrueString)
                    ) || context.User.IsInRole(RoleAccess.Admin);
        }
    }
}
