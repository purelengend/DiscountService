using AutoMapper;
using DiscountAPI.DTO;
using DiscountAPI.Models;
using DiscountAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiscountAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscountProductController : ControllerBase
{
  private readonly IDiscountProductService _discountProductService;
  private readonly IDiscountService _discountService;
  private readonly IMessageProducer _messageProducer;
  private readonly IMapper _mapper;

  public DiscountProductController(
  IDiscountProductService discountProductService,
  IDiscountService discountService,
  IMessageProducer messageProducer,
  IMapper mapper)
  {
    _discountProductService = discountProductService;
    _mapper = mapper;
    _discountService = discountService;
    _messageProducer = messageProducer;
  }

  [HttpGet]
  public ActionResult GetDiscountProducts()
  {
    var listDiscountProduct = _discountProductService.GetDiscountProducts().Result;

    if (listDiscountProduct == null) return NotFound();
    return Ok(_mapper.Map<IEnumerable<DiscountProductDTO>>(listDiscountProduct));
  }

  [HttpGet("product/{productId}")]
  public ActionResult GetDiscounOfProduct(string productId)
  {
    var discountOfProduct = _discountProductService.GetDiscountOfProduct(productId).Result;

    if (discountOfProduct == null) return NotFound();
    return Ok(discountOfProduct);
  }

  [HttpGet("discount/{discountId}")]
  public ActionResult GetProductsOfDiscount(string discountId)
  {
    var productsOfDiscount = _discountProductService.GetProductsOfDiscount(discountId);

    if (productsOfDiscount == null) return NotFound();
    return Ok(productsOfDiscount);
  }
  [HttpPost]
  public ActionResult CreateDiscountProduct(DiscountProductDTO discountProductDTO)
  {
    var discountProduct = _mapper.Map<DiscountProduct>(discountProductDTO);

    var checkDiscountExist = _discountService.GetDiscount(discountProduct.discountId).Result;
    var checkDiscountOfProductExist = _discountProductService.GetDiscountOfProduct(discountProduct.productId).Result;

    if (checkDiscountExist == null) return NotFound("DiscountId not found");
    if (checkDiscountOfProductExist != null) return BadRequest("Product already had discount");

    var addedDiscountProduct = _discountProductService.AddDiscountProduct(discountProduct).Result;

    var discount = _discountService.GetDiscount(discountProduct.discountId).Result;
    
    if (discount.startDate < DateTime.Now && discount.endDate > DateTime.Now)
      _messageProducer.SendingMessage(new Event()
      {
        eventName = "Add a Discount Product",
        discountId = discount.discountId,
        data = new List<string> { addedDiscountProduct.discountId },
        value = discount.discountValue
      });

    return Ok(_mapper.Map<DiscountProductDTO>(addedDiscountProduct));
  }


  //Update DiscountId Of a Product
  [HttpPut]
  public ActionResult UpdateDiscountProduct(DiscountProductDTO discountProductDTO)
  {
    var discountProduct = _mapper.Map<DiscountProduct>(discountProductDTO);

    var checkDiscountExist = _discountService.GetDiscount(discountProduct.discountId).Result;
    var checkDiscountOfProductExist = _discountProductService.GetDiscountOfProduct(discountProduct.productId).Result;

    if (checkDiscountExist == null) return NotFound("DiscountId not found");

    var updatedDiscountProduct = _discountProductService.UpdateDiscountProduct(discountProduct).Result;

    var discount = _discountService.GetDiscount(discountProduct.discountId).Result;

    if (discount.startDate < DateTime.Now && discount.endDate > DateTime.Now)
      _messageProducer.SendingMessage(new Event()
      {
        eventName = "Update a Discount Product",
        discountId = discount.discountId,
        data = new List<string> { updatedDiscountProduct.discountId },
        value = discount.discountValue
      });
    return Ok(_mapper.Map<DiscountProductDTO>(updatedDiscountProduct));
  }
  [HttpDelete]
  public ActionResult DeleteSingleDiscountProduct(DiscountProductDTO discountProductDTO)
  {
    var discountProduct = _mapper.Map<DiscountProduct>(discountProductDTO);

    var checkDiscountProductExist = _discountProductService.GetSingleDiscountProduct(discountProduct).Result;

    if (checkDiscountProductExist == null) return NotFound("DiscountProduct not found");

    _discountProductService.RemoveSingleDiscountProduct(discountProduct);

    return Ok("Delete succeeded");
  }

  [HttpDelete("all")]
  public ActionResult DeleteAllDiscountProducts()
  {

    _discountProductService.RemoveAllDiscountProduct();

    return Ok("Delete succeeded");
  }

}