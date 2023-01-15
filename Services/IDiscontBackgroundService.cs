namespace DiscountAPI.Services;

public interface IDiscountBackgroundService
{
  public void StartTime(string discountId, DateTime startDate, string timerId);
  public void EndTime(string discountId, DateTime endDate, string timerId);

}