using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class FeaturedPosts : ViewComponent
	{
		private readonly IBlogRepository _blogRepositry;

		public FeaturedPosts(IBlogRepository blogRepositry)
		{
			_blogRepositry = blogRepositry;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			//Lấy 3 bài viết
			var post = await _blogRepositry.GetFeaturePostAysnc(3);

			return View(post);
		}
	}
}
