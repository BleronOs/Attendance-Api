using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.ResultModels;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services
{
	public class ManagerService : IManagerService
	{
		private readonly IAsyncRepository<Manager> _managerRepository;
		private readonly IAsyncRepository<EmployeeManagment> _employeeManagmentRepository;

		public ManagerService(IAsyncRepository<Manager> managerRepository, IAsyncRepository<EmployeeManagment> employeeManagmentRepository)
		{
			_managerRepository = managerRepository;
			_employeeManagmentRepository = employeeManagmentRepository;
		}

		public async Task<IReadOnlyList<Manager>> GetManagersAsync(CancellationToken cancellationToken = default)
		{
			var Managers = await _managerRepository.ListAllAsync();
			return Managers;
		}
        public async Task<IReadOnlyList<Manager>> GetEmployeeManagmentAsync(CancellationToken cancellationToken = default)
        {
			var Specifications = new ReadOnlyEmloyeeManagmentByManagersSpecifications();
            return await _managerRepository.ListAsync(Specifications);
        }

        public async Task<bool> AddManagersAsync(int employeeID, bool status,CancellationToken cancellationToken = default)
		{
			var managerSpecification = new ManagerByEmployeeIdSpecification(employeeID);
			var manager = await _managerRepository.FirstOrDefaultAsync(managerSpecification, cancellationToken);
			if(manager != null)
			{
				manager.Status = status;
				return await _managerRepository.UpdateAsync(manager);
			}
			var managers = await _managerRepository.AddAsync(new Manager
			{
				EmployeeId = employeeID,
				Status = status,
			}, cancellationToken);

			return managers;

        }

        public async Task<bool> UpdateManagersAsync(int id, int employeeID, bool status,CancellationToken cancellationToken = default)
		{
			var managerToUpdate = await _managerRepository.GetByIdAsync(id);
			managerToUpdate.EmployeeId = employeeID;
			managerToUpdate.Status = status;

			await _managerRepository.UpdateAsync(managerToUpdate);
			return true;
		}

        public async Task<IReadOnlyList<Manager>> GetManagersWithStatusActiveAsync(CancellationToken cancellationToken = default)
        {
            var managersWithStatusActive = new ReadOnlyManagersWithStatusActiveSpecifications();
			return await _managerRepository.ListAsync(managersWithStatusActive);
        }

        public async Task<IReadOnlyList<Manager>> GetManagersWithStatusPassiveAsync(CancellationToken cancellationToken = default)
        {
            var managersWithStatusPassive = new ReadOnlyManagersWithStatusPassiveSpecification();
            return await _managerRepository.ListAsync(managersWithStatusPassive);
        }

        public async Task<bool> UpdateManagerStatusToFalseAsync(int id, CancellationToken cancellationToken = default)
        {
			var CheckIfManagerHasAnEmployeeInManagment = new ReadOnlyIfExistAnEmployeeInManagmentSpecification(id);
            var findEmployeeInManagment = await _employeeManagmentRepository.FirstOrDefaultAsync(CheckIfManagerHasAnEmployeeInManagment, cancellationToken);
			
			if (findEmployeeInManagment != null) return false;

            var managerStatusUpdate = await _managerRepository.GetByIdAsync(id);
            managerStatusUpdate.Status = false;
            await _managerRepository.UpdateAsync(managerStatusUpdate);

            return true;
        }

       
    }
}

		