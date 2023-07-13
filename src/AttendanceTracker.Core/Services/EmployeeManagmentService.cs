using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services
{
	public class EmployeeManagmentService : IEmployeeManagmentService
	{
        private readonly IAsyncRepository<EmployeeManagment> _employeeManagmentRepository;

        public EmployeeManagmentService(IAsyncRepository<EmployeeManagment> employeeManagmentRepository)
        {
            _employeeManagmentRepository = employeeManagmentRepository;
        }
        public async Task<IReadOnlyList<EmployeeManagment>> GetManagerByRoleIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var managersByRoleId = new ReadOnlyTheDataInTheFrameworkOfTheRoleSpecification(employeeId);
            var employees = await _employeeManagmentRepository.ListAsync(managersByRoleId, cancellationToken);
            return employees;
        }
        public async Task<IReadOnlyList<EmployeeManagment>> GetEmployeeManagmentAsync(CancellationToken cancellationToken = default)
        {
            var employeeManagment = new ReadOnlyEmployee_ManagersToEmployeeManagmentSpecifications();
            return await _employeeManagmentRepository.ListAsync(employeeManagment);
        }

        public async Task<bool> AddEmployeeManagmentAsync(int managerId,int employeeId, CancellationToken cancellationToken = default)
        {
            var employeeManagment = await _employeeManagmentRepository.AddAsync(new EmployeeManagment
            {
                ManagerId = managerId,
                EmployeeId = employeeId
                
            }, cancellationToken);
            return employeeManagment;
        }

        public async Task<bool> DeleteEmployeeManagmentAsync(int id, CancellationToken cancellationToken = default)
        {
            var employeeManagmentDelete = await _employeeManagmentRepository.GetByIdAsync(id);
            await _employeeManagmentRepository.DeleteAsync(employeeManagmentDelete);

            return true;

        }

        public async Task<bool> UpdateEmployeeManagmentAsync(int id,int managerId, int employeeId , CancellationToken cancellationToken = default)
        {
            var employeeManagmentToUpdate = await _employeeManagmentRepository.GetByIdAsync(id);

            employeeManagmentToUpdate.ManagerId = managerId;
            employeeManagmentToUpdate.EmployeeId = employeeId;
            

            await _employeeManagmentRepository.UpdateAsync(employeeManagmentToUpdate);

            return true;
        }
    }
}

