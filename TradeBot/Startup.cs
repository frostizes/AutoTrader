using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BuisnessLogicLayer.BL;
using BuisnessLogicLayer.CmcApiManager;
using BuisnessLogicLayer.CoinMarketCap;
using BuisnessLogicLayer.JwtTokenGenerator;
using ContractEntities.Entities;
using DataAccessLayer.DBConfig;
using DataAccessLayer.Repository;
using ISSHost.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServiceAccessLayer.Boot;
using ServiceAccessLayer.CmcService;
using ServiceAccessLayer.CoinMarketCapService;
using ServiceAccessLayer.CoinMarketCapService.Mapper;
using ServiceAccessLayer.Mapper;
using Util.Configuration;
using Utils.CacheHelper;

namespace TradeBot
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
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.Configure<CoinMarketCapOptions>(Configuration.GetSection("CoinMarketCap"));
            services.Configure<BootOptions>(Configuration.GetSection("BootCoinMarketCapService"));
            services.AddDbContext<AppDbContext>(options => options.UseLazyLoadingProxies());
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = false
                };
            });
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddUserManager<ApplicationUserRepository>();



            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "Redis_";
            });

            services.AddControllers();
            InitServices(services);
            services.AddTransient<ICoinMarketCapMapper, CoinMarketCapMapper>();
            services.AddTransient<ICoinMarketCapServiceBoot, CoinMarketCapServiceBoot>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<ICacheManager, CacheManager>();
            services.AddTransient<ICoinMarketCapBL, CoinMarketCapBL>();
            services.AddTransient<ICoinMarketCapController, CoinMarketCapController>();
            services.AddTransient<ICryptoRepository, CryptoRepository>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TradeBot", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeBot v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ApplicationServices.GetService<ICoinMarketCapServiceBoot>().Start();
        }

        private void InitServices(IServiceCollection services)
        {

            services.AddTransient<ICoinMarketCapClient, CoinMarketCapClient>();
            services.AddTransient<ICoinMarketCapAgent, CoinMarketCapAgent>();
            services.AddHttpClient<ICoinMarketCapClient, CoinMarketCapClient>((serviceProvider, client) =>
            {
                var clientOptions = serviceProvider.GetService<IOptions<CoinMarketCapOptions>>().Value;
                client.BaseAddress = !string.IsNullOrEmpty(clientOptions.BaseUrl)
                    ? new Uri(clientOptions.BaseUrl.TrimEnd('/') + '/')
                    : null;
                client.DefaultRequestHeaders.Add(clientOptions.CoinMarketCapAPIKey,clientOptions.CoinMarketCapAPIValue);
            });
        }
    }
}
