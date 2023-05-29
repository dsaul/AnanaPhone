using Amazon.S3;
using AnanaPhone.AMI;
using AnanaPhone.ARI;
using AnanaPhone.AsteriskContexts;
using AnanaPhone.Boot;
using AnanaPhone.Calls;
using AnanaPhone.Conferences;
using AnanaPhone.VoiceMail;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using GraphQL.AspNet.Configuration;
using Mono.Options;
using Sander0542.Authentication.Authelia;
using Serilog;
using Serilog.Events;

namespace AnanaPhone
{
	public static class Program
	{
		public static WebApplication? Application { get; private set; }

		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
				.WriteTo.Console()
				.CreateLogger();

			bool stage1 = false;

			var options = new OptionSet {
				{ 
					"1|stage1", "run stage 1", 
					b => { 
						if (b != null) 
							stage1 = true;
					} 
				},
			};

			List<string> extra;
			try
			{
				extra = options.Parse(args);
			}
			catch (Exception e)
			{
				Log.Fatal(e, "[{Class}.{Method}()] {Message}",
					"Program",
					System.Reflection.MethodBase.GetCurrentMethod()?.Name,
					e.Message
				);
				return;
			}


			if (stage1)
				Stage1(args);
			else
				Stage2(args);
		}

		static void Stage1(string[] args)
		{
			Log.Information("[{Class}.{Method}()] AnanaPhone Stage 1 (c) 2021 Dan Saul",
				"Program",
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);

			// Basic WebApplication

			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			ApplicationSharedEarly(builder);
			ApplicationSharedSingletons(builder);

			Application = builder.Build();

			_ = Application.Services.GetRequiredService<NotSTUN.Manager>();

			Application.Services.GetRequiredService<BootManager>().GenerateForStage1();
		}

		static void ApplicationSharedEarly(WebApplicationBuilder builder)
		{
			//builder.Host.UseSerilog((HostBuilderContext ctx, LoggerConfiguration lc) =>
			//{
			//	lc.WriteTo.Console();
			//});
		}

		static void ApplicationSharedSingletons(WebApplicationBuilder builder)
		{
			builder.Services.AddSingleton<NotSTUN.Manager>();
			builder.Services.AddSingleton<ARIManager>();
			builder.Services.AddSingleton<ActiveCallManager>();
			builder.Services.AddSingleton<AMIManager>();
			builder.Services.AddSingleton<ConfBridgeManager>();
			builder.Services.AddSingleton<HistoricCallManager>();
			builder.Services.AddSingleton<BootManager>();
			builder.Services.AddSingleton<VoiceMailManager>();
			builder.Services.AddSingleton<SettingsManager.Manager>();
			builder.Services.AddSingleton<AmazonS3Config>((serviceProvider) =>
			{
				Log.Debug("S3_SERVICE_URI = {S3_SERVICE_URI}", EnvAmazonS3.S3_SERVICE_URI);
				Log.Debug("S3_FORCE_PATH_STYLE = {S3_FORCE_PATH_STYLE}", EnvAmazonS3.S3_FORCE_PATH_STYLE);

				return new()
				{
					ServiceURL = EnvAmazonS3.S3_SERVICE_URI,
					ForcePathStyle = EnvAmazonS3.S3_FORCE_PATH_STYLE
				};
			});
			builder.Services.AddSingleton<AmazonS3Client>((serviceProvider) =>
			{
				Log.Debug("S3_ACCESS_KEY = {S3_ACCESS_KEY}", EnvAmazonS3.S3_ACCESS_KEY);
				Log.Debug("S3_SECRET_KEY = {S3_SECRET_KEY}", EnvAmazonS3.S3_SECRET_KEY);

				AmazonS3Config config = serviceProvider.GetRequiredService<AmazonS3Config>();
				return new(
					EnvAmazonS3.S3_ACCESS_KEY,
					EnvAmazonS3.S3_SECRET_KEY,
					config
				);
			});

			builder.Services.AddSingleton<ConfBridgeAdmin>();
			builder.Services.AddSingleton<ConfBridgeExternal>();
			builder.Services.AddSingleton<Inbound>();
			builder.Services.AddSingleton<AsteriskContexts.Extensions>();
			builder.Services.AddSingleton<FAC>();
			builder.Services.AddSingleton<AsteriskContexts.Conference>();
			builder.Services.AddSingleton<AttendantFromExternal>();
			builder.Services.AddSingleton<AttendantDoYouAcceptTheCall>();
			builder.Services.AddSingleton<AttendantTrackedAdminDirectToConference>();
			builder.Services.AddSingleton<AttendantTrackedExternalDirectToConference>();
		}

		static void Stage2(string[] args)
		{
			Log.Information("[{Class}.{Method}()] AnanaPhone Stage 2 (c) 2021 Dan Saul",
				"Program",
				System.Reflection.MethodBase.GetCurrentMethod()?.Name
			);


			Log.Information("[{Class}.{Method}()] AMI_PW {AMI_PW}",
				"Program",
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				Env.AMI_PW
			);



			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			ApplicationSharedEarly(builder);


			// Add services to the container.

			var devCorsOrigin = "dev";
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(name: devCorsOrigin, policy =>
					policy
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod()
				);
			});

			builder.WebHost.UseUrls("http://*:5203/");
			builder.Services.AddAuthentication(AutheliaDefaults.AuthenticationScheme).AddAuthelia();
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			ApplicationSharedSingletons(builder);







			builder.Services.AddGraphQL();

			Application = builder.Build();

			if (Application.Environment.IsDevelopment())
			{
				Application.UseCors(devCorsOrigin);

			}
			//Application.UseHttpLogging();
			//Application.UseSerilogRequestLogging();

			// Configure the HTTP request pipeline.
			if (Application.Environment.IsDevelopment())
			{
				Application.UseSwagger();
				Application.UseSwaggerUI();
			}

			//Application.UseHttpsRedirection();


			Application.UseDefaultFiles(); // Must be before UseStaticFiles
			Application.UseStaticFiles();
			Application.MapFallbackToFile("/index.html");

			Application.UseRouting();
			Application.MapControllers();
			Application.UseGraphQL();


			Application.UseAuthentication();
			Application.UseAuthorization();



			


			// Pass Selected Environment Variables Through to Frontend
			Application.MapGet("/env/VITE_API_ROOT", async context =>
			{
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(Env.VITE_API_ROOT);
			});

			// Ensure that Managers are instantiated.
			_ = Application.Services.GetRequiredService<SettingsManager.Manager>();
			_ = Application.Services.GetRequiredService<HistoricCallManager>();
			_ = Application.Services.GetRequiredService<VoiceMailManager>();
			_ = Application.Services.GetRequiredService<ActiveCallManager>();
			_ = Application.Services.GetRequiredService<ARIManager>();
			_ = Application.Services.GetRequiredService<AMIManager>();
			_ = Application.Services.GetRequiredService<ConfBridgeManager>();
			_ = Application.Services.GetRequiredService<NotSTUN.Manager>();
			
			Application.Run();
		}
	}
}