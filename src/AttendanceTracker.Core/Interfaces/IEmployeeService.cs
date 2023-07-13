using System;
using System.Collections.Generic;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.ResultModels;

namespace AttendanceTracker.Core.Interfaces
{
	public interface IEmployeeService
	{
        Task<IReadOnlyList<Employee>> GetEmployeesAsync(CancellationToken cancellationToken = default);
        Task<bool> AddEmployeesAsync(string firstName, string lastName, DateTime birthDate, long personalNumber, string address, string email, string phoneNumber, int positionID, bool status, string userId,CancellationToken cancellationToken = default);
        Task<UpdateEmployeeStatusResult> UpdateEmployeeStatusToFalseAsync(int id, CancellationToken cancellationToken);
        Task<bool> UpdateEmployeeAsync(int id, string firstName, string lastName, DateTime birthDate, long personalNumber, string address, string email, string phoneNumber, int positionID,bool status,CancellationToken cancellationToken = default);
        Task<UpdateEmployeeStatusResult> UpdateEmployeeToSendToArchiveAsync(int id,string notes, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeManagmentAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeWithoutManagerAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetManagerNotTwiceAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeesWithoutCardAndStatusPasiveAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeJobPositionAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeNotAsManagerAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeThatAreNotManagerAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeNotLinkedIntoEmployeeManagmentAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeesWithStatusActiveAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeesWithStatusPassiveAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeByUserIdAsync(string userId,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Employee>> GetEmployeeBasedInManagerIdAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<bool> UpdateManagerWhileEmployeeIsActiveAsync(int id, CancellationToken cancellationToken = default);
     



    }
}

