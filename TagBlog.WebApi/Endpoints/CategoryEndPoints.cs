﻿using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Net;
using TagBlog.WebApi.Filters;
using TagBlog.WebApi.Models;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;

namespace TagBlog.WebApi.Endpoints
{
	public static class CategoryEndPoints
	{
		public static WebApplication MapCategoryEndPoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/categories");

			routeGroupBuilder.MapGet("/", GetCategory)
				.WithName("GetCategory")
				.Produces<ApiResponse<PaginationResult<CategoryItem>>>();

			routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
				.WithName("GetCategoryById")
				.Produces<ApiResponse<CategoryItem>>();

			routeGroupBuilder.MapGet(
				"/{slug:regex(^[a-z0-9 -]+$)}/posts",
				GetPostsByCategorySlug)
				.WithName("GetPostsByCategorySlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			routeGroupBuilder.MapPost("/", AddCategory)
				.WithName("AddNewAuthor")
				.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
				.Produces(401)
				.Produces<ApiResponse<CategoryItem>>();

			routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
				.WithName("UpdateCategory")
				.Produces(401)
				.Produces<ApiResponse<string>>();

			routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
				.WithName("DeleteCategory")
				.Produces(401)
				.Produces<ApiResponse<string>>();

			return app;
		}

		private static async Task<IResult> GetCategory(
		[AsParameters] CategoryFilterModel model,
		IBlogRepository blogRepository)
		{
			var categoriesList = await blogRepository
				.GetPagedCategoriesAsync(model, model.Name);

			var paginationResult = new PaginationResult<CategoryItem>(categoriesList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetCategoryDetails(
			int id,
			IBlogRepository blogRepository,
			IMapper mapper)
		{
			var category = await blogRepository.GetCachedCategoryByIdAsync(id);

			return category == null
				? Results.Ok(ApiResponse.Fail(System.Net.HttpStatusCode.NotFound,
				$"Không tìm thấy chủ đề có mã số {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(category)));
		}

		private static async Task<IResult> GetPostsByCategoryId(
			int id,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				categoryId = id,
				PublishedOnly = true
			};

			var postsList = await blogRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		public static async Task<IResult> GetPostsByCategorySlug(
			[FromRoute] string slug,
			[AsParameters] PagingModel pagingModel,
			IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				CategorySlug = slug,
				PublishedOnly = true
			};

			var postsList = await blogRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());
			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> AddCategory(
			CategoryEditModel model,
			IBlogRepository blogRepository,
			IMapper mapper)
		{
			if (await blogRepository
				.IsCategorySlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			var category = mapper.Map<Category>(model);
			await blogRepository.AddOrUpdateAsync(category);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
		}

		private static async Task<IResult> UpdateCategory(
			int id, CategoryEditModel model,
			IBlogRepository blogRepository,
			IValidator<CategoryEditModel> validator,
			IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest,
					validationResult));
			}

			if (await blogRepository
				.IsCategorySlugExistedAsync(id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' đã được sử dụng"));
			}

			var category = mapper.Map<Category>(model);
			category.Id = id;

			return await blogRepository.AddOrUpdateAsync(category)
				? Results.Ok(ApiResponse.Success("Category is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(
					HttpStatusCode.NotFound, "Could not find category"));
		}

		private static async Task<IResult> DeleteCategory(
			int id, IBlogRepository blogRepository)
		{
			return await blogRepository.DeleteCategoryAsync(id)
				? Results.Ok(ApiResponse.Success("Category is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Could not find category"));
		}

	}
}
