namespace DiscountAPI.Models;
 
public class Discount
{
  public string discountId { get; set; }
  public string discountName { get; set; }
  public DateTime startDate { get; set; }
  public DateTime endDate { get; set; }
  public string discountType { get; set; }
  public float discountValue { get; set; }
  public string timerId { get; set; }
  public string[] listProductId { get; set; }
  public virtual List<DiscountProduct> discountProducts { get; set; }
}
