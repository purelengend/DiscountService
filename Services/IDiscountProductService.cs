using DiscountAPI.Models;

namespace DiscountAPI.Services;
 
public interface IDiscountProductService
{
  public Task<Guid> GetDiscountOfProduct(Guid productId);
  #nullable enable
  public IEnumerable<Guid> GetProductsOfDiscount(Guid? discountId);

  public Task<DiscountProduct?> GetSingleDiscountProduct(DiscountProduct discountProduct);

  public  Task<IEnumerable<DiscountProduct>> GetDiscountProducts();
  public Task<DiscountProduct> AddDiscountProduct(DiscountProduct discountProduct);
  public Guid[] AddMultipleDiscountProduct(Guid discountId, Guid[] listProductId);
  public Task<DiscountProduct> UpdateDiscountProduct(DiscountProduct discountProduct);

  public void RemoveAllDiscountProduct();
  public void RemoveSingleDiscountProduct(DiscountProduct discountProduct);
}