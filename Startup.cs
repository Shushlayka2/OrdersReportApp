using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrdersReportApp.Models.Order;
using OrdersReportApp.Services;

namespace OrdersReportApp
{
    public class Startup
    {
        protected IConfiguration Configuration { get; }
        protected IWebHostEnvironment Environment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));
            services.AddTransient<IOrderDataAccess, OrderDataAccess>();
            services.AddTransient<IOrdersReporter, OrdersReporter>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();

#if DEBUG
            IMvcBuilder builder = services.AddRazorPages();
            if (Environment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Order}/{action=Index}");
            });
        }
    }
}
