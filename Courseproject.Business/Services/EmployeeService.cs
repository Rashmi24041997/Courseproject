using AutoMapper;
using Courseproject.Business.Exceptions;
using Courseproject.Business.Validation;
using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Courseproject.Business.Services;

public class EmployeeService : IEmployeeService
{
    private IGenericRepository<Employee> EmployeeRepository { get; }
    public IGenericRepository<Job> JobRepository { get; }
    public IGenericRepository<Address> AddressRepository { get; }
    private IMapper Mapper { get; }
    private EmployeeCreateValidator EmployeeCreateValidator { get; }
    private EmployeeUpdateValidator EmployeeUpdateValidator { get; }
    private IUploadService UploadService { get; }

    //private IFileService FileService { get; }
    private ImageFileValidator ImageFileValidator { get; }
    private ILogger<EmployeeService> Logger { get; }

    public EmployeeService(IGenericRepository<Employee> employeeRepository, IGenericRepository<Job> jobRepository,
        IGenericRepository<Address> addressRepository, IMapper mapper,
        EmployeeCreateValidator employeeCreateValidator, EmployeeUpdateValidator employeeUpdateValidator,
        /*IFileService fileService,*/
        IUploadService uploadService,
        ImageFileValidator imageFileValidator,
        ILogger<EmployeeService> logger)
    {
        EmployeeRepository = employeeRepository;
        JobRepository = jobRepository;
        AddressRepository = addressRepository;
        Mapper = mapper;
        EmployeeCreateValidator = employeeCreateValidator;
        EmployeeUpdateValidator = employeeUpdateValidator;
        UploadService = uploadService;
        //FileService = fileService;
        ImageFileValidator = imageFileValidator;
        Logger = logger;
    }

    public async Task<int> CreateEmployeeAsync(EmployeeCreate employeeCreate)
    {
        await EmployeeCreateValidator.ValidateAndThrowAsync(employeeCreate);

        var address = await AddressRepository.GetByIdAsync(employeeCreate.AddressId);

        if (address == null)
            throw new AddressNotFoudException(employeeCreate.AddressId);

        var job = await JobRepository.GetByIdAsync(employeeCreate.JobId);

        if (job == null)
            throw new JobNotFoundException(employeeCreate.JobId);

        var entity = Mapper.Map<Employee>(employeeCreate);
        entity.Address = address;
        entity.Job = job;
        await EmployeeRepository.InsertAsync(entity);
        await EmployeeRepository.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteEmployeeAsync(EmployeeDelete employeeDelete)
    {
        var entity = await EmployeeRepository.GetByIdAsync(employeeDelete.Id);

        if (entity == null)
            throw new EmployeeNotFoundException(employeeDelete.Id);

        EmployeeRepository.Delete(entity);
        await EmployeeRepository.SaveChangesAsync();

    }

    public async Task<EmployeeDetails> GetEmployeeAsync(int id)
    {
        Logger.LogInformation("GetEmployeeAsync called.");
        var entity = await EmployeeRepository.GetByIdAsync(id, (employee) => employee.Address, (employee) => employee.Job, (employee) => employee.Teams);

        if (entity == null)
            throw new EmployeeNotFoundException(id);

        return Mapper.Map<EmployeeDetails>(entity);
    }

    public async Task<List<EmployeeList>> GetEmployeesAsnyc(EmployeeFilter employeeFilter)
    {
        Expression<Func<Employee, bool>> firstNameFilter = (employee) => employeeFilter.FirstName == null ? true :
        employee.FirstName.StartsWith(employeeFilter.FirstName);
        Expression<Func<Employee, bool>> lastNameFilter = (employee) => employeeFilter.LastName == null ? true :
        employee.LastName.StartsWith(employeeFilter.LastName);
        Expression<Func<Employee, bool>> jobfilter = (employee) => employeeFilter.Job == null ? true :
        employee.Job.Name.StartsWith(employeeFilter.Job);
        Expression<Func<Employee, bool>> teamFilter = (employee) => employeeFilter.Team == null ? true :
        employee.Teams.Any(team => team.Name.StartsWith(employeeFilter.Team));

        var entities = await EmployeeRepository.GetFilteredAsync(new Expression<Func<Employee, bool>>[]
        {
            firstNameFilter, lastNameFilter, jobfilter, teamFilter
        }, employeeFilter.Skip, employeeFilter.Take,
        (employee) => employee.Address, (employee) => employee.Job, (employee) => employee.Teams);

        return Mapper.Map<List<EmployeeList>>(entities);
    }

    public async Task UpdateEmployeeAsync(EmployeeUpdate employeeUpdate)
    {
        await EmployeeUpdateValidator.ValidateAsync(employeeUpdate);

        var address = await AddressRepository.GetByIdAsync(employeeUpdate.AddressId);

        if(address == null)
            throw new AddressNotFoudException(employeeUpdate.AddressId);

        var job = await JobRepository.GetByIdAsync(employeeUpdate.JobId);

        if(job == null)
            throw new JobNotFoundException(employeeUpdate.JobId);

        var existingEmployee = await EmployeeRepository.GetByIdAsync(employeeUpdate.Id);

        if(existingEmployee == null)
            throw new EmployeeNotFoundException(employeeUpdate.Id);

        var entity = Mapper.Map<Employee>(employeeUpdate);
        entity.Address = address;
        entity.Job = job;
        EmployeeRepository.Update(entity);
        await EmployeeRepository.SaveChangesAsync();
    }

    public async Task UpdateProfilePhotoAsync(ProfilePhotoUpdate profilePhotoUpdate)
    {
        await ImageFileValidator.ValidateAndThrowAsync(profilePhotoUpdate.Photo);

        var employee = await EmployeeRepository.GetByIdAsync(profilePhotoUpdate.EmployeeId);

        if (employee == null)
            throw new EmployeeNotFoundException(profilePhotoUpdate.EmployeeId);

        if (employee.ProfilePhotoPath != null)
            await UploadService.DeleteFileAsync(employee.ProfilePhotoPath);
        //FileService.DeleteFile(employee.ProfilePhotoPath);

        //var filename = await FileService.SaveFileAsync(profilePhotoUpdate.Photo);
        var filename = await UploadService.UploadFileAsync(profilePhotoUpdate.Photo);
        employee.ProfilePhotoPath = filename;

        EmployeeRepository.Update(employee);
        await EmployeeRepository.SaveChangesAsync();
    }
}
