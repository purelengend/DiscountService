using DiscountAPI.Models;

namespace DiscountAPI.Services;
 
public interface IDiscountService
{
  public  Task<IEnumerable<Discount>> GetDiscounts();
  public  Task<Discount> GetDiscount(string id);
 
  public Task<Discount> AddDiscount(Discount discount);
  public Task<Discount> UpdateDiscount(Discount discount);
  public void RemoveDiscount(Discount discount);
}
