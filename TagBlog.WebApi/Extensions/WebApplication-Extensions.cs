using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Timing;

namespace TagBlog.WebApi.Extensions
{
	public static class WebApplication_Extensions
	{
		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddMemoryCache();

			builder.Services.AddDbContext<BlogDbContext>(option =>
			option.UseSqlServer(
				builder.Configuration
				.GetConnectionString("DefaultConnection")));

			builder.Services
				.AddScoped<ITimeProvider, ITimeProvider>();
			builder.Services
				.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
			builder.Services
				.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services
				.AddScoped<IAuthorRepository, IAuthorRepository>();
			return builder;
		}

		public static WebApplicationBuilder ConfigureCors(
			this WebApplicationBuilder builder )
		{
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("TatBlogApp", policyBuilder =>
				policyBuilder
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod());
			});
			return builder;
		}

		//Cấu hình việc sử dụng NLog
		public static WebApplicationBuilder ConfigureNLog(
			this WebApplicationBuilder builder )
		{
			builder.Logging.ClearProviders();
			builder.Host.UseNLog();

			return builder;
		}

		public static WebApplicationBuilder ConfigureSwaggerOpenApi(
			this WebApplicationBuilder builder )
		{
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			return builder;
		}

		public static WebApplication SetupRequestPipeline(
			this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}	

			app.UseStaticFiles();
			app.UseHttpsRedirection();

			app.UseCors("TatBlogApp");

			return app;
		}
	}
}
