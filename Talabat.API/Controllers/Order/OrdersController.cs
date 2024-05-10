using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.API.DTOs.AccountsDTO;
using Talabat.API.DTOs.OrderDTO;
using Talabat.API.Errors;
using Talabat.Core.Interfaces.InterfaceServices;
using Talabat.Core.Interfaces.InterfaceUnitOfWork;
using Talabat.Core.Models.Order;
using Talabat.Service.OrderService;
using order = Talabat.Core.Models.Order.Order;

namespace Talabat.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // Create Order
        [Authorize]
        [HttpPost("CreateOrder")]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDTO, OrderAddress>(orderDto.ShippingAddress);
            var orders = await _orderService.CreateOrder(Email, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if (orders is null) return BadRequest(new ApiResponse(400, "There's a Problem Occure in Your Order"));

            return Ok(orders);
        }
        // Get Order For Specific User 
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrderForSpecificUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrderForSpecificUser(Email);
            if (Orders is null) return NotFound(new ApiResponse(404, "There's No Order For This User"));
            var MappedOrdered = _mapper.Map<IReadOnlyList<order>, IReadOnlyList<OrderToReturnDTO>>(Orders);
            return Ok(MappedOrdered);
        }
        // Get Order by ID For Specific User
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrderByIdForSpecificUser(int Id)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdforSpecificUser(Email, Id);
            if (order is null) return NotFound(new ApiResponse(404, $" Ther's No Order Matching This id {Id}"));
            var MappedOrder = _mapper.Map<order, OrderToReturnDTO>(order);
            return Ok(MappedOrder);
        }
        // Get All Delivery Methods
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            if (DeliveryMethods is null) return NotFound(new ApiResponse(404, "No Delivery Methods Found"));
            return Ok(DeliveryMethods);
        }

    }
}
