using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
	public class BlogController : Controller
    {

        //Phần 9
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name ="k")] string keyWord = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            //tạo đối tượng chưa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                //chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true,

                //Tìm bài viết theo từ khóa
                keyword = keyWord
            };

            //truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            //Lưu lại điều kiện truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;

            //Truyền danh sách bài viết vào View để rander ra HTMl
            return View(postsList);
        }

		public async Task<IActionResult> Category(
            string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{
			//tạo đối tượng chưa các điều kiện truy vấn
			var postQuery = new PostQuery()
			{
				//chỉ lấy những bài viết có trạng thái Published
				PublishedOnly = true,

				//Tìm bài viết theo từ khóa
				CategorySlug = slug,
			};

			//truy vấn các bài viết theo điều kiện đã tạo
			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            var category = await _blogRepository
                .GetCategoryFromSlugAsync(slug);

			//Lưu lại điều kiện truy vấn để hiển thị trong View
			ViewBag.NameCategory = category.Name;

			//Truyền danh sách bài viết vào View để rander ra HTMl
			return View(postsList);
		}

		public async Task<IActionResult> Author(
			string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{
			//tạo đối tượng chưa các điều kiện truy vấn
			var postQuery = new PostQuery()
			{
				//chỉ lấy những bài viết có trạng thái Published
				PublishedOnly = true,

				//Tìm bài viết theo từ khóa
				AuthorSlug = slug,
			};

			//truy vấn các bài viết theo điều kiện đã tạo
			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

			var author = await _blogRepository
				.GetAuthorFromSlugAsync(slug);

			//Lưu lại điều kiện truy vấn để hiển thị trong View
			ViewBag.NameAuthor = author.FullName;

			//Truyền danh sách bài viết vào View để rander ra HTMl
			return View(postsList);
		}

		public async Task<IActionResult> Post(
			string slug, int month, int year)
		{
			var post = await _blogRepository.GetPostAsync(year, month, slug);

			return View(post);
		}

		public async Task<IActionResult> Tag(
			string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{
			//tạo đối tượng chưa các điều kiện truy vấn
			var postQuery = new PostQuery()
			{
				//chỉ lấy những bài viết có trạng thái Published
				PublishedOnly = true,

				//Tìm bài viết theo từ khóa
				TagSlug = slug,
			};

			//truy vấn các bài viết theo điều kiện đã tạo
			var postsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

			var Tag = await _blogRepository
				.GetTagFromSlugAsync(slug);

			//Lưu lại điều kiện truy vấn để hiển thị trong View
			ViewBag.NameTag = Tag.Name;

			//Truyền danh sách bài viết vào View để rander ra HTMl
			return View(postsList);
		}
		//public IActionResult Index()
		//{
		//    ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");

		//    return View();
		//}

		public IActionResult About()
            => View();

        public IActionResult Contact() 
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");
    }
}
