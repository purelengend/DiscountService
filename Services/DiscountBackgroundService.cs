using DiscountAPI.Models;
namespace DiscountAPI.Services;

public class DiscountBackgroundService : IDiscountBackgroundService
{
  private readonly IMessageProducer _messageProducer;
  private Timer _startTimer;
  private Timer _endTimer;

  public DiscountBackgroundService(IMessageProducer messageProducer)
  {
    _messageProducer = messageProducer;
  }

  public void EndTime(Guid discountId, DateTime endDate, string timerId)
  {
    Console.WriteLine("EndTime Task initiated.");
    if (endDate < DateTime.Now)
    {
      Console.WriteLine("End Date is in the past. BackgroundTask will not running");
    }
    else
    {
      var saleTime = endDate - DateTime.Now;
      _endTimer = new Timer(EndDiscount, (discountId, timerId), saleTime, TimeSpan.Zero);
    }
  }

  public void StartTime(Guid discountId, DateTime startDate, string timerId)
  {
    Console.WriteLine("StartTime Task initiated.");
    if (startDate < DateTime.Now)
    {
      Console.WriteLine("Start Date is in the past. BackgroundTask will not running");
    }
    else
    {
      var saleTime = startDate - DateTime.Now;
      _startTimer = new Timer(StartDiscount, (discountId, timerId), saleTime, TimeSpan.Zero);
    }
  }


  private void StartDiscount(object state)
  {
    var (discountId, timerId) = (ValueTuple<Guid, string>)state;

    List<Guid> listDiscountProduct;
    double discountValue;
    Event sendingMessage = new Event();

    // Use the DbContext to modify the database here
    using (var _context = new AppDbContext())
    {
      var checkDiscountExist = _context.Discounts.Find(discountId);

      if (checkDiscountExist == null)
      {
        Console.WriteLine("Discount not found.");
        return;
      }
      var checkTimerId = checkDiscountExist.timerId;
      if (checkTimerId != timerId)
      {
        Console.WriteLine("StartDiscount with timer id: {0} was cancelled.", timerId);
        return;
      }
      Console.WriteLine("StartDiscount is working with id {0}.", discountId);

      listDiscountProduct = _context.DiscountProducts.Where(p => p.discountId == discountId).Select(p => p.productId).ToList();
      discountValue = _context.Discounts.Find(discountId).discountValue;
      _context.SaveChanges();
    }
    Console.WriteLine("StartDiscount done.", discountId);
    try
    {
      sendingMessage.eventName = "Start Discount";
      sendingMessage.discountId = discountId;
      sendingMessage.data = listDiscountProduct;
      sendingMessage.value = discountValue;
      _messageProducer.SendingMessage(sendingMessage);

    }
    catch (System.Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }


  private void EndDiscount(object state)
  {
    var (discountId, timerId) = (ValueTuple<Guid, string>)state;
    List<Guid> listDiscountProduct;
    double discountValue;
    Event sendingMessage = new Event();
    // Use the DbContext to modify the database here
    using (var _context = new AppDbContext())
    {
      var checkDiscountExist = _context.Discounts.Find(discountId);

      if (checkDiscountExist == null)
      {
        Console.WriteLine("Discount not found.");
        return;
      }
      var checkTimerId = checkDiscountExist.timerId;
      if (checkTimerId != timerId)
      {
        Console.WriteLine("EndDiscount with timer id: {0} was cancelled.", timerId);
        return;
      }
      Console.WriteLine("EndDiscount is working with id {0}.", discountId);
      listDiscountProduct = _context.DiscountProducts.Where(p => p.discountId == discountId).Select(p => p.productId).ToList();
      discountValue = _context.Discounts.Find(discountId).discountValue;
      var listProductsOfDiscount = _context.DiscountProducts.Where(p => p.discountId == discountId);
      _context.DiscountProducts.RemoveRange(listProductsOfDiscount);
      _context.SaveChanges();
    }
    Console.WriteLine("EndDiscount done.", discountId);
    try
    {
      sendingMessage.eventName = "End Discount";
      sendingMessage.discountId = discountId;
      sendingMessage.data = listDiscountProduct;
      sendingMessage.value = Math.Round(discountValue, 1);
      _messageProducer.SendingMessage(sendingMessage);

    }
    catch (System.Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }


}