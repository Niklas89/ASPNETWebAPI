using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpendingController : ControllerBase
    {
        private readonly DataContext _context;

        public SpendingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SpendingDto>>> Get(int userId)
        {
            var spendings = await _context.Spendings
                .Where(s => s.UserId == userId)
                // circular error, because each spendingType has a spending and each spending has a spendingType...
                // So I added [JsonIgnore] in SpendingType class
                .Include(s => s.SpendingType)
                .Include(s => s.User)
                .ToListAsync();

            List<SpendingDto> spendingDtos = new List<SpendingDto>();

            foreach(var spending in spendings)
            {
                spendingDtos.Add(new SpendingDto {
                    Id = spending.Id,
                    UserId= spending.UserId,
                    UserFullName = spending.User.FirstName + spending.User.LastName,
                    Date = spending.Date,
                    SpendingTypeId= spending.SpendingTypeId,
                    SpendingTypeName = spending.SpendingType.Name,
                    Amount = spending.Amount,
                    CurrencyName = spending.User.Currency.IsoCode,
                    Comment = spending.Comment
                });
            }

            return spendingDtos;
        }
    }
}
