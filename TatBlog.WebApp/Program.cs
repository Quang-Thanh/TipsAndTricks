using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{
	//Thêm các dịch vụ đucợ yên cầu bới MVC Framework
	builder.Services.AddControllersWithViews();

	//Đăng ký các dịch vụ với DI Container
	builder.Services.AddDbContext<BlogDbContext>(options =>
		options.UseSqlServer(
			builder.Configuration.GetConnectionString("DefaultConnection")));
	builder.Services.AddScoped<IBlogRepository, BlogRepository>();
	builder.Services.AddScoped<IDataSeeder, DataSeeder>();
}
var app = builder.Build();
{
	//Cấu hình HTTP Request pipeline

	// Thêm middleware để hiện thị thông báo lỗi
	if(app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}	
	else
	{
		app.UseExceptionHandler("/Blog/Eror");

		//Thêm middleware cho việc áp dụng HSTS (thêm header
		// Strict-Transport-Security vào HTTP Respose).
		app.UseHsts();
	}	

	//Thêm middleware để chuyển hướng HTTP sáng HTTPS
	app.UseHttpsRedirection();

	//Thêm middleware phục vụ các yêu cầu liên quan
	// tới các tin nội dung tĩnh như hình ảnh, css,...
	app.UseStaticFiles();

	//Thêm middleware lựa chọn endpoint phù hợp nhất
	//để xử lý một HTTP request.
	app.UseRouting();

	//Định nghĩa router template, route constraint cho các
	//endpoints kết hợp với các action trong các controller


	app.MapControllerRoute(
			//name: "default",
			//pattern: "{controller=Blog}/{action=Index}/{id?}"
			name: "post-by-category",
			pattern: "blog/category/{slug}",
			defaults: new { controller = "Blog", action = "Category"});
	
	app.MapControllerRoute(
		name: "post-by-tag",
		pattern: "blog/tag/{slug}",
		defaults: new { controller = "Blog", action = "Tag"});

	app.MapControllerRoute(
		name: "single",
		pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
		defaults:new { controller = "Blog", action = "Post"});

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Blog}/{action=Index}/{id?}");
	//app.UseEndpoints(endpoints =>
	//{
	//	endpoints.MapControllerRoute(
	//		name: "default",
	//		pattern: "{controller=Blog}/{action=Index}/{id?}");
	//});
}

using (var scope = app.Services.CreateScope())
{
	var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
	seeder.Initialize();
}

app.Run();
