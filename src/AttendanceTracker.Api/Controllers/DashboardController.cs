using System;
using AttendanceTracker.Api.Interfaces;
using AttendanceTracker.Api.Models;
using AttendanceTracker.Api.ViewModels;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ExtendedBaseController
    {
        private readonly IDashBoardService _dashboardService;
        private readonly IEmployeeService _employeeService;
        private readonly ICheckService _checkService;

        public DashboardController(SignInManager<User> signInManager, IDashBoardService dashboardService, IEmployeeService employeeService, ICheckService checkService) : base(signInManager)
        {
            _dashboardService = dashboardService;
            _employeeService = employeeService;
            _checkService = checkService;
        }

        [HttpGet("list-employees/{managerId}")]
        public async Task<IActionResult> GetEmployeesByManager(int managerId, CancellationToken cancellationToken = default)
        {
            var employees = await _dashboardService.GetEmployeeCountByManagerAsync(managerId);
            return Ok(employees);
        }
        [HttpGet("list-pasive-employees/{managerId}")]
        public async Task<IActionResult> GetPasiveEmployeesByManager(int managerId, CancellationToken cancellationToken = default)
        {
            var employees = await _dashboardService.GetPasiveEmployeeCountByManagerAsync(managerId);
            return Ok(employees);
        }
        [HttpGet("list-pasive-cards/{managerId}")]
        public async Task<IActionResult> GetPasiveCardsByManager(int managerId, CancellationToken cancellationToken = default)
        {
            var employees = await _dashboardService.GetCardStatusFalseByManagerAsync(managerId);
            return Ok(employees);
        }
        [HttpGet("list-active-cards/{managerId}")]
        public async Task<IActionResult> GetTrueCardsByManager(int managerId, CancellationToken cancellationToken = default)
        {
            var employees = await _dashboardService.GetCardStatusTrueByManagerAsync(managerId);
            return Ok(employees);
        }

        // Total Target Weekly

        [HttpGet("total-target-of-hours/{managerId}")]
        public async Task<IActionResult> GetTargetofHoursOfAllEmployeesInCompany(int managerId, CancellationToken cancellationToken)
        {
            DateTime dateTime = DateTime.Now;
            List<WeekHourseTarger> weeklyTargetHours = new List<WeekHourseTarger>();
            var employees = await _employeeService.GetEmployeeBasedInManagerIdAsync(managerId, cancellationToken);
            var employeesCount = employees.Count() * 8;

            dateTime = DateTime.Now.AddDays(+1);
            var sevenDaysLast = dateTime;
            sevenDaysLast = DateTime.Now.AddDays(-6);

            for (DateTime date = (DateTime)sevenDaysLast; date <= dateTime; date = date.AddDays(+1))
            {
                var resultOfCheckInOut = await _dashboardService.GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync(managerId,(DateTime)date, cancellationToken);

                var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);

                double hoursDiff = 0;
                double totalHourse = 0;

                foreach (var item in groupByCardId)
                {
                    var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();
                    if (orderedList.Count() < 2)
                    {
                        continue;
                    }
                    Check firstDateCheck = orderedList[0];
                    Check lastDateCheck = orderedList[orderedList.Count() - 1];
                    TimeSpan timeDiff = lastDateCheck.CheckDateTime - firstDateCheck.CheckDateTime;
                    hoursDiff += timeDiff.TotalMinutes;

                }
                if (hoursDiff >= 60)
                {
                    int hours = (int)(hoursDiff / 60);
                    double getMinutes = (hoursDiff - (int)(hours * 60)) / 100;
                    totalHourse = (double)(hours) + getMinutes;

                }
                else
                {
                    totalHourse = hoursDiff / 100;
                }

                DayOfWeek dayOfWeek = date.DayOfWeek;
                string dayOfWeekString = dayOfWeek.ToString();

                var getCheckCount = new WeekHourseTarger()
                {
                    TargetHourse = employeesCount,
                    CompletedHourse = Math.Round(totalHourse, 2),
                    DayOfWeeks = dayOfWeekString,
                };
                weeklyTargetHours.Add(getCheckCount);

            }
            return Ok(weeklyTargetHours);
        }

        // Employees For Evidenve Weekly Check Registered

        [HttpGet("weekly/statistics/{managerId}")]
        public async Task<IActionResult> GetResultOfCheckInOutHoursCalculated2(int managerId, CancellationToken cancellationToken)
        {
            List<WeekEvidenceStatistice> countList = new List<WeekEvidenceStatistice>();
            DateTime dateTime = DateTime.Now;

            var employees = await _employeeService.GetEmployeeBasedInManagerIdAsync(managerId,cancellationToken);
            var employeesCount = employees.Count();

            dateTime = DateTime.Now.AddDays(+1);
            var sevenDaysLast = dateTime;
            sevenDaysLast = DateTime.Now.AddDays(-6);

            for (DateTime date = (DateTime)sevenDaysLast; date <= dateTime; date = date.AddDays(+1))
            {
                var resultOfCheckInOut = await _dashboardService.GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync(managerId, (DateTime)date, cancellationToken);

                var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);

                int count = 0;
                foreach (var item in groupByCardId)
                {
                    var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();
                    if (orderedList == null)
                    {
                        count = -1;
                    }
                    Check firstDateCheck = orderedList[0];
                    Check secondDateCheck = null;
                    if (orderedList.Count() >= 2)
                        secondDateCheck = orderedList[orderedList.Count() - 1];
                    count++;
                }

                DayOfWeek dayOfWeek = date.DayOfWeek;
                string dayOfWeekString = dayOfWeek.ToString();

                var getCheckCount = new WeekEvidenceStatistice()
                {
                    ActiveEmployee = count,
                    PassiveEmployee = employeesCount - count,
                    DayOfWeeks = dayOfWeekString
                };
                countList.Add(getCheckCount);
            }
            return Ok(countList);
        }

        // Total Target Weekly for an specific Employee
        [HttpGet("total-target-of-hours-for-an-employee/{employeeId}")]
        public async Task<IActionResult> GetTargetofHoursOfAnEmployee(int employeeId, CancellationToken cancellationToken)
        {
            DateTime dateTime = DateTime.Now;
            List<WeekHourseTarger> weeklyTargetHours = new List<WeekHourseTarger>();
            var employeesCount =  8;

            dateTime = DateTime.Now.AddDays(+1);
            var sevenDaysLast = dateTime;
            sevenDaysLast = DateTime.Now.AddDays(-6);

            for (DateTime date = (DateTime)sevenDaysLast; date <= dateTime; date = date.AddDays(+1))
            {
                var resultOfCheckInOut = await _checkService.GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync((DateTime)date, employeeId, cancellationToken);

                var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);

                double hoursDiff = 0;
                double totalHourse = 0;

                foreach (var item in groupByCardId)
                {
                    var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();
                    if (orderedList.Count() < 2)
                    {
                        continue;
                    }
                    Check firstDateCheck = orderedList[0];
                    Check lastDateCheck = orderedList[orderedList.Count() - 1];
                    TimeSpan timeDiff = lastDateCheck.CheckDateTime - firstDateCheck.CheckDateTime;
                    hoursDiff += timeDiff.TotalMinutes;

                }
                if (hoursDiff >= 60)
                {
                    int hours = (int)(hoursDiff / 60);
                    double getMinutes = (hoursDiff - (int)(hours * 60)) / 100;
                    totalHourse = (double)(hours) + getMinutes;

                }
                else
                {
                    totalHourse = hoursDiff / 100;
                }

                DayOfWeek dayOfWeek = date.DayOfWeek;
                string dayOfWeekString = dayOfWeek.ToString();

                var getCheckCount = new WeekHourseTarger()
                {
                    TargetHourse = employeesCount,
                    CompletedHourse = Math.Round(totalHourse, 2),
                    DayOfWeeks = dayOfWeekString,
                };
                weeklyTargetHours.Add(getCheckCount);

            }
            return Ok(weeklyTargetHours);
        }


        // Monthly Target Statistic For An Employee

        [HttpGet("total-target-of-hours-monthly-employee/{employeeId}")]
        public async Task<IActionResult> GetTargetofHoursOfAllEmployeesInCompanyMonthly(int employeeId, CancellationToken cancellationToken)
        {
            List<MonthlyHoursCalculate> monthlyTargetHours = new List<MonthlyHoursCalculate>();

            DateTime currentDate = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            double totalHourse = 0;
            double hoursDiff = 0;
            double remainingHours = 0;
            int target = 0;

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(currentDate.Year, currentDate.Month, day);
                if (date.DayOfWeek != DayOfWeek.Sunday)
                {
                    target += 8;
                }

                var resultOfCheckInOut = await _checkService.GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync((DateTime)date, employeeId, cancellationToken);

                var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);

                foreach (var item in groupByCardId)
                {
                    var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();
                    if (orderedList.Count() < 2)
                    {
                        continue;
                    }
                    Check firstDateCheck = orderedList[0];
                    Check lastDateCheck = orderedList[orderedList.Count() - 1];
                    TimeSpan timeDiff = lastDateCheck.CheckDateTime - firstDateCheck.CheckDateTime;
                    hoursDiff += timeDiff.TotalMinutes;

                }
            }
            if (hoursDiff >= 60)
            {
                int hours = (int)(hoursDiff / 60);
                double getMinutes = (hoursDiff - (int)(hours * 60)) / 100;
                totalHourse = (double)(hours) + getMinutes;

                remainingHours = target - 1 + 0.6;
                remainingHours -= totalHourse;

            }
            else
            {
                totalHourse = hoursDiff / 100;
                remainingHours -= totalHourse;
            }
            var getMonthlyHours = new MonthlyHoursCalculate()
            {
                TotalMonthlyHourse = target,
                TotalCompletedHourse = Math.Round(totalHourse, 2),
                DayOfMonth = currentDate,
                RemainingHours = Math.Round(remainingHours, 2)
            };
            monthlyTargetHours.Add(getMonthlyHours);
            return Ok(monthlyTargetHours);
        }


    }
}
