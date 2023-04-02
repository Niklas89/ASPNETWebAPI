using ASPNETWebAPI.Dto;
using ASPNETWebAPI.Models;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ASPNETWebAPI.Services.SpendingService
{
    public class SpendingService : ISpendingService
    {
        private readonly DataContext _context;

        public SpendingService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<SpendingDto>>> GetSpendings(int userId, string orderBy)
        {

            var spendings = await _context.Spendings
                .Where(s => s.UserId == userId)
                // circular error, because each spendingType has a spending and each spending has a spendingType...
                // So I added [JsonIgnore] in SpendingType class
                .Include(s => s.SpendingType)
                .Include(s => s.User)
                .Include(s => s.User.Currency)
                .ToListAsync();

            List<SpendingDto> spendingDtos = new List<SpendingDto>();

            foreach (var spending in spendings)
            {
                spendingDtos.Add(new SpendingDto
                {
                    Id = spending.Id,
                    UserId = spending.UserId,
                    UserFullName = spending.User.FirstName + " " + spending.User.LastName,
                    Date = spending.Date,
                    SpendingTypeId = spending.SpendingTypeId,
                    SpendingTypeName = spending.SpendingType.Name,
                    Amount = spending.Amount,
                    CurrencyName = spending.User.Currency.IsoCode,
                    Comment = spending.Comment
                });
            }

            List<SpendingDto> orderedSpendingDtos = spendingDtos;

            if (orderBy != null)
            {
                if (orderBy.Equals("/api/Spending/date")) // order from the most recent date to the latest date
                    orderedSpendingDtos = spendingDtos.OrderByDescending(s => s.Date).ToList();

                if (orderBy.Equals("/api/Spending/amount")) // order from the most expensive to the cheapest amount
                    orderedSpendingDtos = spendingDtos.OrderByDescending(s => s.Amount).ToList();
            }

            return new ServiceResponse<List<SpendingDto>>
            {
                Data = orderedSpendingDtos
            };
        }

        public async Task<ServiceResponse<Spending>> CreateSpending(SpendingDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new ServiceResponse<Spending>
                {
                    Success = false,
                    Data = null,
                    Message = "User not found."
                };
            }

            var spendingType = await _context.SpendingTypes.FindAsync(request.SpendingTypeId);
            if (spendingType == null)
            {
                return new ServiceResponse<Spending>
                {
                    Success = false,
                    Data = null,
                    Message = "SpendingType not found."
                };
            }

            if (request.Date > DateTime.Now)
            {
                return new ServiceResponse<Spending>
                {
                    Success = false,
                    Data = null,
                    Message = "The date cannot be superior to the current date."
                };
            }

            if (request.Date < DateTime.Now.AddMonths(-3))
            {
                return new ServiceResponse<Spending>
                {
                    Success = false,
                    Data = null,
                    Message = "The date cannot be set to before 3 months ago. It must be between three months ago and today."
                };
            }

            if (request.Comment.IsNullOrEmpty() || request.Comment.Equals(" "))
            {
                return new ServiceResponse<Spending>
                {
                    Success = false,
                    Data = null,
                    Message = "The comment is mandatory."
                };
            }

            var currency = await _context.Currencies.FindAsync(user.CurrencyId);
            if (!currency.IsoCode.Equals(request.CurrencyName))
            {
                return new ServiceResponse<Spending>
                {
                    Success = false,
                    Data = null,
                    Message = "The currency must be the same as the user's currency."
                };
            }

            var spendings = await _context.Spendings.Where(s => s.UserId == request.UserId
            && s.Date == request.Date && s.Amount == request.Amount).FirstOrDefaultAsync();
            if (spendings != null)
            {
                return new ServiceResponse<Spending>
                {
                    Success = false,
                    Data = null,
                    Message = "There is already a spending with the same date and the same amount for this user."
                };
            }

            var newSpending = new Spending
            {
                Date = request.Date,
                Amount = request.Amount,
                Comment = request.Comment,
                UserId = request.UserId,
                SpendingTypeId = request.SpendingTypeId
            };

            _context.Spendings.Add(newSpending);
            await _context.SaveChangesAsync();


            return new ServiceResponse<Spending> { Data = newSpending };
        }
    }
}
