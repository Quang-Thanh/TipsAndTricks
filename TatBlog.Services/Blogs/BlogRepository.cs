using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
//using SlugGenerator;
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
		private readonly IMemoryCache _memoryCache;

		public BlogRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
			_memoryCache = memoryCache;
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
        //public async Task<IList<AuthorItem>> GetAuthorsAsync(
        
        //CancellationToken cancellationToken = default)
        //{
        //    IQueryable<Author> author = _context.Set<Author>();

            

        //    return await author
        //        .OrderBy(x => x.FullName)
        //        .Select(x => new AuthorItem()
        //        {
        //            Id = x.Id,
        //            FullName = x.FullName,
        //            UrlSlug = x.UrlSlug,
        //            Email = x.Email,
        //            JoineDate = x.JoineDate,
        //            ImageUrl = x.ImageUrl,
        //            Notes = x.Notes,
        //            PostCount = x.Posts.Count(p => p.Published)
        //        })
        //        .ToListAsync(cancellationToken);
        //}

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
                .Include(x => x.Author)
                .Include(x => x.Tags);

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

        public async Task<Tag> GetTagFromSlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(t => t.UrlSlug == slug)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IPagedList> GetPagedPostsAsync(PostQuery condition, int pageNumber = 1, int pageSize = 5, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }
		private IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.categoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.categoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.authorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.authorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.keyWord))
            {
                posts = posts.Where(x => x.Title.Contains(condition.keyWord) ||
                                         x.ShortDescription.Contains(condition.keyWord) ||
                                         x.Description.Contains(condition.keyWord) ||
                                         x.Category.Name.Contains(condition.keyWord) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.keyWord)));
            }

            if (condition.postYear > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.postYear);
            }

            if (condition.postMonth > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.postMonth);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;
        }

		public async Task<Category> GetCategoryFromSlugAsync(string slug, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Category>()
				.Where(t => t.UrlSlug == slug)
				.FirstOrDefaultAsync(cancellationToken);
		}
  //      public async Task<Author> GetAuthorFromSlugAsync(string slug, CancellationToken cancellationToken = default)
		//{
		//	return await _context.Set<Author>()
		//		.Where(t => t.UrlSlug == slug)
		//		.FirstOrDefaultAsync(cancellationToken);
		//}

        public async Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }

		public async Task<Tag> GetTagAsync(
		string slug, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
		}

		public async Task<IList<TagItem>> GetTagsAsync(
			CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.OrderBy(x => x.Name)
				.Select(x => new TagItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					PostCount = x.Posts.Count(p => p.Published)
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default)
		{
			if (post.Id > 0)
			{
				await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
			}
			else
			{
				post.Tags = new List<Tag>();
			}

			var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => new
				{
					Name = x,
					Slug = x.GenerateSlug()
				})
				.GroupBy(x => x.Slug)
				.ToDictionary(g => g.Key, g => g.First().Name);


			foreach (var kv in validTags)
			{
				if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

				var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
				{
					Name = kv.Value,
					Description = kv.Value,
					UrlSlug = kv.Key
				};

				post.Tags.Add(tag);
			}

			post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

			if (post.Id > 0)
				_context.Update(post);
			else
				_context.Add(post);

			await _context.SaveChangesAsync(cancellationToken);

			return post;
		}

        public async Task ChangeStatusPublishedOfPostAsyn(int id, 
            CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(id);

            post.Published = !post.Published;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeletePostAsync(int postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(postId);

            if(!post.Published) return false;

            _context.Set<Post>().Remove(post);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }

		public async Task<IList<Post>> GetFeaturePostAysnc(
		  int numberPost,
		  CancellationToken cancellationToken = default)
		{

			return await _context.Set<Post>()
				.Include(x => x.Category)
				.Include(x => x.Author)
				.Include(x => x.Tags)
				.OrderByDescending(x => x.ViewCout)
				.Take(numberPost)
				.ToListAsync(cancellationToken);
		}

		public async Task<IList<Post>> GetRandomArticlesAsync(
		int numPosts, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.OrderBy(x => Guid.NewGuid())
				.Take(numPosts)
				.ToListAsync(cancellationToken);
		}

		public async Task<IPagedList<T>> GetPagedPostsAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
		{
			IQueryable<T> result = mapper(FilterPosts(query));

			return await result.ToPagedListAsync(pagingParams, cancellationToken);
		}


        //category API
		public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
		IPagingParams pagingParams,
		string name = null,
	    CancellationToken cancellationToken = default)
		{

			return await _context.Set<Category>()
				.AsNoTracking()
				.Where(x => x.Name.Contains(name))
				.Select(x => new CategoryItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					ShowOnMenu = x.ShowOnMenu,
					PostCount = x.Posts.Count(p => p.Published)
				})
				.ToPagedListAsync(pagingParams, cancellationToken);
		}


        public async Task<Category> GetCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }

        public async Task<Category> GetCachedCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-slug.{slug}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryBySlugAsync(slug, cancellationToken);
                });
        }

        public async Task<Category> GetcategoryByIdAsync(int categoryId)
        {
            return await _context.Set<Category>().FindAsync(categoryId);
        }

        public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-id.{categoryId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetcategoryByIdAsync(categoryId);
                });
        }

		public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
            Func<IQueryable<Category>, IQueryable<T>> mapper,
                IPagingParams pagingParams,
                string name = null,
                CancellationToken cancellationToken = default)
        {
            var categoryQuery = _context.Set<Category>().AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                categoryQuery = categoryQuery.Where(x => x.Name.Contains(name));
            }

            return await mapper(categoryQuery)
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<bool> AddOrUpdateAsync(
            Category category, CancellationToken cancellationToken = default)
        {
            if (category.Id > 0)
            {
                _context.Categories.Update(category);
                _memoryCache.Remove($"category.by-id.{category.Id}");
            }
            else
            {
                _context.Categories.Add(category);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteCategoryAsync(
            int categoryId, CancellationToken cancellation = default)
        {
            return await _context.Categories
                .Where(x => x.Id == categoryId)
                .ExecuteDeleteAsync(cancellation) > 0;
        }

        public async Task<bool> IsCategorySlugExistedAsync(
            int categoryId,
            string slug,
            CancellationToken cancellation = default)
        {
            return await _context.Categories
                .AnyAsync(x => x.Id != categoryId && x.UrlSlug == slug, cancellation);
        }

        
        //posts API


	}
}
