using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);

        Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default);

        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);

        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);

        Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default);

        //Phần C lab01
        //Task<Tag> GetTagFromSlugAsync(string slug, CancellationToken cancellationToken = default);
        //a
        //Task<Tag> FindTagBySlugAsync(
        //    string slug, CancellationToken cancellationToken = default);

		//b
		//Task<IList<TagItem>> GetTagAsync(CancellationToken cancellationToken = default);

        //c danh sách tất cả thẻ tag
        Task<IList<TagItem>> FindTagItemSlugAsync(CancellationToken cancellationToken= default);

        // Câu d:
        Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default);

        // Câu g:
        Task<bool> AddOrUpdateCategoryAsync(Category newCategory, CancellationToken cancellationToken= default);
	}
}
