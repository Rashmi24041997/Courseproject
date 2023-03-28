using Courseproject.Common.Dtos.Teams;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class TeamUpdateValidator : AbstractValidator<TeamUpdate>
{
	public TeamUpdateValidator()
	{
		RuleFor(teamCreate => teamCreate.Name).NotEmpty().MaximumLength(50);
	}
}
