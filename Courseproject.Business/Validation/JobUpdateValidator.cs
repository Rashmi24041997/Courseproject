using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Dtos.Job;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class JobUpdateValidator : AbstractValidator<JobUpdate>
{
	public JobUpdateValidator()
	{
		RuleFor(jobUpdate => jobUpdate.Name).NotEmpty().MaximumLength(50);
		RuleFor(JobUpdate => JobUpdate.Description).NotEmpty().MaximumLength(250);
	}
}
