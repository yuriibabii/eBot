using eBot.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eBot
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseMvc();
            
            Bot.GetBotClientAsync().Wait();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddMvcOptions(ApplyMvcOptions);
            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson();
        }

        private static void ApplyMvcOptions(MvcOptions options)
        {
            options.EnableEndpointRouting = false;
        }
    }
}