using System;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Interfaces
{
	public interface IEmployeeManagmentService
	{

        Task<IReadOnlyList<EmployeeManagment>> GetEmployeeManagmentAsync(CancellationToken cancellationToken = default);
        Task<bool> AddEmployeeManagmentAsync(int managerId, int employeeId, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeManagmentAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeManagmentAsync(int id, int managerId, int employeeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<EmployeeManagment>> GetManagerByRoleIdAsync(int employeeId, CancellationToken cancellationToken = default);
    }
}

