using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Courseproject.Business.Validation;

public class ImageFileValidator : AbstractValidator<IFormFile>
{
	public ImageFileValidator()
	{
		RuleFor(file => file.ContentType).NotNull().Must(ct => ct.Equals("image/jpeg") ||
		ct.Equals("image/jpg") || ct.Equals("image/png"));
	}
}
