namespace TagBlog.WebApi.Models
{
	public class CategoryEditModel
	{
		//tên chuyên mục, chủ đề
		public string Name { get; set; }

		//Tên định dạng để tạo URL
		public string UrlSlug { get; set; }

		//mô tả chuyên mục
		public string Description { get; set; }

		//Đánh giá chuyên mục được hiện thị trên menu
		public bool ShowOnMenu { get; set; }
	}
}
