namespace DiscountAPI.Models;
public class DiscountProduct
{
  public Guid discountId { get; set; }
  public Discount discount { get; set; }
  public string productId { get; set; }
}