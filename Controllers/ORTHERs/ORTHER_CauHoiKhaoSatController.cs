using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models;
using Microsoft.Extensions.Logging;
using feedBackMvc.Helpers;
using Npgsql;
using System;

namespace feedBackMvc.Controllers.ORTHERs
{
    public class ORTHER_CauHoiKhaoSatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly ILogger<ORTHER_CauHoiKhaoSatController> _logger;

        public ORTHER_CauHoiKhaoSatController(AppDbContext context, JwtTokenHelper jwtTokenHelper, ILogger<ORTHER_CauHoiKhaoSatController> logger)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
            _logger = logger;

        }

        public IActionResult Show_ORTHER_CauHoiKhaoSat()
        {
            try
            {
                // Retrieve data from the database
                var cauHoiKhaoSats = _context.ORTHER_CauHoiKhaoSat;
                // Log information  
                _logger.LogInformation("Successfully retrieved ORTHER_CauHoiKhaoSat data.");
                return PartialView("_Show_ORTHER_CauHoiKhaoSat", cauHoiKhaoSats);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while retrieving ORTHER_CauHoiKhaoSat data.");

                // Handle the error (return an error view, etc.)
                return StatusCode(500, "Internal server error");
            }
        }
        public class TitleAndQuestionList
        {
            public List<string>? TieuDeCauHois { get; set; }
            public List<string>? CauHois { get; set; }
            public int Id { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> ThemCauHoiKhaoSat([FromBody] TitleAndQuestionList data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (data.TieuDeCauHois?.Count != data.CauHois?.Count)
            {
                return BadRequest("Mismatched number of titles and contents.");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                for (int i = 0; i < data.TieuDeCauHois?.Count; i++)
                {
                    var newGroup = new ORTHER_CauHoiKhaoSat
                    {
                        TieuDeCauHoi = data.TieuDeCauHois[i],
                        CauHoi = data.CauHois?[i],
                        IdORTHER_NhomCauHoiKhaoSat = data.Id,
                    };
                    var existingRecord = await _context.ORTHER_CauHoiKhaoSat
                        .Where(x => x.TieuDeCauHoi == newGroup.TieuDeCauHoi)
                        .FirstOrDefaultAsync();
                    if (existingRecord != null)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = "Tiêu đề đã tồn tại" });
                        //return Json(new { success = false });
                    }
                    _context.ORTHER_CauHoiKhaoSat.Add(newGroup);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while adding records.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
                //return Json(new { success = false });
            }
        }
        public class DeleteRequest
        {
            public int Id { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> XoaCauHoiKhaoSat([FromBody] DeleteRequest request)
        {
            try
            {
                var nhom = await _context.ORTHER_CauHoiKhaoSat.FindAsync(request.Id);
                if (nhom == null)
                {
                    return Json(new { success = false, message = "Nhóm câu hỏi không tồn tại." });
                }
                //bool isUsedInInSurvey = await _context.IN_MauKhaoSat.AnyAsync(m => m.CauHoiKhaoSat.Contains(nhom.TieuDeCauHoi));
                bool isUsedInInSurvey = await _context.ORTHER_MauKhaoSat.AnyAsync(m => m.CauHoiKhaoSat != null && nhom.TieuDeCauHoi != null && m.CauHoiKhaoSat.Contains(nhom.TieuDeCauHoi));
                if (isUsedInInSurvey)
                {
                    return Json(new { success = false, message = "Câu hỏi này đang được sử dụng trong mẫu khảo sát." });
                }

                _context.ORTHER_CauHoiKhaoSat.Remove(nhom);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và ghi log nếu cần
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
        public class UpdateRequest
        {
            public int Id { get; set; }
            public string? TieuDeCauHoi { get; set; }
            public string? CauHoi { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatCauHoiKhaoSat([FromBody] List<UpdateRequest> request)
        {
            if (request == null || !request.Any())
            {
                return BadRequest(new { success = false, message = "Không nhận được dữ liệu cập nhật." });
            }
            try
            {
                foreach (var item in request)
                {
                    // Giả sử bạn có một DbContext tên là _context
                    var existingItem = await _context.ORTHER_CauHoiKhaoSat.FindAsync(item.Id);
                    if (existingItem != null)
                    {
                        existingItem.TieuDeCauHoi = item.TieuDeCauHoi;
                        existingItem.CauHoi = item.CauHoi;
                        // Bạn có thể thêm các logic kiểm tra khác ở đây
                    }
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật thành công." });
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và ghi log nếu cần
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
