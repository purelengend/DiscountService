namespace DiscountAPI.Models;

public class Event
{
  public string eventName { get; set; }
  public string discountId { get; set; }
  public object data { get; set; }
  public double value { get; set; }
}