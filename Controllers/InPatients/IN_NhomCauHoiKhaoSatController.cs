using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models;
using Microsoft.Extensions.Logging;
using feedBackMvc.Helpers;
using Npgsql;
using System;

namespace feedBackMvc.Controllers.InPatients
{
    public class In_NhomCauHoiKhaoSatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly ILogger<In_NhomCauHoiKhaoSatController> _logger;

        public In_NhomCauHoiKhaoSatController(AppDbContext context, JwtTokenHelper jwtTokenHelper, ILogger<In_NhomCauHoiKhaoSatController> logger)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
            _logger = logger;

        }

        public async Task<IActionResult> Show_In_NhomCauHoiKhaoSat()
        {
            try
            {
                // Retrieve data from the database
                var nhomCauHoiKhaoSats = await _context.IN_NhomCauHoiKhaoSat
                    .Include(n => n.CauHoiKhaoSats) // Include related entities
                    .ToListAsync();
                // Log information  
                _logger.LogInformation("Successfully retrieved IN_NhomCauHoiKhaoSat data.");
                return PartialView("_Show_In_NhomCauHoiKhaoSat", nhomCauHoiKhaoSats);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while retrieving IN_NhomCauHoiKhaoSat data.");

                // Handle the error (return an error view, etc.)
                return StatusCode(500, "Internal server error");
            }
        }
        public class TitleAndContentList
        {
            public List<string>? TieuDes { get; set; }
            public List<string>? NoiDungs { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> ThemNhomCauHoiKhaoSat([FromBody] TitleAndContentList data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token) || !_jwtTokenHelper.TryParseToken(token, out var adminId))
            {
                return BadRequest("Invalid token or admin ID.");
            }

            if (data.TieuDes?.Count != data.NoiDungs?.Count)
            {
                return BadRequest("Mismatched number of titles and contents.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                for (int i = 0; i < data.TieuDes?.Count; i++)
                {
                    var newGroup = new IN_NhomCauHoiKhaoSat
                    {
                        TieuDe = data.TieuDes[i],
                        NoiDung = data.NoiDungs?[i],
                        idAdmin = adminId
                    };

                    var existingRecord = await _context.IN_NhomCauHoiKhaoSat
                        .Where(x => x.TieuDe == newGroup.TieuDe)
                        .FirstOrDefaultAsync();

                    if (existingRecord != null)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = "Tiêu đề đã tồn tại" });
                        //return Json(new { success = false });
                    }
                    _context.IN_NhomCauHoiKhaoSat.Add(newGroup);
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
        public async Task<IActionResult> XoaNhomCauHoiKhaoSat([FromBody] DeleteRequest request)
        {
            try
            {
                var nhom = await _context.IN_NhomCauHoiKhaoSat.FindAsync(request.Id);
                if (nhom == null)
                {
                    return Json(new { success = false, message = "Nhóm câu hỏi không tồn tại." });
                }
                bool isUsedInInSurvey = await _context.IN_MauKhaoSat.AnyAsync(m => m.NhomCauHoiKhaoSat.Contains(nhom.TieuDe));
                if (isUsedInInSurvey)
                {
                    return Json(new { success = false, message = "Nhóm câu hỏi này đang được sử dụng trong mẫu khảo sát." });
                }

                _context.IN_NhomCauHoiKhaoSat.Remove(nhom);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ và ghi log nếu cần
                return Json(new { success = true });
            }
        }
        public class UpdateRequest
        {
            public int Id { get; set; }
            public string? TieuDe { get; set; }
            public string? NoiDung { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatNhomCauHoiKhaoSat([FromBody] List<UpdateRequest> request)
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
                    var existingItem = await _context.IN_NhomCauHoiKhaoSat.FindAsync(item.Id);
                    if (existingItem != null)
                    {
                        existingItem.TieuDe = item.TieuDe;
                        existingItem.NoiDung = item.NoiDung;
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
