using EasyBuy.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Services.SIMPLECHAT
{
    public class LearningService
    {
        private readonly EasyBuyContext _context;

        public LearningService(EasyBuyContext context)
        {
            _context = context;
        }

        // Học thông tin mới
        public async Task<bool> LearnInformationAsync(string keyword, string information)
        {
            try
            {
                // Kiểm tra xem keyword đã tồn tại chưa
                var existing = await _context.LearnedInfos
                    .FirstOrDefaultAsync(x => x.Keyword.ToLower() == keyword.ToLower());

                if (existing != null)
                {
                    // Cập nhật thông tin nếu đã tồn tại
                    existing.Information = information;
                    existing.UpdatedAt = DateTime.Now;
                }
                else
                {
                    // Thêm mới nếu chưa tồn tại
                    var learnedInfo = new LearnedInfo
                    {
                        Keyword = keyword,
                        Information = information,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.LearnedInfos.Add(learnedInfo);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi học thông tin: {ex.Message}");
                return false;
            }
        }

        // Tìm kiếm thông tin theo keyword
        public async Task<string?> FindInformationAsync(string keyword)
        {
            try
            {
                var learnedInfo = await _context.LearnedInfos
                    .FirstOrDefaultAsync(x => x.Keyword.ToLower().Contains(keyword.ToLower()) ||
                                            keyword.ToLower().Contains(x.Keyword.ToLower()));

                return learnedInfo?.Information;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tìm kiếm thông tin: {ex.Message}");
                return null;
            }
        }

        // Xử lý tin nhắn để học hoặc tìm kiếm
        public async Task<string> ProcessMessageAsync(string message)
        {
            var lowerMessage = message.ToLower();

            // Kiểm tra xem có phải là lệnh học không
            if (lowerMessage.StartsWith("học "))
            {
                // Tách thông tin học
                var learningContent = message.Substring(3).Trim(); // Bỏ "học "
                
                // Kiểm tra xem có dấu ngoặc kép không
                if (learningContent.Contains('"'))
                {
                    // Tìm từ khóa trong dấu ngoặc kép
                    var firstQuoteIndex = learningContent.IndexOf('"');
                    var lastQuoteIndex = learningContent.LastIndexOf('"');
                    
                    if (firstQuoteIndex != -1 && lastQuoteIndex != -1 && firstQuoteIndex != lastQuoteIndex)
                    {
                        var keyword = learningContent.Substring(firstQuoteIndex + 1, lastQuoteIndex - firstQuoteIndex - 1);
                        var information = learningContent.Substring(lastQuoteIndex + 1).Trim();
                        
                        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(information))
                        {
                            var success = await LearnInformationAsync(keyword, information);
                            if (success)
                            {
                                return $"Đã học thành công! Từ khóa: '{keyword}', Thông tin: '{information}'";
                            }
                            else
                            {
                                return "Có lỗi xảy ra khi học thông tin. Vui lòng thử lại!";
                            }
                        }
                        else
                        {
                            return "Cú pháp không đúng. Vui lòng sử dụng: 'học \"[từ khóa]\" [thông tin]'";
                        }
                    }
                    else
                    {
                        return "Cú pháp không đúng. Vui lòng sử dụng: 'học \"[từ khóa]\" [thông tin]'";
                    }
                }
                else
                {
                    // Tìm từ khóa và thông tin (cách cũ)
                    var parts = learningContent.Split(' ', 2);
                    if (parts.Length >= 2)
                    {
                        var keyword = parts[0];
                        var information = parts[1];

                        var success = await LearnInformationAsync(keyword, information);
                        if (success)
                        {
                            return $"Đã học thành công! Từ khóa: '{keyword}', Thông tin: '{information}'";
                        }
                        else
                        {
                            return "Có lỗi xảy ra khi học thông tin. Vui lòng thử lại!";
                        }
                    }
                    else
                    {
                        return "Cú pháp không đúng. Vui lòng sử dụng: 'học [từ khóa] [thông tin]' hoặc 'học \"[từ khóa]\" [thông tin]'";
                    }
                }
            }

            // Tìm kiếm thông tin đã học
            var foundInfo = await FindInformationAsync(lowerMessage);
            if (!string.IsNullOrEmpty(foundInfo))
            {
                return foundInfo;
            }

            return null; // Không tìm thấy thông tin
        }
    }
} 