using Amazon.S3;
using AnanaPhone.AMI;
using AnanaPhone.ARI;
using AnanaPhone.Calls;
using AnanaPhone.Conferences;
using AnanaPhone.VoiceMail;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using GraphQL.AspNet.Configuration;
using Sander0542.Authentication.Authelia;
using Serilog;
using Serilog.Events;

namespace AnanaPhone
{
	public class Program
	{
		public static WebApplication? Application { get; private set; }

		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.Enrich.WithMachineName()
				.Enrich.FromLogContext()
				.Enrich.WithProcessId()
				.Enrich.WithThreadId()
				.Enrich.WithMachineName()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
				.WriteTo.Console()
				.CreateLogger();


			Log.Information("[{Class}.{Method}()] AnanaPhone (c) 2021 Dan Saul",
					"Program",
					System.Reflection.MethodBase.GetCurrentMethod()?.Name
				);

			var builder = WebApplication.CreateBuilder(args);

			//builder.Host.UseSerilog((HostBuilderContext ctx, LoggerConfiguration lc) =>
			//{
			//	lc.WriteTo.Console();
			//});

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


			builder.Services.AddAuthentication(AutheliaDefaults.AuthenticationScheme).AddAuthelia();
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSingleton<ARIManager>();
			builder.Services.AddSingleton<ActiveCallManager>();
			builder.Services.AddSingleton<AMIManager>();
			builder.Services.AddSingleton<ConfBridgeManager>();
			builder.Services.AddSingleton<HistoricCallManager>();
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


			builder.Services.AddGraphQL();

			Application = builder.Build();

			if (Application.Environment.IsDevelopment())
			{
				Application.UseCors(devCorsOrigin);
			}

			//Application.UseSerilogRequestLogging();

			// Configure the HTTP request pipeline.
			if (Application.Environment.IsDevelopment())
			{
				Application.UseSwagger();
				Application.UseSwaggerUI();
			}

			Application.UseHttpsRedirection();

			
			Application.UseDefaultFiles(); // Must be before UseStaticFiles
			Application.UseStaticFiles();
			//Application.MapFallbackToFile("/index.html");
			Application.UseRouting();
			Application.MapControllers();
			Application.UseGraphQL();

			

			Application.UseAuthentication();
			Application.UseAuthorization();

			

			

			// Pass Selected Environment Variables Through to Frontend
			//Application.MapGet("/env/VITE_MQTT_HOST", async context => {
			//	context.Response.ContentType = "text/plain";
			//	await context.Response.WriteAsync(Konstants.VITE_MQTT_HOST ?? "");
			//});

			// Ensure that Managers are instantiated.
			_ = Application.Services.GetRequiredService<SettingsManager.Manager>();
			_ = Application.Services.GetRequiredService<HistoricCallManager>();
			_ = Application.Services.GetRequiredService<VoiceMailManager>();
			_ = Application.Services.GetRequiredService<ActiveCallManager>();
			_ = Application.Services.GetRequiredService<ARIManager>();
			_ = Application.Services.GetRequiredService<AMIManager>();
			_ = Application.Services.GetRequiredService<ConfBridgeManager>();
			
			Application.Run();
		}
	}
}