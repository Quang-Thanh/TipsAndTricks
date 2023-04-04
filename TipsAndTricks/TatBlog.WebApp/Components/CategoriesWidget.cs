using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class CategoriesWidget : ViewComponent
	{
		private readonly IBlogRepository _blogRepositry;

		public CategoriesWidget(IBlogRepository blogRepositry)
		{
			_blogRepositry = blogRepositry;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			//Lấy danh sách chủ đề
			var categories = await _blogRepositry.GetCategoriesAsync();

			return View(categories);
		}
	}
}
