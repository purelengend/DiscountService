namespace DiscountAPI.Services;

public interface IDiscountBackgroundService
{
  public void StartTime(Guid discountId, DateTime startDate, string timerId);
  public void EndTime(Guid discountId, DateTime endDate, string timerId);

}