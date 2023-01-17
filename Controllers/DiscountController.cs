using AutoMapper;
using DiscountAPI.DTO;
using DiscountAPI.Models;
using DiscountAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiscountAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscountController : ControllerBase
{
  private readonly IDiscountService _discountService;
  private readonly IDiscountProductService _discountProductService;
  private readonly IMapper _mapper;
  private readonly IServiceProvider _serviceProvider;
  private readonly IDiscountBackgroundService _discountBackgroundService;

  public DiscountController(
    IDiscountService discountService,
    IDiscountProductService discountProductService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IDiscountBackgroundService discountBackgroundService
    )
  {
    _discountService = discountService;
    _discountProductService = discountProductService;
    _mapper = mapper;
    _serviceProvider = serviceProvider;
    _discountBackgroundService = discountBackgroundService;
  }

  [HttpGet]
  public ActionResult<IEnumerable<DiscountDTO>> GetDiscounts()
  {
    var list = _discountService.GetDiscounts().Result;
    if (list == null) return NotFound();

    return Ok(_mapper.Map<IEnumerable<DiscountDTO>>(list));
  }

  [HttpGet("{id}")]
  public ActionResult<IEnumerable<Discount>> GetDiscount(string id)
  {
    var discount = _discountService.GetDiscount(id).Result;
    if (discount == null) return NotFound();

    var discountDTO = _mapper.Map<DiscountDTO>(discount);
    return Ok(discountDTO);
  }


  [HttpPost]
  public ActionResult CreateDiscount([FromBody] DiscountDTO discountDTO)
  {
    var discount = _mapper.Map<Discount>(discountDTO);
    discount.timerId = Guid.NewGuid().ToString(); ;
    var addedDiscount = _discountService.AddDiscount(discount).Result;

    if (addedDiscount != null)
      _discountProductService.AddMultipleDiscountProduct(discount.discountId, discount.listProductId);

    _discountBackgroundService.StartTime(addedDiscount.discountId, addedDiscount.startDate, addedDiscount.timerId);
    _discountBackgroundService.EndTime(addedDiscount.discountId, addedDiscount.endDate, addedDiscount.timerId);

    return Ok(_mapper.Map<DiscountDTO>(addedDiscount));
  }

  [HttpPut]
  public ActionResult UpdateDiscount(DiscountDTO discountDTO)
  {
    var discount = _mapper.Map<Discount>(discountDTO);
    discount.timerId = Guid.NewGuid().ToString();
    var updatedDiscount = _discountService.UpdateDiscount(discount).Result;

    if (updatedDiscount != null)
      _discountProductService.AddMultipleDiscountProduct(discount.discountId, discount.listProductId);
      
    _discountBackgroundService.StartTime(updatedDiscount.discountId, updatedDiscount.startDate, updatedDiscount.timerId);
    _discountBackgroundService.EndTime(updatedDiscount.discountId, updatedDiscount.endDate, updatedDiscount.timerId);

    return Ok(_mapper.Map<DiscountDTO>(updatedDiscount));
  }

  [HttpDelete("{discountId}")]
  public ActionResult DeleteDiscount(string discountId)
  {
    var discountToDelete = _discountService.GetDiscount(discountId).Result;

    if (discountToDelete == null) return NotFound();

    _discountService.RemoveDiscount(discountToDelete);

    return Ok("Delete succeeded");
  }
}
