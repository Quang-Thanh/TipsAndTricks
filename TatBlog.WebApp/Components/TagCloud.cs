using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class TagCloud : ViewComponent
	{
		private readonly IBlogRepository _blogRepositry;

		public TagCloud(IBlogRepository blogRepositry)
		{
			_blogRepositry = blogRepositry;
		}

		//public async Task<IViewComponentResult> InvokeAsync()
		//{
		//	//Hiển thị danh sách thẻ tag (còn làm trong blogreponsi với iblogrepon)
		//	var tagCloud = await _blogRepositry.GetTagCloudAysnc();

		//	return View(tagCloud);
		//}
	}
}
