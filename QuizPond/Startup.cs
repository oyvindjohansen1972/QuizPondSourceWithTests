using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using QuizPond.Data.Interfaces;
using QuizPond.Data.Repositories;
using QuizPond.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using QuizPond.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QuizPond
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;
       
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // in 2.1 you can do -> services.AddHttpContextAccessor();

            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:QuizPond:ConnectionString"]));

           // services.AddDbContextPool<IdentityDbContext>(options => options.UseSqlServer(Configuration["Data:QuizPondIdentity:ConnectionString"]));            


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<PasswordHasherOptions>(options =>
            {
                options.IterationCount = 20000;
            });

            services.AddTransient<SessionUtility>();

            services.AddTransient<IQuestionRepository, EFQuestionRepository>();
            services.AddMvc();
           // services.AddMemoryCache();
          //  services.AddSession();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Main/Index");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: null, template: "{controller=Main}/{action=Index}/{id?}");
            });

           // SeedData.EnsurePopulated(app);




        }
    }
}


