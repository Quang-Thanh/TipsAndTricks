using TatBlog.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
	//add services to the container.
	builder
		.ConfigureCors()
		.ConfigureNLog()
		.ConfigureServices()
		.ConfigureSwaggerOpenApi();
}

var app = builder.Build();
{
	//Configure the HTTP request pipeline
	app.SetupRequestPipeline();

	app.Run();
}

app.Run();