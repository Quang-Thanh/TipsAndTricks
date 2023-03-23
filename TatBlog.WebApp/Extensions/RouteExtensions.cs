using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Extensions
{
	public static class RouteExtensions
	{
		//Định nghĩa route template, route constraint cho các
		//endpoint kết hợp với cá action trong các controller

		public static IEndpointRouteBuilder UseBlogRoutes(
			this IEndpointRouteBuilder endpoint)
		{
			endpoint.MapControllerRoute(
				name: "post-by-Author",
				pattern: "blog/Author/{slug}",
				defaults: new { controller = "Blog", action = "Author" });

			endpoint.MapControllerRoute(
				name: "post-by-category",
				pattern: "blog/category/{slug}",
				defaults: new { controller = "Blog", action = "Category" });

			endpoint.MapControllerRoute(
				name: "post-by-tag",
				pattern: "blog/tag/{slug}",
				defaults: new { controller = "Blog", action = "Tag" });


			endpoint.MapAreaControllerRoute(
				name: "admin",
				areaName: "admin",
				pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}");

            endpoint.MapControllerRoute(
                name: "admin-area",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            endpoint.MapControllerRoute(
				name: "single-post",
				pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
				defaults: new { controller = "Blog", action = "Post" });




			endpoint.MapControllerRoute(
				name: "default",
				pattern: "{controller=Blog}/{action=Index}/{id?}");


			return endpoint;
		}
	}
}
