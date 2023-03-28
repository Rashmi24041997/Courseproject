using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Dtos.Employee;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class EmployeeUpdateValidator : AbstractValidator<EmployeeUpdate>
{
	public EmployeeUpdateValidator()
	{
		RuleFor(employeeUpdate => employeeUpdate.FirstName).NotEmpty().MaximumLength(50);
		RuleFor(employeeUpdate => employeeUpdate.LastName).NotEmpty().MaximumLength(50);
	}
}
