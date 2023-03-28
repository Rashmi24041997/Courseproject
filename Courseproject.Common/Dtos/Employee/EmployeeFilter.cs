namespace Courseproject.Common.Dtos.Employee;

public record EmployeeFilter(string? FirstName, string? LastName, string? Job, string? Team, int? Skip, int? Take);