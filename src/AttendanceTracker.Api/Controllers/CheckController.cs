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
    public class CheckController:ExtendedBaseController
	{
		private readonly ICheckService _checkRepository;
		private readonly ICardService _cardService;
        private readonly IEmployeeService _employeeService;

		public CheckController(SignInManager<User> signInManager, ICheckService checkRepository, ICardService cardService, IEmployeeService employeeService) : base(signInManager)
		{
			_checkRepository = checkRepository;
			_cardService = cardService;
            _employeeService = employeeService;
		}

		[HttpGet("list")]
		public async Task<IActionResult> GetChecks(CancellationToken cancellationToken)
		{
			var checks = await _checkRepository.GetChecksAsync(cancellationToken);
			return Ok(checks.OrderByDescending(x => x.Id));
		}

        [HttpGet("todays-checks/list")]
        public async Task<IActionResult> GetResultOfCheckInOut([FromQuery] DateTime? dateTime, CancellationToken cancellationToken)
        {
			if (dateTime == null) dateTime = DateTime.Now;

            var resultOfCheckInOut = await _checkRepository.GetTodaysChecksAsync((DateTime) dateTime, cancellationToken);

			var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);

			List<TodayCheksResponeseCalculated> responseList= new List<TodayCheksResponeseCalculated>();

            double hoursDiff = 0;
            double totalHourse = 0;
            double extraTime = 0;

            foreach (var item in groupByCardId)
			{
                var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();

                Check firstDateCheck = orderedList[0];
                Check secondDateCheck = null;
                if (orderedList.Count() >= 2)
                {
                    secondDateCheck = orderedList[orderedList.Count() - 1];
                    TimeSpan timeDiff = secondDateCheck.CheckDateTime - firstDateCheck.CheckDateTime;
                    hoursDiff += timeDiff.TotalMinutes;

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
                }

                if (totalHourse >= 8)
                {
                    extraTime += (totalHourse - 8);
                    totalHourse -= extraTime;
                }

                var employeeTodaysCheck = new TodayCheksResponeseCalculated()
                {
                    Name = firstDateCheck.Card.Employee.FirstName,
                    LastName = firstDateCheck.Card.Employee.LastName,
                    CardNumber = firstDateCheck.Card.CardRefId,
                    FirstDateTime = firstDateCheck.CheckDateTime,
                    LastDateTime = secondDateCheck?.CheckDateTime,
                    CompletedWeekHours = Math.Round(totalHourse,2),
                    TotalWeekHours = Math.Round(extraTime, 2)
                };
                responseList.Add(employeeTodaysCheck);
                hoursDiff *= 0;
                extraTime *= 0;
                totalHourse *= 0;
            }
            return Ok(responseList);
        }


        [HttpPost("add")]
		public async Task<IActionResult> AddCheck([FromBody] AddCheck addCheck, CancellationToken cancellationToken)
		{
			var card = await _cardService.GetActiveCardByEmployeeIdAsync(addCheck.EmployeeId, true, cancellationToken);
			if (card == null) return NotFound();
            //merre cards ku employeeId = addCheck.cardId edhe merre veq te paren edhe vendosja cardIds
            var checks = await _checkRepository.AddCheckAsync(addCheck.CheckDateTime, card.Id, addCheck.AdminId, addCheck.Note);
			if (checks) return Ok();
			return BadRequest();
		}
        [HttpGet("checks-by-employee-id/{employeeId}")]
        public async Task<IActionResult> GetChecksByEmployeeId(int employeeId, CancellationToken cancellationToken = default)
        {
            var checksByEmployeeId = await _checkRepository.GetChecksByEmployeeIdAsync(employeeId, cancellationToken);
            return Ok(checksByEmployeeId);
        }


        [HttpGet("todays-checks-by-employee-id/{employeeId}")]
        public async Task<IActionResult> GetCheckInOutByEmployeeId([FromQuery] DateTime? dateTime,int employeeId ,CancellationToken cancellationToken)
        {
            if (dateTime == null) dateTime = DateTime.Now;

            var resultOfCheckInOut = await _checkRepository.GetTodayChecksByEmployeeIdAsync((DateTime)dateTime,employeeId ,cancellationToken);

            var groupByCardId = resultOfCheckInOut.GroupBy(s => s.CardId);

            List<TodayCheksResponeseCalculated> responseList = new List<TodayCheksResponeseCalculated>();

            double hoursDiff = 0;
            double totalHourse = 0;
            double extraTime = 0;

            foreach (var item in groupByCardId)
            {
                var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();

                Check firstDateCheck = orderedList[0];
                Check secondDateCheck = null;
                if (orderedList.Count() >= 2)
                {
                    secondDateCheck = orderedList[orderedList.Count() - 1];
                    TimeSpan timeDiff = secondDateCheck.CheckDateTime - firstDateCheck.CheckDateTime;
                    hoursDiff += timeDiff.TotalMinutes;

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
                }

                if (totalHourse >= 8)
                {
                    extraTime += (totalHourse - 8);
                    totalHourse -= extraTime;
                }

                var employeeTodaysCheck = new TodayCheksResponeseCalculated()
                {
                    Name = firstDateCheck.Card.Employee.FirstName,
                    LastName = firstDateCheck.Card.Employee.LastName,
                    CardNumber = firstDateCheck.Card.CardRefId,
                    FirstDateTime = firstDateCheck.CheckDateTime,
                    LastDateTime = secondDateCheck?.CheckDateTime,
                    CompletedWeekHours = Math.Round(totalHourse, 2),
                    TotalWeekHours = Math.Round(extraTime, 2)
                };
                responseList.Add(employeeTodaysCheck);
                hoursDiff *= 0;
                extraTime *= 0;
                totalHourse *= 0;
            }
            return Ok(responseList);
        }

        [HttpGet("employee-checks-by-employee-id/{employeeId}")]
        public async Task<IActionResult> GetEmployeeChecksByEmployeeId(int employeeId, CancellationToken cancellationToken = default)
        {
            var employeeChecksByEmployeeId = await _checkRepository.GetEmployeeChecksByEmployeeIdAsync(employeeId, cancellationToken);
            return Ok(employeeChecksByEmployeeId);
        }

        [HttpGet("employee-checks-by-employee-id-and-datetime/{employeeId}")]
        public async Task<IActionResult> GetEmployeeChecksByEmployeeId([FromQuery] DateTime? dateTime,int employeeId, CancellationToken cancellationToken = default)
        {
            if (dateTime == null) dateTime = DateTime.Now;

            var resultOfCheckInOut = await _checkRepository.GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync((DateTime)dateTime, employeeId, cancellationToken);

            var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);

            List<TodayCheksResponeseCalculated> responseList = new List<TodayCheksResponeseCalculated>();
            double hoursDiff = 0;
            double totalHourse = 0;
            double extraTime = 0;

            foreach (var item in groupByCardId)
            {
                var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();

                Check firstDateCheck = orderedList[0];
                Check secondDateCheck = null;
                if (orderedList.Count() >= 2)
                {
                    secondDateCheck = orderedList[orderedList.Count() - 1];
                    TimeSpan timeDiff = secondDateCheck.CheckDateTime - firstDateCheck.CheckDateTime;
                    hoursDiff += timeDiff.TotalMinutes;

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
                }

                if (totalHourse >= 8)
                {
                    extraTime += (totalHourse - 8);
                    totalHourse -= extraTime;
                }

                var employeeTodaysCheck = new TodayCheksResponeseCalculated()
                {
                    Name = firstDateCheck.Card.Employee.FirstName,
                    LastName = firstDateCheck.Card.Employee.LastName,
                    CardNumber = firstDateCheck.Card.CardRefId,
                    FirstDateTime = firstDateCheck.CheckDateTime,
                    LastDateTime = secondDateCheck?.CheckDateTime,
                    CompletedWeekHours = Math.Round(totalHourse, 2),
                    TotalWeekHours = Math.Round(extraTime, 2)
                };
                responseList.Add(employeeTodaysCheck);
                hoursDiff *= 0;
                extraTime *= 0;
                totalHourse *= 0;
            }
            return Ok(responseList);
        }

        // Dashboard


        // AttendanceTracker of Employee of each day of 7days later from current datetime

        [HttpGet("weekly/statistics")]
        public async Task<IActionResult> GetResultOfCheckInOutHoursCalculated2([FromQuery] DateTime? dateTime, CancellationToken cancellationToken)
        {
            List<WeekEvidenceStatistice> countList = new List<WeekEvidenceStatistice>();
            if (dateTime == null) dateTime = DateTime.Now;

            var employees = await _employeeService.GetEmployeesWithStatusActiveAsync(cancellationToken);
            var employeesCount = employees.Count();

            dateTime = DateTime.Now.AddDays(+1);
            var sevenDaysLast = dateTime;
            sevenDaysLast = DateTime.Now.AddDays(-6);

            for (DateTime date = (DateTime)sevenDaysLast; date <= dateTime; date = date.AddDays(+1))
            {
                var resultOfCheckInOut = await _checkRepository.GetTodaysChecksAsync((DateTime)date, cancellationToken);

            var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);

                int count = 0;
                foreach (var item in groupByCardId)
                {
                    var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();
                    if(orderedList == null)
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

        //get Total Hours Target For all Employees of Company

        [HttpGet("total-target-of-hours")]
        public async Task<IActionResult> GetTargetofHoursOfAllEmployeesInCompany(CancellationToken cancellationToken)
        {
            DateTime dateTime = DateTime.Now;
            List<WeekHourseTarger> weeklyTargetHours = new List<WeekHourseTarger>();
            var employees = await _employeeService.GetEmployeesWithStatusActiveAsync(cancellationToken);
            var employeesCount = employees.Count() * 8;

            dateTime = DateTime.Now.AddDays(+1);
            var sevenDaysLast = dateTime;
            sevenDaysLast = DateTime.Now.AddDays(-6);

            for (DateTime date = (DateTime)sevenDaysLast; date <= dateTime; date = date.AddDays(+1))
            {
                var resultOfCheckInOut = await _checkRepository.GetTodaysChecksAsync((DateTime)date, cancellationToken);

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
                if(hoursDiff >= 60)
                {
                    int hours = (int)(hoursDiff / 60);
                    double getMinutes = (hoursDiff - (int)(hours * 60))/100;
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


        // Get Monthly Hours Calculated

        [HttpGet("total-target-of-hours-monthly")]
        public async Task<IActionResult> GetTargetofHoursOfAllEmployeesInCompanyMonthly(CancellationToken cancellationToken)
        {
            List<MonthlyHoursCalculate> monthlyTargetHours = new List<MonthlyHoursCalculate>();
            var employees = await _employeeService.GetEmployeesWithStatusActiveAsync(cancellationToken);

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

                var resultOfCheckInOut = await _checkRepository.GetTodaysChecksAsync((DateTime)date, cancellationToken);

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
            target *= employees.Count();

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


        // Profile

        // Get weekly hours calculated for a month

        [HttpGet("total-weekly-target-of-hours-monthly/{employeeId}")]
        public async Task<IActionResult> GetWeeklyTargetofHoursOfAllEmployeesInCompanyMonthly(int employeeId, CancellationToken cancellationToken)
            {
            List<GetEvidenceForEachWeekOfMonth> monthlyTargetHours = new List<GetEvidenceForEachWeekOfMonth>();

            DateTime currentDate = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            DateTime lastDate = new DateTime(currentDate.Year, currentDate.Month, daysInMonth);

            int days = 0;
            bool status;

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(currentDate.Year, currentDate.Month, day);
                int days_until_sunday = (7 - (int)date.DayOfWeek) % 7;
                DateTime next_sunday_date = date.AddDays(days_until_sunday);
                if (next_sunday_date >= lastDate)
                {
                    next_sunday_date = lastDate;
                    days = (lastDate - date).Days + 1;
                }
                else
                {
                    days = days_until_sunday;
                }
                
                if (next_sunday_date < currentDate)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
                double totalHourse = 0;
                double hoursDiff = 0;
                for (DateTime datetime = new DateTime(currentDate.Year, currentDate.Month, day); datetime <= next_sunday_date; datetime = datetime.AddDays(+1))
                {
                    day += 1;

                    var resultOfCheckInOut = await _checkRepository.GetTodayEmployeeChecksByEmployeeIdAsync((DateTime)datetime, employeeId,cancellationToken);

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

                }
                else
                {
                    totalHourse = hoursDiff / 100;
                }

                var targetWeeklyHours = 8 * days;
                var getMonthlyHoursForEachWeek = new GetEvidenceForEachWeekOfMonth()
                {
                    WeekTarget = targetWeeklyHours,
                    DoneHours = Math.Round(totalHourse, 2),
                    Status = status
                };
                monthlyTargetHours.Add(getMonthlyHoursForEachWeek);
                day -= 1;
            }
            return Ok(monthlyTargetHours);
        }
        
        
        // Profile Iliri
        
        [HttpGet("weekly-target-hours-employee/{employeeId}")]
        public async Task<IActionResult> GetWeekEmployeeCheckInOutByEmployeeId(int employeeId,CancellationToken cancellationToken)
        {
            List<CalculatedHours> countList = new List<CalculatedHours>();
           
            DateTime now = DateTime.Now;
            DateTime monday = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime sunday = monday.AddDays(6);
            int count = 0;
            double hoursDiff = 0;
            double totalHourse = 0;
            for (DateTime date = (DateTime)monday; date <= sunday; date = date.AddDays(+1))
            {
                var resultOfCheckInOut = await _checkRepository.GetWeekChecksByEmployeeIdAsync(date ,employeeId ,cancellationToken);
                var groupByCardId = resultOfCheckInOut.GroupBy(s => s.Card.EmployeeId);
                if (date == now.AddDays(+1))
                {
                    break;
                }
                count += 8;
                foreach (var item in groupByCardId)
                {
                    
                    var orderedList = item.OrderBy(s => s.CheckDateTime).ToList();
                    if (orderedList.Count() < 2 )
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
            }
            else
            {
                totalHourse = hoursDiff / 100;
            }

            var getCheckCount = new CalculatedHours()
            {
                TotalHoursPerDay = count,
                CompletedHoursPerDay = Math.Round(totalHourse, 2),
                dateTime = sunday
            };
            countList.Add(getCheckCount);


            return Ok(countList);
        }

        [HttpGet("total-target-of-hours-monthly-employee-id/{employeeId}")]
        public async Task<IActionResult> GetTargetofHoursOfAllEmployeesInCompanyMonthly(int employeeId, CancellationToken cancellationToken)
        {
            List<GetMonthlylHoursUsingEmployeeId> monthlyTargetHours = new List<GetMonthlylHoursUsingEmployeeId>();
            DateTime currentDate = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            double totalHourse = 0;
            double hoursDiff = 0;
            int target = 0;
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(currentDate.Year, currentDate.Month, day);
                if (date.DayOfWeek != DayOfWeek.Sunday)
                {
                    target += 8;
                }
                var resultOfCheckInOut = await _checkRepository.GetWeekChecksByEmployeeIdAsync(date, employeeId, cancellationToken);
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
            }
            else
            {
                totalHourse = hoursDiff / 100;
            }
            var getMonthlyHours = new GetMonthlylHoursUsingEmployeeId()
            {
                CompletedHours = Math.Round(totalHourse, 2),
                TargetHours = target,
                CurrentDate = currentDate
            };
            monthlyTargetHours.Add(getMonthlyHours);
            return Ok(monthlyTargetHours);
        }
    }
}