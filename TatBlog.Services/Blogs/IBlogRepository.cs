﻿using System;
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
        string name,
        CancellationToken cancellationToken = default);

        Task<Tag> GetTagFromSlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<Category> GetCategoryFromSlugAsync(string slug, CancellationToken cancellationToken = default);
        //Task<Author> GetAuthorFromSlugAsync(string slug, CancellationToken cancellationToken = default);

        //Phần9 --> Lab01S
        Task<IPagedList> GetPagedPostsAsync(
            PostQuery condition, int pageNumber, int pageSize,
            CancellationToken cancellationToken = default);
        //Task<IList<AuthorItem>> GetAuthorsAsync(

        //CancellationToken cancellationToken = default);

        Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default);

        Task<Post> CreateOrUpdatePostAsync(
        Post post, IEnumerable<string> tags,
        CancellationToken cancellationToken = default);

        Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default);

        Task ChangeStatusPublishedOfPostAsyn(int id,
            CancellationToken cancellationToken = default);

        Task<bool> DeletePostAsync(int postId, CancellationToken cancellationToken = default);

        Task<IList<Post>> GetFeaturePostAysnc(
          int numberPost,
          CancellationToken cancellationToken = default);
        Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default);

        Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedPostsAsync<T>(
            PostQuery query, IPagingParams pagingParams, 
            Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

        //category API
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);

        Task<Category> GetCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default);

        Task<Category> GetCachedCategoryBySlugAsync(
            string slug, CancellationToken cancellationToken = default);

        Task<Category> GetcategoryByIdAsync(int categoryId);

        Task<Category> GetCachedCategoryByIdAsync(int categoryId);

        Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
            Func<IQueryable<Category>, IQueryable<T>> mapper,
                IPagingParams pagingParams,
                string name = null,
                CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(
            Category category, CancellationToken cancellationToken = default);

        Task<bool> DeleteCategoryAsync(
            int categoryId, CancellationToken cancellation = default);

        Task<bool> IsCategorySlugExistedAsync(
            int categoryId,
            string slug,
            CancellationToken cancellation = default);

        //post API
        Task<Post> GetPostBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

        Task<Post> GetCachedPostBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

        Task<Post> GetPostByIdAsync(int postId);

        Task<Post> GetCachedPostByIdAsync(int postId);

        Task<IList<Post>> GetPostsAsync(
        PostQuery condition,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedPostsAsync<T>(
        Func<IQueryable<Post>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(
        Post post, CancellationToken cancellationToken = default);

        Task<bool> SetImageUrlAsync(
        int postId, string imageUrl,
        CancellationToken cancellationToken = default);

        //tag
        Task<Tag> GetTagBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

        Task<Tag> GetCachedTagBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

        Task<Tag> GetTagByIdAsync(int tagId);

        Task<Tag> GetCachedTagByIdAsync(int tagId);

        Task<IPagedList<T>> GetPagedTagsAsync<T>(
        Func<IQueryable<Tag>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);

        Task<bool> AddOrUpdateAsync(
        Tag tag, CancellationToken cancellationToken = default);

        Task<bool> DeleteTagAsync(
        int tagId, CancellationToken cancellationToken = default);

        Task<bool> IsTagSlugExistedAsync(
        int tagId,
        string slug,
        CancellationToken cancellationToken = default);

	}
}
