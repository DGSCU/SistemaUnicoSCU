
using Logger.Data;
using Logger.Output;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using RegistrazioneSistemaUnico.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RegistrazioneSistemaUnico
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
			services.AddDbContext<RegistrazioneContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("RegistrazioneDB"))
			);
			services.AddDbContext<LogContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("LogDB"))
			);

			services.AddControllersWithViews().AddRazorRuntimeCompilation();

			//Configuarazione Log
			Logger.Logger.SetApplicationName("Registrazione");

			var logFile = new LogFile(
				Configuration.GetValue<string>("Logs:LogFile:Path"), 
				"FileLog@Date@_.txt",
				"",
				1);
			//logFile.SetPathForLevel(Logger.Data.LogLevel.Levels.Error, Configuration.GetValue<string>("Logs:LogFile:PathError"));
			LogDB logDB = new LogDB(
				Configuration.GetConnectionString("LogDB"));
			
			LogNextView LogNextView = new LogNextView(
				Configuration.GetValue<string>("Logs:LogNextView:Endpoint"),
				Configuration.GetValue<string>("Logs:LogNextView:Queue"),
				Configuration.GetValue<string>("Logs:LogNextView:QueueUsername"),
				Configuration.GetValue<string>("Logs:LogNextView:QueuePassword"));
			Logger.Logger.AddOutput(LogNextView);
			Logger.Logger.AddOutput(logDB);
			Logger.Logger.AddOutput(logFile);


			Parametri.CaricaParametri(Configuration);

			services.AddControllersWithViews();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IViewRenderService, ViewRenderService>();
			//services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();

			//Aggiunge un gestore custom per le pagine (su caricamento e chiusura)
			services.AddMvc(options =>
			{

				options.Filters.Add(new CustomActionFilter());

			});
			//services.AddAuthentication(Configuration.GetValue<string>("Authentication"));

			/* Configurazione autenticazione con Token JWT*/
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
					.AddJwtBearer(x =>
					{
						x.RequireHttpsMetadata = true;
						x.SaveToken = true;
						x.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateLifetime = true,
							LifetimeValidator = TokenService.LifetimeValidator,
							ValidateIssuerSigningKey = true,
							IssuerSigningKey = TokenService.Key,
							ValidateIssuer = false,
							ValidateAudience = false
						};
						x.Events = new JwtBearerEvents
						{
							OnMessageReceived = context =>
							{
								context.Token = context.Request.Cookies["jwtToken"];
								return Task.CompletedTask;
							},
						};
					});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Utente", policy => policy.RequireClaim("User"));
			});


			

			services.AddDistributedMemoryCache();
			//services.AddControllersWithViews().AddRazorRuntimeCompilation();

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(3600);
				//options.Cookie.HttpOnly = true;
				//options.Cookie.IsEssential = true;
			});

			var cultureInfo = new CultureInfo("it-IT");
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			string subsitePath=Configuration.GetValue<string>("SubSite");
			if (!string.IsNullOrEmpty(subsitePath))
			{
				subsitePath = "/" + subsitePath;
				app.UsePathBase(subsitePath);
				app.UseStaticFiles();
			}
			app.UseSession();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler($"/Error");
				app.UseHsts();
			}
			//Gestione pagine di errore
			app.UseStatusCodePages(async context => { ErrorHandler.DefalultHandler(context); });
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
			Helpers.Configuration.SubSite= Configuration.GetValue<string>("SubSite");


			TokenService.Config(
				Configuration.GetValue<int>("Token:LifetimeMinutes"),
				Configuration.GetValue<string>("Token:PasswordKey"));
			Comuni.SetConnectionString(Configuration.GetConnectionString("RegistrazioneDB"));
		}
	}
}
