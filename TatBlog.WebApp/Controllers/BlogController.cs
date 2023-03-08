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
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            //tạo đối tượng chưa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                //chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true,

                //Tìm bài viết theo từ khóa
                keyWord = keyWord
            };

            //truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            //Lưu lại điều kiện truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;

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
