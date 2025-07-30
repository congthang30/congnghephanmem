using Microsoft.AspNetCore.Mvc;
using EasyBuy.Services.SIMPLECHAT;

namespace EasyBuy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatApiController : ControllerBase
    {
        private readonly SimpleChatService _chatService;

        public ChatApiController(SimpleChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.UserMessage))
                {
                    return BadRequest(new { error = "Tin nhắn không được để trống" });
                }

                var reply = await _chatService.ChatAsync(req.UserMessage);

                return Ok(new { 
                    candidates = new[] { 
                        new { 
                            content = new { 
                                parts = new[] { 
                                    new { text = reply } 
                                } 
                            } 
                        } 
                    } 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Có lỗi xảy ra khi gọi API", details = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string UserMessage { get; set; }
    }
} 