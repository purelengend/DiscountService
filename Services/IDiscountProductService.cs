using DiscountAPI.Models;

namespace DiscountAPI.Services;

public interface IDiscountProductService
{
  public Task<Guid> GetDiscountOfProduct(string productId);
#nullable enable
  public IEnumerable<string> GetProductsOfDiscount(Guid? discountId);

  public Task<DiscountProduct?> GetSingleDiscountProduct(DiscountProduct discountProduct);

  public Task<IEnumerable<DiscountProduct>> GetDiscountProducts();
  public Task<DiscountProduct> AddDiscountProduct(DiscountProduct discountProduct);
  public string[] AddMultipleDiscountProduct(Guid discountId, string[] listProductId);
  public Task<DiscountProduct> UpdateDiscountProduct(DiscountProduct discountProduct);

  public void RemoveAllDiscountProduct();
  public void RemoveSingleDiscountProduct(DiscountProduct discountProduct);
}