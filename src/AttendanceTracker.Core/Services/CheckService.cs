using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services

{
    public class CheckService : ICheckService
    {
        private readonly IAsyncRepository<Check> _checkRepository;
        public CheckService(IAsyncRepository<Check> checkRepository)
        {
            _checkRepository = checkRepository;
        }

        public async Task<IReadOnlyList<Check>> GetChecksAsync(CancellationToken cancellationToken = default)
        {
            var specification = new GetEmployeeToChecks();
            return await _checkRepository.ListAsync(specification);
            //var checks = await _checkRepository.ListAllAsync();
            //return checks;
        }
        public async Task<IReadOnlyList<Check>> GetTodaysChecksAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            var specification2 = new GetTodaysChecks(dateTime);
            return await _checkRepository.ListAsync(specification2);
        }

        public async Task<IReadOnlyList<Check>> GetTodaysChecksCalculatedAsync(DateTime dateTime, CancellationToken cancellationToken = default)
        {
            var specification2 = new GetTotalHoursFromCheckInOut(dateTime);
            return await _checkRepository.ListAsync(specification2);
        }
        public async Task<bool> AddCheckAsync(DateTime checkDate, int cardId, string? adminId, string? note, CancellationToken cancellationToken = default)
        {
            DateTime time = DateTime.Now;
            var checks = await _checkRepository.AddAsync(new Check
            {
                CheckDateTime = checkDate,
                ServerDateTime = time,
                CardId = cardId,
                AdminId = adminId,
                Note = note
            }, cancellationToken);
            return checks;
        }
        public async Task<IReadOnlyList<Check>> GetChecksByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var checksByEmployeeId = new ReadOnlyChecksByManagerIdSpecification(employeeId);
            var checks = await _checkRepository.ListAsync(checksByEmployeeId, cancellationToken);
            return checks;
        }
        public async Task<IReadOnlyList<Check>> GetTodayChecksByEmployeeIdAsync(DateTime dateTime, int employeeId, CancellationToken cancellationToken = default)
        {
            var checksTodayByEmployeeId = new ReadOnlyCheckInOutByEmployeeIdSpecification(dateTime, employeeId);
            return await _checkRepository.ListAsync(checksTodayByEmployeeId);
        }

        public async Task<IReadOnlyList<Check>> GetEmployeeChecksByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var employeeChecksByEmployeeId = new ReadOnlyEmployeeChecksByEmployeeIdSpecification(employeeId);
            var checks = await _checkRepository.ListAsync(employeeChecksByEmployeeId, cancellationToken);
            return checks;
        }
        public async Task<IReadOnlyList<Check>> GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync(DateTime dateTime,int employeeId, CancellationToken cancellationToken = default)
        {
            var employeeChecksByEmployeeIdAndDateTime = new ReadOnlyChecksByEmployeeIdAndDateTimeSpecification(dateTime,employeeId);
            var checksByEmployeeIdAndDateTime = await _checkRepository.ListAsync(employeeChecksByEmployeeIdAndDateTime, cancellationToken);
            return checksByEmployeeIdAndDateTime;
        }
        public async Task<IReadOnlyList<Check>> GetTodayEmployeeChecksByEmployeeIdAsync(DateTime dateTime, int employeeId, CancellationToken cancellationToken = default)
        {
            var employeeChecksTodayByEmployeeId = new ReadOnlyEmployeeCheckInOutByEmployeeIdSpecification(dateTime, employeeId);
            return await _checkRepository.ListAsync(employeeChecksTodayByEmployeeId);

        }


        public async Task<IReadOnlyList<Check>> GetWeekChecksByEmployeeIdAsync(DateTime dateTime, int employeeId, CancellationToken cancellationToken = default)
        {
            var checksTodayByEmployeeId = new ReadOnlyCheckInOutForWeekSpecification(dateTime, employeeId);
            return await _checkRepository.ListAsync(checksTodayByEmployeeId);
        }

        public async Task<bool> GetEmployeeinChecksListAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var checkifEmployeeHasAnCheckIn = new ReadonlyCheckInForAnSpecificEmployeeSpecification(employeeId);
            var findCheckIn = await _checkRepository.FirstOrDefaultAsync(checkifEmployeeHasAnCheckIn, cancellationToken);
            if (findCheckIn != null) return false;
            return true;
        }
    }
}

