using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ASPNETWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpendingController : ControllerBase
    {
        private readonly ISpendingService _spendingService;

        public SpendingController(ISpendingService spendingService)
        {
            _spendingService = spendingService;
        }


        /// <summary>
        /// Get a list of spendings ordered or unordered associated with specified user ID.
        /// <list type="bullet">
        /// <item><description><para><em>To return a list ordered by date: the route parameter should be "date" : /api/Spending/date</em></para></description></item>
        /// <item><description><para><em>To return a list ordered by amount: the route parameter should be "amount" : /api/Spending/amount</em></para></description></item>
        /// <item><description><para><em>To return an unordered list (ordered by spending id), the route should be : /api/Spending/</em></para></description></item>
        /// </list>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><strong>A list of spendings ordered by date, amount or unordered associated with specified user ID.</strong></returns>
        [HttpGet]
        [HttpGet("{order}")]
        public async Task<ActionResult<List<SpendingDto>>> Get(int userId)
        {
            string orderBy = string.Empty;

            if (Request?.Path.Value != null) 
                orderBy = Request.Path.Value;

            var result = await _spendingService.GetSpendings(userId, orderBy);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);

        }


        /// <summary>
        /// Create a new spending
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The new list of spendings</returns>
        [HttpPost]
        public async Task<ActionResult<Spending>> Create(SpendingDto request)
        {

            var result = await _spendingService.CreateSpending(request);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            //return await Get(result.Data.UserId);
            return Ok(result.Data);
        }
    }
}
