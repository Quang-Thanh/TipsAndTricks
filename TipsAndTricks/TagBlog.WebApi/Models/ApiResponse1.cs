namespace TagBlog.WebApi.Models
{
	public class ApiResponse<T> : ApiResponse
	{
		public T Result { get; set; }
	}
}