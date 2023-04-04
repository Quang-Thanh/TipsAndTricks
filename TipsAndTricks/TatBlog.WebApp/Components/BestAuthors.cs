using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class BestAuthors : ViewComponent
	{
		private readonly IBlogRepository _blogRepositry;

		public BestAuthors(IBlogRepository blogRepositry)
		{
			_blogRepositry = blogRepositry;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			//Hiện thị top 4 tác giả
			var authors = await _blogRepositry.GetAuthorsAsync();

			return View(authors);
		}
	}
}
