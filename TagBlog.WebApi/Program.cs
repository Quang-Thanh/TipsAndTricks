using TagBlog.WebApi.Extensions;
using TagBlog.WebApi.Mapsters;
using TagBlog.WebApi.Extensions;
using TagBlog.WebApi.Validations;
using TagBlog.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
{
	//add services to the container.
	builder
		.ConfigureCors()
		.ConfigureNLog()
		.ConfigureServices()
		.ConfigureSwaggerOpenApi()
		.ConfigureMapster()
		.ConfigureFluentValidation();
}

var app = builder.Build();
{
	//Configure the HTTP request pipeline
	app.SetupRequestPipeline();

	//Configure API endpoints
	app.MapAuthorEndpoints();

	//CategoryEndponit
	app.MapCategoryEndPoints();

	//PostEndpoint
	app.MapPostEndpoints();

	//TagEndpoint
	app.MapTagEndpoint();

	app.Run();
}

app.Run();