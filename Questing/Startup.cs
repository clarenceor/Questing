using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Questing.Data.DAL;
using Questing.Data.IDAL;
using Questing.Data.SqlDatabaseService;
using Questing.IServices;
using Questing.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Questing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSqlDatabaseService();
            services.AddScoped<IUserQuestService, UserQuestService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddSqlDatabaseService(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IQuestDAL, QuestDAL>();
            services.AddScoped<IPlayerMilestoneDAL, PlayerMilestoneDAL>();
            services.AddScoped<IQuestPointTransactionDAL, QuestPointTransactionDAL>();
        }
    }
}
