using DiscountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountAPI.Services;

public class DiscountService : IDiscountService
{
  private readonly AppDbContext _context;

  public DiscountService(AppDbContext context)
  {
    _context = context;
  }
  public async Task<Discount> AddDiscount(Discount discount)
  {
    var addedDiscount = await _context.Discounts.AddAsync(discount);
    await _context.SaveChangesAsync();

    var entry = _context.Entry(discount);
    discount.discountId = entry.Property("discountId").CurrentValue.ToString();
    return discount;
  }

  public async Task<Discount> GetDiscount(string id)
  {
    var discount = await _context.Discounts.FindAsync(id);
    return discount;
  }

  public async Task<IEnumerable<Discount>> GetDiscounts()
  {
    var listDiscounts = await _context.Discounts.AsNoTracking().ToListAsync();
    return listDiscounts;
  }

  public void RemoveDiscount(Discount discount)
  {
    var listProductsOfDiscount = _context.DiscountProducts.Where(p => p.discountId == discount.discountId);
    _context.DiscountProducts.RemoveRange(listProductsOfDiscount);
    _context.Discounts.Remove(discount);
    _context.SaveChanges();
  }

  public async Task<Discount> UpdateDiscount(Discount discount)
  {
    var updatedDiscount = await Task.Run(() => UpdateDiscountInDB(discount));

    return updatedDiscount;
  }

  private Discount UpdateDiscountInDB(Discount discount)
  {
    _context.Discounts.Update(discount);
    _context.SaveChanges();
    return discount;
  }
  // private bool RemoveDiscountInDB(Discount discount)
  // {
  //   _context.Remove(discount);
  //   return (_context.SaveChanges() > 0);
  // }
}   
