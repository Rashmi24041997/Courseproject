using Courseproject.Common.Dtos.Employee;

namespace Courseproject.Common.Dtos.Teams;

public record TeamGet(int Id, string Name, List<EmployeeList> Employees);