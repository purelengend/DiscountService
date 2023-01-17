namespace DiscountAPI.DTO;
public class DiscountDTO
{
  public string id { get; set; }
  public string discountName { get; set; }
  public DateTime startDate { get; set; }
  public DateTime endDate { get; set; }
  // public string discountType { get; set; }
  public string[] listProductId { get; set; }
  public float discountValue { get; set; }

 
}