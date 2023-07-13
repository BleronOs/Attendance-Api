using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services
{
	public class RemarksService:IRemarksService
    {
		private readonly IAsyncRepository<Remarks> _remarksRepository;
		public RemarksService(IAsyncRepository<Remarks> remarksRepository)
		{
			_remarksRepository = remarksRepository;
		}
        public async Task<IReadOnlyList<Remarks>> GetRemarksAsync(int id,CancellationToken cancellationToken = default)
        {
            var specification = new ReadOnlyRemarksIncludeEmployee(id);
            return await _remarksRepository.ListAsync(specification);
        }
        public async Task<bool> AddRemarksAsync(int employeeId, string notes, CancellationToken cancellationToken = default)
        {
            var remarks = await _remarksRepository.AddAsync(new Remarks
            {
                EmployeeId = employeeId,
                Notes = notes,
                InsertedDateTime = DateTime.Now
            }, cancellationToken) ;

            return remarks;
        }
    }
}

