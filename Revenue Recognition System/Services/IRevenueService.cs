using Revenue_Recognition_System.DTOs;

namespace Revenue_Recognition_System.Services;

public interface IRevenueService
{
    public Task<RevenueResponceModel> CalculateRevenue(RevenueRequestModel revenueRequestModel);
    public Task<RevenueResponceModel> CalculateFutureRevenue(RevenueRequestModel revenueRequestModel);
}