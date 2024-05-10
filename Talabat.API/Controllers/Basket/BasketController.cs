using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs.BasketDTO;
using Talabat.API.Errors;
using Talabat.Core.Interfaces;
using Talabat.Core.Models.Basket;

namespace Talabat.API.Controllers.Basket
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepo basketRepo , IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        // Get Or ReCreate Basket
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var Basket = await _basketRepo.GetBasketAsync(id);
            return Basket is null ? new CustomerBasket(id) : Basket;
        }
        // Update Or Create New Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDTO basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(basket);
            var UpdateOrCreateBasket = await _basketRepo.UpdateBasketAsync(MappedBasket);
            if (UpdateOrCreateBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(UpdateOrCreateBasket);
        }
        // Delete Basket
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepo.DeleteBasketAsync(id);
        }
    }
}
