using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.ResultModels;
using AttendanceTracker.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker.Core.Services
{
	public class EmployeeService : IEmployeeService
	{

		private readonly IAsyncRepository<Employee> _employeeRepository;
        private readonly IAsyncRepository<Manager> _managerRepository;

		public EmployeeService(IAsyncRepository<Employee> employeeRepository, IAsyncRepository<Manager> managerRepository)
		{
			_employeeRepository = employeeRepository;
            _managerRepository = managerRepository;

        }

		public async Task<IReadOnlyList<Employee>> GetEmployeesAsync(CancellationToken cancellationToken = default)
		{
            var Employees = await _employeeRepository.ListAllAsync();
			return Employees;
		}

        public async Task<IReadOnlyList<Employee>> GetEmployeeNotAsManagerAsync(CancellationToken cancellationToken = default)
        {
            var employeesNotAsManager = new ReadOnlyEmployeeNotAsManager();
            return await _employeeRepository.ListAsync(employeesNotAsManager);
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeeThatAreNotManagerAsync(CancellationToken cancellationToken = default)
        {
            var employeesThatAreNotManager = new ReadOnlyEmployeeThatAreNotManagers();
            return await _employeeRepository.ListAsync(employeesThatAreNotManager);
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeeNotLinkedIntoEmployeeManagmentAsync(CancellationToken cancellationToken = default)
        {
            var employeesNotLinkedIntoEmployeeManagment = new ReadOnlyEmployeNotLinkedIntoEmployeeManagment();
            return await _employeeRepository.ListAsync(employeesNotLinkedIntoEmployeeManagment);
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeeJobPositionAsync(CancellationToken cancellationToken = default)
        {
            var EmployeeJobPosition = new ReadOnlyEmployeeWithJobPosition();
            return await _employeeRepository.ListAsync(EmployeeJobPosition);
        }


        public async Task<IReadOnlyList<Employee>> GetEmployeeManagmentAsync(CancellationToken cancellationToken = default)
		{
			var specification = new ReadonlyEmployeeWithCardSpecification();
			return await _employeeRepository.ListAsync(specification);
		}
        public async Task<IReadOnlyList<Employee>> GetEmployeesWithoutCardAndStatusPasiveAsync(CancellationToken cancellationToken = default)
        {
            var specification = new ReadOnlyEmployeeWithoutCardAndStatusPasive();
            return await _employeeRepository.ListAsync(specification);
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeesWithStatusActiveAsync(CancellationToken cancellationToken = default)
        {
            var specification = new ReadOnlyEmployeWithStatusActiveSpecifications();
            return await _employeeRepository.ListAsync(specification);
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeesWithStatusPassiveAsync(CancellationToken cancellationToken = default)
        {
            var specification = new ReadOnlyEmployeeWithStatusPassiveSpecification();
            return await _employeeRepository.ListAsync(specification);
        }

        public async Task<bool> AddEmployeesAsync(string firstName, string lastName, DateTime birthDate, long personalNumber, string address, string email, string phoneNumber, int positionID,bool status , string userId, CancellationToken cancellationToken = default)
		{
			var employees = await _employeeRepository.AddAsync(new Employee
			{
				FirstName = firstName,
				LastName = lastName,
				BirthDate = birthDate,
				PersonalNumber = personalNumber,
				Address = address,
				Email = email,
				PhoneNumber = phoneNumber,
				PositionId = positionID,
				Status = status,
                UserId = userId,
			}, cancellationToken);
			return employees;
		}


		public async Task<UpdateEmployeeStatusResult> UpdateEmployeeStatusToFalseAsync(int id, CancellationToken cancellationToken = default)
		{
            var updateEmployeeStatusResult = new UpdateEmployeeStatusResult();

            var inactiveEmployeeSpecification = new ReadonlyInactiveManagerByEmployeeIdSpecification(id);
            var inactiveEmployee = await _employeeRepository.FirstOrDefaultAsync(inactiveEmployeeSpecification, cancellationToken);
            if (inactiveEmployee != null)
            {
                updateEmployeeStatusResult.HasActiveManager = true;
                return updateEmployeeStatusResult;

            }
            else
            {
                updateEmployeeStatusResult.HasActiveManager = false;
            }

            var employeeStatusUpdate = await _employeeRepository.GetByIdAsync(id);
            employeeStatusUpdate.Status = false;
            await _employeeRepository.UpdateAsync(employeeStatusUpdate);

            return updateEmployeeStatusResult;

        }

        public async Task<bool> UpdateManagerWhileEmployeeIsActiveAsync(int id, CancellationToken cancellationToken = default)
        {

            var inactiveManagerSpecification = new ReadOnlyInactiveManagersWhileEmployeeIsInactiveSpecifications(id);
            var inactiveManager = await _employeeRepository.FirstOrDefaultAsync(inactiveManagerSpecification, cancellationToken);
            if (inactiveManager != null)
            {
                var getmanagerbyId = new getManagerbyEmployeeId(id);
                var managerStatusUpdate = await _managerRepository.FirstOrDefaultAsync(getmanagerbyId,cancellationToken);

                managerStatusUpdate.Status = true;
                await _managerRepository.UpdateAsync(managerStatusUpdate);
                return true;
            }

            return false;

        }


        public async Task<bool> UpdateEmployeeAsync(int id, string firstName, string lastName, DateTime birthDate, long personalNumber, string address, string email, string phoneNumber, int positionID,bool status , CancellationToken cancellationToken = default)
		{
			var employeeToUpdate = await _employeeRepository.GetByIdAsync(id);

			employeeToUpdate.FirstName = firstName;
			employeeToUpdate.LastName = lastName;
			employeeToUpdate.BirthDate = birthDate;
			employeeToUpdate.PersonalNumber = personalNumber;
			employeeToUpdate.Address = address;
			employeeToUpdate.Email = email;
			employeeToUpdate.PhoneNumber = phoneNumber;
			employeeToUpdate.PositionId = positionID;
			employeeToUpdate.Status = status;
			await _employeeRepository.UpdateAsync(employeeToUpdate);

			return true;
		}

        public async Task<UpdateEmployeeStatusResult> UpdateEmployeeToSendToArchiveAsync(int id,string notes, CancellationToken cancellationToken = default)
        {
            var employeeToUpdate = await _employeeRepository.GetByIdAsync(id);
            employeeToUpdate.Status = false;
            employeeToUpdate.Notes = notes;

            await _employeeRepository.UpdateAsync(employeeToUpdate);

            var updateEmployeeStatusResult = new UpdateEmployeeStatusResult();

            var inactiveEmployeeSpecification = new ReadonlyInactiveManagerByEmployeeIdSpecification(id);
            var inactiveEmployee = await _employeeRepository.FirstOrDefaultAsync(inactiveEmployeeSpecification, cancellationToken);

            if (inactiveEmployee != null)
            {
                updateEmployeeStatusResult.HasActiveManager = true;
            }
            else
            {
                updateEmployeeStatusResult.HasActiveManager = false;
                var employeeStatusUpdate = await _employeeRepository.GetByIdAsync(id);
                employeeStatusUpdate.Status = false;
                await _employeeRepository.UpdateAsync(employeeStatusUpdate);
            }

            return updateEmployeeStatusResult;
        }
        public async Task<IReadOnlyList<Employee>> GetEmployeeWithoutManagerAsync(CancellationToken cancellationToken = default)
        {
            var employeeManagment = new ReadOnlyManagersWithoutEmployeesSpecifications();
			var employees = await _employeeRepository.ListAsync(employeeManagment, cancellationToken);
            return employees;
        }

        public async Task<IReadOnlyList<Employee>> GetManagerNotTwiceAsync(CancellationToken cancellationToken = default)
        {
            var managers = new ReadOnlyManagersSpecifications();
            var employees = await _employeeRepository.ListAsync(managers, cancellationToken);
            return employees;
        }

        public async Task<IReadOnlyList<Employee>> GetEmployeeByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var employeeByUserId = new ReadOnlyEmployeeByUserIdSpecification(userId);
            var employees = await _employeeRepository.ListAsync(employeeByUserId, cancellationToken);
            return employees;
        }
        public async Task<IReadOnlyList<Employee>> GetEmployeeBasedInManagerIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var managersByRoleId = new ReadOnlyEmployeeBasedInManagerId(employeeId);
            var employees = await _employeeRepository.ListAsync(managersByRoleId, cancellationToken);
            return employees;
        }

      

    }
}

