using ASPNETWebAPI.Dto;
using ASPNETWebAPI.Models;

namespace ASPNETWebAPI.Services.SpendingService
{
    public interface ISpendingService
    {
        Task<ServiceResponse<List<SpendingDto>>> GetSpendings(int userId, string orderBy);
        Task<ServiceResponse<Spending>> CreateSpending(SpendingDto request);
    }
}
