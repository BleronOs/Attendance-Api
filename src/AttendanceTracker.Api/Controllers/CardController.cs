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
    public class CardController : ExtendedBaseController
    {
        private readonly ICardService _cardService;
        private readonly IEmployeeService _employeeService;

        public CardController(SignInManager<User> signInManager, ICardService cardService, IEmployeeService employeeService) : base(signInManager)
        {
            _cardService = cardService;
            _employeeService = employeeService;
        }

        [HttpGet("list")]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        public async Task<IActionResult> GetCards(CancellationToken cancellationToken)
        {
            var cards = await _cardService.GetCardsAsync(cancellationToken);
            return Ok(cards.OrderByDescending(x => x.Id));
        }
        [HttpGet("status/false")]
        public async Task<IActionResult> GetCardsStatusFalse(CancellationToken cancellationToken)
        {
            var cardFalse = await _cardService.GetCardWithStatusFalseAsync(cancellationToken);
            return Ok(cardFalse.OrderByDescending(x => x.Id));
        }


        [HttpGet("eligible-employee-list")]
        public async Task<IActionResult> GetEmployee(CancellationToken cancellationToken = default)
        {
            var employees = await _employeeService.GetEmployeeManagmentAsync(cancellationToken);
            return Ok(employees);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddCards([FromBody] AddCard addCard, CancellationToken cancellationToken)
        {
            var create = await _cardService.AddCardsAsync(addCard.CardRefId, addCard.Status ,addCard.EmployeeId,addCard.ReasonNote, cancellationToken);
            if (create) return Ok();
            return BadRequest();
        }
        [HttpPut ("update/status/{id}")]
        public async Task<IActionResult> UpdateStatusCard(int id, CancellationToken cancellationToken)
        {
            await _cardService.UpdateCardStatusFalseAsync(id, cancellationToken);
            return Ok();
        }
        [HttpPut("update/status/true/{id}/{employeeId}/{cardId}/{status}")]
        public async Task<IActionResult> UpdateStatusCardTrue(int id, int employeeId, string cardId, bool status, CancellationToken cancellationToken)
        {
            await _cardService.UpdateCardStatusTrueAsync(id, employeeId, cardId, status, cancellationToken);
            return Ok();
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCard([FromBody] CardViewModel model, int id, CancellationToken cancellationToken)
        {
            await _cardService.UpdateCardAsync(id, model.CardRefId, model.Status, model.EmployeeId,model.ReasonNote, cancellationToken);

            return Ok();
        }
        [HttpGet("card-by-employee-id/{employeeId}")]
        public async Task<IActionResult> GetCardByEmployeeId(int employeeId, CancellationToken cancellationToken = default)
        {
            var cardByEmployeeId = await _cardService.GetCardByEmployeeIdAsync(employeeId, cancellationToken);
            return Ok(cardByEmployeeId);
        }
        [HttpPut("update/card-if-employee-is-active/{id}/{employeeId}/{statusi}/{cardRefId}")]
        public async Task<IActionResult> UpdateCardIfEmployeeIsactive(int id,int employeeId, bool statusi,string cardRefId,CancellationToken cancellationToken)
        {
            var result =await _cardService.UpdateCardStatusIfEmployeeIsActiveAsync(id, employeeId, statusi,cardRefId,cancellationToken);

            if (!result)
            {
                return StatusCode(2000);
            }
            return Ok();
        }
    }
}

