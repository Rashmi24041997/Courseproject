using Microsoft.AspNetCore.Http;

namespace Courseproject.Common.Dtos.Employee;

public record ProfilePhotoUpdate(int EmployeeId, IFormFile Photo);
