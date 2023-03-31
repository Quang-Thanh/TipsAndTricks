using FluentValidation;
using TagBlog.WebApi.Models;

namespace TagBlog.WebApi.Validations
{
	public class TagValidators : AbstractValidator<TagEditModel>
	{
		public TagValidators() 
		{
			RuleFor(a => a.Name)
				.NotEmpty()
				.WithMessage("Không được để trống")
				.MaximumLength(100)
				.WithMessage("Tối đa 100 ký tự");

			RuleFor(a => a.Description)
				.NotEmpty()
				.WithMessage("Giới thiệu không được để trống")
				.MaximumLength(200)
				.WithMessage("Giới thiệu không quá 200 ký tự");

			RuleFor(a => a.UrlSlug)
				.NotEmpty()
				.WithMessage("UrlSlug không được để trống")
				.MaximumLength(100)
				.WithMessage("UrlSlug không quá 200 ký tự");


		}
	}
}
