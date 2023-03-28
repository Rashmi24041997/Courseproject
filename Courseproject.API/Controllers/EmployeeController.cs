using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Courseproject.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private IEmployeeService EmployeeService { get; }
    private ILogger<EmployeeController> Logger { get; }

    public EmployeeController(IEmployeeService employeeService,
        ILogger<EmployeeController> logger)
	{
        EmployeeService = employeeService;
        Logger = logger;
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateEmployee(EmployeeCreate employeeCreate)
    {
            var id = await EmployeeService.CreateEmployeeAsync(employeeCreate);
            return Ok(id);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateEmployee(EmployeeUpdate employeeUpdate)
    {
        await EmployeeService.UpdateEmployeeAsync(employeeUpdate);
        return Ok();
    }

    [HttpPut]
    [Route("Update/ProfilePhoto")]
    public async Task<IActionResult> UpdateProfilePhoto([FromForm]ProfilePhotoUpdate profilePhotoUpdate)
    {
        await EmployeeService.UpdateProfilePhotoAsync(profilePhotoUpdate);
        return Ok();
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<IActionResult> DeleteEmployee(EmployeeDelete employeeDelete)
    {
        await EmployeeService.DeleteEmployeeAsync(employeeDelete);
        return Ok();
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        using (LogContext.PushProperty("Employee Id", id))
        {
            var employee = await EmployeeService.GetEmployeeAsync(id);
            return Ok(employee);
        }
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> GetEmployees([FromQuery]EmployeeFilter employeeFilter)
    {
        var employees = await EmployeeService.GetEmployeesAsnyc(employeeFilter);
        return Ok(employees);
    }

}
