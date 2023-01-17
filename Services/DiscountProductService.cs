using DiscountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountAPI.Services;

public class DiscountProductService : IDiscountProductService
{
  private readonly AppDbContext _context;

  public DiscountProductService(AppDbContext context)
  {
    _context = context;
  }
  public async Task<DiscountProduct> AddDiscountProduct(DiscountProduct discountProduct)
  {
    await _context.DiscountProducts.AddAsync(discountProduct);
    await _context.SaveChangesAsync();

    return discountProduct;
  }

  public string[] AddMultipleDiscountProduct(string discountId, string[] listProductId)
  {
    var checkListProductId = GetProductsOfDiscount(null);

    var uniqueListProductId = listProductId.Except(checkListProductId).ToList();

    foreach (var item in uniqueListProductId)
    {
      Console.Write(item + " ");
       _context.DiscountProducts.Add(new DiscountProduct { discountId = discountId, productId = item });
    }
     _context.SaveChanges();
    return listProductId;
  }

  public async Task<string> GetDiscountOfProduct(string productId)
  {
    var discountOfProduct = await _context.DiscountProducts.AsNoTracking().FirstOrDefaultAsync(d => d.productId == productId);
    if (discountOfProduct == null) return null;
    return discountOfProduct.discountId;
  }

  public async Task<IEnumerable<DiscountProduct>> GetDiscountProducts()
  {
    var listDiscountProduct = await _context.DiscountProducts.AsNoTracking().ToListAsync();

    return listDiscountProduct;
  }

#nullable enable
  public IEnumerable<string> GetProductsOfDiscount(string? discountId)
  {
    var productsOfDiscount = _context.DiscountProducts
                                    .AsNoTracking()
                                    .Where(p => discountId == null || p.discountId == discountId)
                                    .Select(p => p.productId)
                                    .ToList();

    return productsOfDiscount;
  }

  public async Task<DiscountProduct> GetSingleDiscountProduct(DiscountProduct discountProduct)
  {
    var discountProductInDb = await _context.DiscountProducts
      .AsNoTracking()
      .FirstOrDefaultAsync(p => p.discountId == discountProduct.discountId && p.productId == discountProduct.productId);

    return discountProductInDb;
  }


  public async Task<DiscountProduct> UpdateDiscountProduct(DiscountProduct discountProduct)
  {
    var updatedDiscountProduct = await Task.Run(() => UpdateDiscountProductInDb(discountProduct));

    return updatedDiscountProduct;
  }

  private DiscountProduct UpdateDiscountProductInDb(DiscountProduct discountProduct)
  {
    _context.DiscountProducts.Update(discountProduct);
    _context.SaveChanges();
    return discountProduct;
  }
  public void RemoveAllDiscountProduct()
  {
    _context.DiscountProducts.RemoveRange(_context.DiscountProducts);
    _context.SaveChanges();
  }

  public void RemoveSingleDiscountProduct(DiscountProduct discountProduct)
  {
    _context.DiscountProducts.Remove(discountProduct);
    _context.SaveChanges();
  }


}
