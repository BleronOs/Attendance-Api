using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.ResultModels;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services
{
	public class JobPositionService : IJobPositionService
	{
		private readonly IAsyncRepository<JobPosition> _jobPositionRepository;
		private readonly IAsyncRepository<Employee> _employeeRepository;

		public JobPositionService(IAsyncRepository<JobPosition> jobPositionRepository, IAsyncRepository<Employee> employee)
		{
			_jobPositionRepository = jobPositionRepository;
            _employeeRepository = employee;
        }

		public async Task<bool> AddJobPositionAsync(string name,CancellationToken cancellationToken = default)
		{
			var created = await _jobPositionRepository.AddAsync(new JobPosition
			{
				PositionName = name,
				Status = true,
			}, cancellationToken);

			return created;
		}

		public async Task<IReadOnlyList<JobPosition>> GetJobPositionsAsync(CancellationToken cancellationToken = default)
		{
			var jobPositions = await _jobPositionRepository.ListAllAsync();
			return jobPositions;
		}
        public async Task<IReadOnlyList<JobPosition>> GetJobPositionsStatusTrueAsync(CancellationToken cancellationToken = default)
        {
            var jobPositionStatusTrue = new ReadOnlyJobPositionStatusActive();
            return await _jobPositionRepository.ListAsync(jobPositionStatusTrue);
        }
        public async Task<UpdateJobPositionStatusResult> UpdateJobPositionStatusAsync(int id, CancellationToken cancellationToken = default)
		{
			var updateJobPositionStatus = new UpdateJobPositionStatusResult();

			var inactiveJobPositionSpecification = new ReadOnlyInactiveJobPositionsByEmployee(id);
			var inactiveJobPosition = await _employeeRepository.FirstOrDefaultAsync(inactiveJobPositionSpecification,cancellationToken);

			if(inactiveJobPosition != null)
			{
				updateJobPositionStatus.HasActiveJobPosition = true;
				return updateJobPositionStatus;

			}
			else
			{
				updateJobPositionStatus.HasActiveJobPosition = false;
			}
			var jobPositionStatusUpdate = await _jobPositionRepository.GetByIdAsync(id);

			jobPositionStatusUpdate.Status = false;
			await _jobPositionRepository.UpdateAsync(jobPositionStatusUpdate);

			return updateJobPositionStatus;
		}

		public async Task<bool> UpdateJobPositionAsync(int id, string jobPositionName,bool status, CancellationToken cancellationToken = default)
		{
            var jobPositionToUpdate = await _jobPositionRepository.GetByIdAsync(id);

			jobPositionToUpdate.PositionName = jobPositionName;
			jobPositionToUpdate.Status = status;

			await _jobPositionRepository.UpdateAsync(jobPositionToUpdate);

			return true;
        }

       
    }
}

