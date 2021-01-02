using Chirp.Data;
using Chirp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Setup
{
    public class DataAccessInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Identity")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>();
            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Data")));
            services.AddDbContext<AcceptedJobsContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("AcceptedJobs")));

            services.AddSingleton<IPostService, PostService>();
        }
    }
}
