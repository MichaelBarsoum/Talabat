using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.API.DTOs.BasketDTO;
using Talabat.API.Errors;
using Talabat.Core.Interfaces.InterfaceServices;
using Talabat.Core.Models.Basket;

namespace Talabat.API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_42a7624183a2fdaf60f690317edf52e30e9fbb0f75ab0d98c65f944debb3d4f0";

        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var CustomerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "There's a Problem in Your Basket"));
            var MappedBusket = _mapper.Map<CustomerBasket, CustomerBasketDTO>(CustomerBasket);
            return Ok(MappedBusket);
        }



        [HttpPost("webhook")] // post => BaseUrl/api/Payment/webhook
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var PaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                    await _paymentService.UpdatePaymentIntentSucceedOrFailed(PaymentIntent.Id, false);
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                    await _paymentService.UpdatePaymentIntentSucceedOrFailed(PaymentIntent.Id, true);
                // ... handle other event types
                else
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
