using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Timing;

namespace TatBlog.WebApi.Extensions
{
	public static class WebApplicationExtensions
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

		public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
		{
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("TatBlogApp", policyBuilder => policyBuilder
							)
			}
		}

	}
}
