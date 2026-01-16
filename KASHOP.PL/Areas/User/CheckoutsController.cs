using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]

        public async Task<IActionResult> Payment([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _checkoutService.ProcessPaymentAsync(userId, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
