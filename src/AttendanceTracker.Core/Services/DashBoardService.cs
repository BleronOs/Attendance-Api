using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IAsyncRepository<Employee> _employeeRepository;
        private readonly IAsyncRepository<Card> _cardRepository;
        private readonly IAsyncRepository<Check> _checkRepository;
        public DashBoardService(IAsyncRepository<Employee> employeeRepository, IAsyncRepository<Card> cardRepository, IAsyncRepository<Check> checkRepository)
        {
            _employeeRepository = employeeRepository;
            _cardRepository = cardRepository;
            _checkRepository = checkRepository;
        }

        // employees in managment from a specification manager
        public async Task<int> GetEmployeeCountByManagerAsync(int managerId, CancellationToken cancellationToken = default)
        {
            var employeesInManagement = new ReadonlyAllEmployeesByManagerSpecification(managerId);
            var employees = await _employeeRepository.ListAsync(employeesInManagement);
            return employees.Count;
        }

        public async Task<int> GetPasiveEmployeeCountByManagerAsync(int managerId, CancellationToken cancellationToken = default)
        {
            var pasiveEmployeesInManagment = new ReadonlyAllPasiveEmployeesByManagerSpecification(managerId);
            var pasiveEmployees = await _employeeRepository.ListAsync(pasiveEmployeesInManagment);
            return pasiveEmployees.Count;
        }
        // cards of employees in managment from a specification manager
        public async Task<int> GetCardStatusFalseByManagerAsync(int managerId, CancellationToken cancellationToken = default)
        {
            var pasiveCardsInManagmentByManager = new ReadOnlyCardStatusFalseByManagerIdSpecification(managerId);
            var pasiveCards = await _cardRepository.ListAsync(pasiveCardsInManagmentByManager);
            return pasiveCards.Count;
        }
        public async Task<int> GetCardStatusTrueByManagerAsync(int managerId, CancellationToken cancellationToken = default)
        {
            var pasiveCardsInManagmentByManager = new ReadOnlyCardByEmployeeIdSpecification(managerId);
            var pasiveCards = await _cardRepository.ListAsync(pasiveCardsInManagmentByManager);
            return pasiveCards.Count;
        }

        // Total Weekly Hours For Employees In Managment

        public async Task<IReadOnlyList<Check>> GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync(int managerId, DateTime dateTime, CancellationToken cancellationToken = default)
        {
            var employeeChecksEmployeesByManagerIdAndDateTime = new ReadOnlyChecksForEmployeesInManagmentSpecification(managerId, dateTime);
            var employeesChecks = await _checkRepository.ListAsync(employeeChecksEmployeesByManagerIdAndDateTime, cancellationToken);
            return employeesChecks;
        }
    }
}
