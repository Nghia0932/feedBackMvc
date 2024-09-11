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
    public class In_MauKhaoSatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly ILogger<In_MauKhaoSatController> _logger;

        public In_MauKhaoSatController(AppDbContext context, JwtTokenHelper jwtTokenHelper, ILogger<In_MauKhaoSatController> logger)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
            _logger = logger;

        }

        public async Task<IActionResult> Show_In_MauKhaoSat()
        {
            try
            {
                var nhomCauHoiKhaoSats = await _context.IN_NhomCauHoiKhaoSat
                    .Include(n => n.CauHoiKhaoSats) // Include related entities
                    .ToListAsync();
                return PartialView("_Show_In_MauKhaoSat", nhomCauHoiKhaoSats);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while retrieving IN_NhomCauHoiKhaoSat data.");

                // Handle the error (return an error view, etc.)
                return StatusCode(500, "Internal server error");
            }
        }
        public class ThemMauKhaoSat_Request
        {
            public string? TenMauKhaoSat { get; set; }
            //public DateOnly? NgayBatDau { get; set; }
            //public DateOnly? NgayKetThuc { get; set; }
            public int? SoLuongDanhGia { get; set; }
            public List<string>? NhomCauHoi { get; set; }
            public List<string>? TieuDeCauHoi { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> ThemMauKhaoSat([FromBody] ThemMauKhaoSat_Request request)
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

            var existingRecord = await _context.IN_MauKhaoSat
                .Where(x => x.TenMauKhaoSat == request.TenMauKhaoSat)
                .FirstOrDefaultAsync();
            if (existingRecord != null)
            {
                return Json(new { success = false, message = "Tên mẫu khảo sát đã tồn tại" });
            }
            var mauKhaoSat = new IN_MauKhaoSat
            {
                TenMauKhaoSat = request.TenMauKhaoSat,
                NgayTao = DateOnly.FromDateTime(DateTime.UtcNow), // Ngày hiện tại theo giờ UTC
                NhomCauHoiKhaoSat = request.NhomCauHoi?.ToArray(),
                CauHoiKhaoSat = request.TieuDeCauHoi?.ToArray(),
                idAdmin = adminId,
                //NgayBatDau = request.NgayBatDau,
                //NgayKetThuc = request.NgayKetThuc,
                SoluongKhaoSat = request.SoLuongDanhGia
            };
            try
            {
                _context.IN_MauKhaoSat.Add(mauKhaoSat);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm mẫu khảo sát người bệnh nội trú thành công" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        public class DeleteRequest
        {
            public int Id { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Xoa_IN_MauKhaoSat([FromBody] DeleteRequest request)
        {
            try
            {
                var mauKhaoSat = await _context.IN_MauKhaoSat.FindAsync(request.Id);

                if (mauKhaoSat == null)
                {
                    return NotFound(new { success = false, message = "Mẫu khảo sát không tồn tại." });
                }

                _context.IN_MauKhaoSat.Remove(mauKhaoSat);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Mẫu khảo sát đã được xóa thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xóa mẫu khảo sát.", error = ex.Message });
            }
        }
        public class UpdateRequest
        {
            public int Id { get; set; }
            public string? TenMauKhaoSat { get; set; }
            //public DateOnly? NgayKetThuc { get; set; }
            public int? SoLuongDanhGia { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> CapNhat_IN_MauKhaoSat([FromBody] UpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { success = false, message = "Không nhận được dữ liệu cập nhật." });
            }
            try
            {
                var mauKhaoSat = await _context.IN_MauKhaoSat.FindAsync(request.Id);

                if (mauKhaoSat == null)
                {
                    return NotFound(new { success = false, message = "Mẫu khảo sát không tồn tại." });
                }
                else
                {
                    mauKhaoSat.TenMauKhaoSat = request.TenMauKhaoSat;
                    //mauKhaoSat.NgayKetThuc = request.NgayKetThuc;
                    mauKhaoSat.SoluongKhaoSat = request.SoLuongDanhGia ?? 0;
                }
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                // Trả về phản hồi thành công
                return Ok(new { success = true, message = "Cập nhật mẫu khảo sát thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xóa mẫu khảo sát.", error = ex.Message });
            }
        }


    }
}
