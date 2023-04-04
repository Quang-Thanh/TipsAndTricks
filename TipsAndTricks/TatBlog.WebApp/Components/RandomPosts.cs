using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class RandomPosts : ViewComponent
	{
		private readonly IBlogRepository _blogRepositry;

		public RandomPosts(IBlogRepository blogRepositry)
		{
			_blogRepositry = blogRepositry;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			//Random 5 bài viết
			var randomPost = await _blogRepositry.GetRandomArticlesAsync(5);

			return View(randomPost);
		}
	}
}
