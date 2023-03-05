var builder = WebApplication.CreateBuilder(args);
{
	//Thêm các dịch vụ đucợ yên cầu bới MVC Framework
	builder.Services.AddControllersWithViews();
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
		name:"default", 
		pattern:"{controller=Blog}/{acion=Index}/{id?}");
}

//app.MapGet("/", () => "Hello World!");

app.Run();
