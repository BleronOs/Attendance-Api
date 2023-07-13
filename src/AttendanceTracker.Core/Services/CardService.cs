using System;
using System.Net.NetworkInformation;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services
{
	public class CardService : ICardService
	{
		private readonly IAsyncRepository<Card> _cardRepository;

		public CardService(IAsyncRepository<Card> cardRespository)
		{
			_cardRepository = cardRespository;
		}

		public async Task<IReadOnlyList<Card>> GetCardsAsync(CancellationToken cancellationToken = default)
		{
			var specification = new GetEmployeeToCard();
			return await _cardRepository.ListAsync(specification);
		}
		public async Task<IReadOnlyList<Card>> GetCardWithStatusFalseAsync(CancellationToken cancellationToken = default)
		{
			var cardStatusFalse = new ReadOnlyCardWithStatusFalse();
			return await _cardRepository.ListAsync(cardStatusFalse);
		}

        //public async Task<bool> AddCardsAsync(string cardId, bool status, int employeeId, string notes, CancellationToken cancellationToken = default)
        //{
        //          var checkCardIfHasAny = new ReadOnlyCardsForTheFirstTimeInsertedSpecification(employeeId);
        //          var oldCard = await _cardRepository.FirstOrDefaultAsync(checkCardIfHasAny, cancellationToken);

        //          if (oldCard == null)
        //          {
        //              var newCard = await _cardRepository.AddAsync(new Card
        //              {
        //                  CardRefId = cardId,
        //                  Status = status,
        //                  EmployeeId = employeeId,
        //                  ReasonNote = notes,
        //                  InsertedDateTime = DateTime.Now
        //              }, cancellationToken);

        //              return newCard;
        //          }
        //          else
        //          {
        //              if (oldCard.Status == status)
        //              {
        //                  return false;
        //              }
        //              else
        //              {
        //                  oldCard.Status = false;

        //                  var newCard = await _cardRepository.AddAsync(new Card
        //                  {
        //                      CardRefId = cardId,
        //                      Status = status,
        //                      EmployeeId = employeeId,
        //                      ReasonNote = notes,
        //                      InsertedDateTime = DateTime.Now
        //                  }, cancellationToken);
        //                  return newCard;
        //              }

        //          }
        //          return false;
        //	//return false;
        // //             if (status == false) await ResetActiveCardStatusToFalseAsync(employeeId, "Pas krijimit te nje kartele te re, kjo kartele eshte kthyer automatikisht ne kartele pasive", cancellationToken);

        // //             return cards;
        //          }

        public async Task<bool> AddCardsAsync(string cardId, bool status, int employeeId, string notes, CancellationToken cancellationToken = default)
        {
            var checkCardIfHasAny = new ReadOnlyCardsForTheFirstTimeInsertedSpecification(employeeId);
            var oldCard = await _cardRepository.FirstOrDefaultAsync(checkCardIfHasAny, cancellationToken);

            if (oldCard == null)
            {
                var newCard = await _cardRepository.AddAsync(new Card
                {
                    CardRefId = cardId,
                    Status = status,
                    EmployeeId = employeeId,
                    ReasonNote = notes,
                    InsertedDateTime = DateTime.Now
                }, cancellationToken);

                return newCard != null;
            }
            else
            {
                if (oldCard.Status == !status)
                {
                    return false;
                }
                else
                {
                    oldCard.Status = false;
                    var updatedOldCard = await _cardRepository.UpdateAsync(oldCard, cancellationToken);

                    var newCard = await _cardRepository.AddAsync(new Card
                    {
                        CardRefId = cardId,
                        Status = status,
                        EmployeeId = employeeId,
                        ReasonNote = notes,
                        InsertedDateTime = DateTime.Now
                    }, cancellationToken);

                    return updatedOldCard != null && newCard != null;
                }
            }
        }
        private async Task<bool> ResetActiveCardStatusToFalseAsync(int employeeId, string notes, CancellationToken cancellationToken = default)
		{
            var activeCard = await GetActiveCardByEmployeeIdAsync(employeeId, false, cancellationToken);
            if (activeCard != null)
            {
                activeCard.Status = false;
                activeCard.UpdatedDateTime = DateTime.Now;
				if (!string.IsNullOrEmpty(notes))
					activeCard.Note = notes;
                return await _cardRepository.UpdateAsync(activeCard, cancellationToken);
            }
			return false;
        }

        public async Task<bool> UpdateCardStatusFalseAsync(int id, CancellationToken cancellationToken = default)
		{
			var cardStatusUpdate = await _cardRepository.GetByIdAsync(id);

			cardStatusUpdate.Status = false;
			await _cardRepository.UpdateAsync(cardStatusUpdate);


			return true;
		}
		public async Task<bool> UpdateCardStatusTrueAsync(int id,int employeeId,string cardRef,bool status, CancellationToken cancellationToken = default)
		{
			var cardStatusTrue = await _cardRepository.GetByIdAsync(id);
			cardStatusTrue.Status = true;
			cardStatusTrue.Note = "Kjo kartel eshte kthyer Perseri ne Kartele Aktive, nga Arkiva";
            if (status == false) await ResetActiveCardStatusToFalseAsync(employeeId, $"Pas aktivizimit te karteles me numer unik {cardRef}, kjo kartele eshte kthyer automatikisht ne kartele pasive", cancellationToken);
            return await _cardRepository.UpdateAsync(cardStatusTrue);
		}

        public async Task<Card> GetActiveCardByEmployeeIdAsync(int employeeId, bool isReadonly, CancellationToken cancellationToken = default)
		{
			var specification = new ActiveCardByEmployeeIdSpecification(employeeId, isReadonly);
			return await _cardRepository.FirstOrDefaultAsync(specification, cancellationToken);
		}

        public async Task<bool> UpdateCardAsync(int id, string cardRef, bool status, int employeeId, string notes, CancellationToken cancellationToken)
		{
			var cardToUpdate = await _cardRepository.GetByIdAsync(id);

			cardToUpdate.CardRefId = cardRef;
			cardToUpdate.Status = status;
			cardToUpdate.EmployeeId = employeeId;
			cardToUpdate.ReasonNote = notes;
			cardToUpdate.UpdatedDateTime = DateTime.Now;
            return await _cardRepository.UpdateAsync(cardToUpdate);
		}
      

        public async Task<IReadOnlyList<Card>> GetCardByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var cardByEmployeeId = new ReadOnlyCardByEmployeeIdSpecification(employeeId);
            var cards = await _cardRepository.ListAsync(cardByEmployeeId, cancellationToken);
            return cards;
        }

        public async Task<bool> UpdateCardStatusIfEmployeeIsActiveAsync(int id, int employeeId,bool statusi,string cardRefId,CancellationToken cancellationToken)
        {
            var cardStatusIfEmployeeIsActive = new ActivateCardIfEmployeeIsActiveSpecification(employeeId);
            var findEmployeeActive =await _cardRepository.FirstOrDefaultAsync(cardStatusIfEmployeeIsActive, cancellationToken);

			if (findEmployeeActive != null)
			{

                //var cardToUpdate = await _cardRepository.GetByIdAsync(id);


                //cardToUpdate.EmployeeId = employeeId;
                //            cardToUpdate.Status = true;
                //            return await _cardRepository.UpdateAsync(cardToUpdate);
                var cardStatusTrue = await _cardRepository.GetByIdAsync(id);
                cardStatusTrue.Status = true;
                cardStatusTrue.Note = "Kjo kartel eshte kthyer Perseri ne Kartele Aktive, nga Arkiva";
                if (statusi == false) await ResetActiveCardStatusToFalseAsync(employeeId, $"Pas aktivizimit te karteles me numer unik {cardRefId}, kjo kartele eshte kthyer automatikisht ne kartele pasive", cancellationToken);
                return await _cardRepository.UpdateAsync(cardStatusTrue);

            }

			return false;
        }


        public async Task<bool> UpdateCardWhenEmployeeIsPassiveAsync(int employeeId,CancellationToken cancellationToken)
        {
            var cardStatusIfEmployeeIsActive = new ReadOnlyActivateCardByEmployeeId(employeeId);
            var findEmployeeActive = await _cardRepository.FirstOrDefaultAsync(cardStatusIfEmployeeIsActive, cancellationToken);

			if(findEmployeeActive != null)
			{
                var cardStatus = await _cardRepository.GetByIdAsync(findEmployeeActive.Id);
				cardStatus.Status = false;

                return await _cardRepository.UpdateAsync(cardStatus);
            }

			return false;
        }
    }
}

