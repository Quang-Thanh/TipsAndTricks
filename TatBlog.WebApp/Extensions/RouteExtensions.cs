using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Extensions
{
	public class RouteExtensions
	{
		//Thêm các dịch vụ được yêu cầu bỏi MVC
		public static WebApplicationBuilder ConfigureMvc(
			this WebApplicationBuilder builder) 
		{
			builder.Services.AddControllersWithViews();
			builder.Services.AddResponseCompression();

			return builder;
		}

		//Đăng kí các dịch vụ với DI Container
		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder) 
		{
			builder.Services.AddDbContext<BlogDbContext>(options =>
				options.UseSqlServer(
					builder.Configuration
					.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IBlogRepository,  BlogRepository>();
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();

			return builder;
		
		}
	}
}
