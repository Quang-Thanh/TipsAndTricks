using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class CategoriesController : Controller
	{
		public IActionResult Index() 
		{
			return View();
		}
	}
}
