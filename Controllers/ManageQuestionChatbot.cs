using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models;
using feedBackMvc.Helpers;
using System;
using Microsoft.Extensions.Logging;

public class ManagerQuestionChatbotController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly ILogger<ManagerQuestionChatbotController> _logger;


    public ManagerQuestionChatbotController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<ManagerQuestionChatbotController> logger)
    {
        _appDbContext = appDbContext;
        _jwtTokenHelper = jwtTokenHelper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuestionChatbot()
    {
        try
        {
            // Lấy tất cả dữ liệu từ bảng FallbackChatbot
            var chatbotQuestions = await _appDbContext.FallbackChatbot.ToListAsync();
            return PartialView("_QL_CauHoiChatbot", chatbotQuestions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy dữ liệu FallbackChatbot");
            return StatusCode(500, "Có lỗi xảy ra khi lấy dữ liệu");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateIntentChatbot([FromBody] List<FallbackChatbot> updatedQuestions)
    {
        try
        {
            foreach (var question in updatedQuestions)
            {
                var entity = await _appDbContext.FallbackChatbot.FindAsync(question.id);
                if (entity != null)
                {
                    entity.Intent = question.Intent; // Cập nhật intent
                    _appDbContext.FallbackChatbot.Update(entity);
                }
            }

            await _appDbContext.SaveChangesAsync();
            return StatusCode(200, "Cập nhật intent thành công");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật intent");
            return StatusCode(500, "Lỗi trong quá trình cập nhật");
        }
    }


    //[HttpPost]
    //public async Task<IActionResult> UpdateIntentChatbot()
    //{
    //    // Return the partial view and pass the data
    //    return PartialView("_QL_MauKhaoSatXoa");
    //}
}
