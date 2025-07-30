

namespace EasyBuy.Services.SIMPLECHAT
{
    public class SimpleChatService
    {
        private readonly LearningService _learningService;

        public SimpleChatService(LearningService learningService)
        {
            _learningService = learningService;
        }

        public async Task<string> ChatAsync(string message)
        {
            await Task.Delay(1000);
            
            // Kiểm tra xem có phải là lệnh học hoặc tìm kiếm thông tin đã học không
            var learnedResponse = await _learningService.ProcessMessageAsync(message);
            if (!string.IsNullOrEmpty(learnedResponse))
            {
                return learnedResponse;
            }
            
            var lowerMessage = message.ToLower();

            if (lowerMessage.Contains("xin chào") || lowerMessage.Contains("hello") ||lowerMessage.Contains("xin chao") ||lowerMessage.Contains("Xin chao") ||lowerMessage.Contains("Xin chào"))
            {
                return "Xin chào! Tôi là AI Assistant của EasyBuy, rất vui được gặp bạn!";
            }
            else
            {
                return "Cảm ơn bạn đã hỏi! Tôi đang học và phát triển để có thể trả lời tốt hơn. Bạn có thể hỏi tôi về bất cứ chủ đề nào khác!";
            }
            
        }
    }
} 