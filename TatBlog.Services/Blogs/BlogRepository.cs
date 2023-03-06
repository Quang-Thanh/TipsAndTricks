using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
	public class BlogRepository : IBlogRepository
	{
		private readonly BlogDbContext _context;

		public BlogRepository(BlogDbContext context)
		{
			_context = context;
		}

		public async Task<IList<CategoryItem>> GetCategoriesAsync(
		bool showOnMenu = false,
		CancellationToken cancellationToken = default)
		{
			IQueryable<Category> categories = _context.Set<Category>();

			if (showOnMenu)
			{
				categories = categories.Where(x => x.ShowOnMenu);
			}

			return await categories
				.OrderBy(x => x.Name)
				.Select(x => new CategoryItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					ShowOnMenu = x.ShowOnMenu,
					PostCount = x.Posts.Count(p => p.Published)
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
		{
			var tagQuery = _context.Set<Tag>()
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			});

			return await tagQuery
				.ToPagedListAsync(pagingParams, cancellationToken);
		}

		public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.Include(x => x.Author)
				.Include(x => x.Category)
				.OrderByDescending(p => p.ViewCout)
				.Take(numPosts).ToListAsync();
		}

		public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
		{
			IQueryable<Post> postsQuery = _context.Set<Post>()
				.Include(x => x.Category)
				.Include(x => x.Author);

			if (year > 0)
			{
				postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
			}

			if (month > 0)
			{
				postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
			}

			if (!string.IsNullOrEmpty(slug))
			{
				postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
			}

			return await postsQuery.FirstOrDefaultAsync(cancellationToken);
		}


		public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
		{
			await _context.Set<Post>()
				.Where(x => x.Id == postId)
				.ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCout, x => x.ViewCout + 1), cancellationToken);
		}

		public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
		}

		//public async Task<Tag> GetTagFromSlugAsync(string slug, CancellationToken cancellationToken = default)
		//{
		//    //phần C lab 01
		//    return await _context.Set<Tag>()
		//        .Where(t => t.UrlSlug == slug)
		//        .FirstOrDefaultAsync(cancellationToken);
		//}

		//Phần C
		//a
		//public Task<Tag> FindTagBySlugAsync(
		//    string slug, CancellationToken cancellationToken = default
		//    )
		//{
		//    return _context.Set<Tag>()
		//        .Where(x => x.UrlSlug == slug)
		//        .FirstOrDefaultAsync(cancellationToken);
		//}

		//b tạo lớp DTO
		//public async Task<IList<TagItem>> GetTagAsync(CancellationToken cancellationToken = default)
		//{
		//    var tagQuery = _context.Set<Tag>()
		//        .Select(x => new TagItem()
		//        {
		//            Id = x.Id,
		//            Name = x.Name,
		//            UrlSlug = x.UrlSlug,
		//            Description = x.Description,
		//            PostCount = x.Posts.Count(p => p.Published),
		//        });

		//    return await tagQuery.ToListAsync(cancellationToken);
		//}

		//c 
		public async Task<IList<TagItem>> FindTagItemSlugAsync(CancellationToken cancellationToken = default)
		{
			var query = _context.Set<Tag>()
					.Select(x => new TagItem()
					{
						Id = x.Id,
						Name = x.Name,
						UrlSlug = x.UrlSlug,
						Description = x.Description,
						PostCount = x.Posts.Count(p => p.Published)
					});
			return await query.ToListAsync(cancellationToken);
		}

		public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.Where(x => x.Id == id)
				.ExecuteDeleteAsync(cancellationToken) > 0;
		}

		public async Task<bool> AddOrUpdateCategoryAsync(Category newCategory, CancellationToken cancellationToken = default)
		{
			_context.Set<Category>()
				.Entry(newCategory).State = newCategory.Id == 0 
				? EntityState.Added 
				: EntityState.Modified;
			_context.SaveChanges();
			return true;
		}
	}
}