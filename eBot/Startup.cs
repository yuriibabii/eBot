using eBot.DbContexts;
using eBot.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseMvc();

            Bot.GetBotClientAsync().Wait();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddMvcOptions(ApplyMvcOptions);

            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson();
            RegisterDbContexts(services);
            RegisterRepositories(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<IVocabularyRepository, VocabularyRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
        }

        private void RegisterDbContexts(IServiceCollection services)
        {
            services.AddDbContext<StudyContext>(
                options => options.UseSqlite(Configuration["DatabaseStrings:Study"]));
        }

        private static void ApplyMvcOptions(MvcOptions options)
        {
            options.EnableEndpointRouting = false;
        }
    }
}