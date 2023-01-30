namespace DiscountAPI.DTO;
public class DiscountDTO
{
  public Guid id { get; set; }
  public string discountName { get; set; }
  public DateTime startDate { get; set; }
  public DateTime endDate { get; set; }
  public Guid[] listProductId { get; set; }
  public float discountValue { get; set; }

 
}