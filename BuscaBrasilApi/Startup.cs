using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using BuscaBrasilApi.Services;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using BuscaBrasilApi.Infrastructure;
using BuscaBrasilApi.Factories;
using System.IO;
using System.Reflection;
using System;
using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.CSI.BuscaBrasilApi.Factories;

namespace BuscaBrasilApi
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

            DataAccess data = new DataAccess(Configuration.GetSection("ConnectionStrings")["Conexao"]);

            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "BRL";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            var builder = WebApplication.CreateBuilder();

            services.AddControllers();

            string ambiente = builder.Configuration.GetSection("Environment").Value;
            FactoryConfiguracao fConfig = new FactoryConfiguracao();
            IConfiguracao config = fConfig.Build(ambiente);

            //injeção de dependencia é realizada aqui
            services.AddSingleton(sp => config);
            services.AddSingleton<IAeroportosService, AeroportoService>();
            services.AddSingleton<ICidadesService, CidadeService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = string.Concat("BUSCA BRASIL API - Ambiente de ", Configuration.GetSection("ConfiguracaoAPI")["environment"]),
                    Version = "v1",
                    Description = "API ira consultar condições climaticas em uma determinada cidade ou determinado aeroporto"   
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            FactoryGravaLog factoryGravaLog = new FactoryGravaLog();
            IGravaLog gravaLog = factoryGravaLog.Build(config);
            
            FactoryConfiguracao fconfig = new FactoryConfiguracao();

            builder.Services.AddSingleton(sp => gravaLog);
            builder.Services.AddSingleton(sp => config);

            services.AddLogging(options =>
            {
                options.AddSimpleConsole(c =>
                {
                    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                });
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR"), new CultureInfo("pt-BR") };
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(x => x
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .SetIsOriginAllowed(origin => true) // allow any origin
                  );

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
