using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Infrastructure.Configuration.Email;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Api.Hangfire
{
    public class HangfireJob
    {

        private readonly IEmailSender _emailSender;
        private readonly IEmployeeService _employeeService;
        private readonly ICheckService _checkService;

        public HangfireJob(IEmailSender emailSender, IEmployeeService employeeService, ICheckService checkService)
        {
            _emailSender = emailSender;
            _employeeService = employeeService;
            _checkService = checkService;

        }

        public async Task SendEmailToEmployeesWhoNotComeToCompanyAtTime(CancellationToken cancellationToken = default)
        {
            var employeeWithStatusActive = await _employeeService.GetEmployeesWithStatusActiveAsync(cancellationToken);
            DateTime today = DateTime.Now;
            foreach (var item in employeeWithStatusActive)
            {
                var findCheckIn = await _checkService.GetEmployeeinChecksListAsync(item.Id, cancellationToken);

                if (!findCheckIn) continue;

                var emailBody = $"Pershendetje {item.FirstName} {item.LastName}, Ju njoftojme se jeni vonuar ne orarin e punes ";
                try
                {
                    await _emailSender.SendAsync(item.Email,"Lajmrim Per Vonese ne Pune", emailBody);

                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
    }
}
