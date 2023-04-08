using TatBlog.Core.Entities;

namespace TagBlog.WebApi.Models
{
	public class PostDto
	{
		//Mã bài viết
		public int Id { get; set; }

		//Tiêu đề bài viết
		public string Title { get; set; }

		//Mô tả hay giới thiệu ngắn về nội dung
		public string ShortDescription { get; set; }

		//Tên định dạng để tạo URL
		public string UrlSlug { get; set; }

		//Đường dẫn đến tập tin hình ảnh
		public string ImageUrl { get; set; }

		//Số lượng xem, đọc bài viết
		public int ViewCount { get; set; }

		//Ngày giờ đăng bài
		public DateTime PostedDate { get; set; }

		//Ngày giờ cập nhật lần cuối
		public DateTime? ModifiedDate { get; set; }

		public bool Published { get; set; }
		
		//Chuyên mục bài viết
		public	CategoryDto Category { get; set; }

		//Tác giả của bài viết
		public AuthorDto Author { get; set; }

		//Danh sách các từ khóa của bài viết
		public IList<TagDto> Tags { get; set; }
	}
}
