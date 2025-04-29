using Microsoft.AspNetCore.Mvc;
using PortfolioTrackerApi.Models;
using PortfolioTrackerApi.Services;
using System;
using System.Threading.Tasks;

namespace PortfolioTrackerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IEmailVerificationService _emailVerificationService;

        public EmailVerificationController(IEmailVerificationService emailVerificationService)
        {
            _emailVerificationService = emailVerificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendVerificationCode([FromBody] EmailRequest request)
        {
            try
            {
                await _emailVerificationService.SendVerificationCode(request.Email);
                return Ok($"Verification code sent to {request.Email}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }

        [HttpPost("verify")]
        public IActionResult VerifyCode([FromBody] VerificationRequest request)
        {
            var result = _emailVerificationService.VerifyCode(request.Email, request.Code);
            if (result)
                return Ok("Email verified successfully.");
            else
                return BadRequest("Invalid verification code or email.");
        }
    }
}
