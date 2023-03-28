using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Dtos.Employee;
using FluentValidation;

namespace Courseproject.Business.Validation;

public class EmployeeCreateValidator : AbstractValidator<EmployeeCreate>
{
	public EmployeeCreateValidator()
	{
		RuleFor(employeeCreate => employeeCreate.FirstName).NotEmpty().MaximumLength(50);
		RuleFor(employeeCreate => employeeCreate.LastName).NotEmpty().MaximumLength(50);
	}
}
