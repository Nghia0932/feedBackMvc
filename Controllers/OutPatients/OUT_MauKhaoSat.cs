using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models;
using Microsoft.Extensions.Logging;
using feedBackMvc.Helpers;
using Npgsql;
using System;

namespace feedBackMvc.Controllers.OutPatients
{
    public class OUT_MauKhaoSatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly ILogger<OUT_MauKhaoSatController> _logger;

        public OUT_MauKhaoSatController(AppDbContext context, JwtTokenHelper jwtTokenHelper, ILogger<OUT_MauKhaoSatController> logger)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
            _logger = logger;

        }

        public async Task<IActionResult> Show_OUT_MauKhaoSat()
        {
            try
            { var nhomCauHoiKhaoSats = await _context.OUT_NhomCauHoiKhaoSat
                    .Include(n => n.CauHoiKhaoSats) // Include related entities
                    .ToListAsync();
                return PartialView("_Show_OUT_MauKhaoSat",nhomCauHoiKhaoSats);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while retrieving OUT_NhomCauHoiKhaoSat data.");

                // Handle the error (return an error view, etc.)
                return StatusCode(500, "Internal server error");
            }
        }
        public class ThemMauKhaoSat_Request
        {
            public string? TenMauKhaoSat { get; set; }
            public DateTime? NgayBatDau { get; set; }
            public DateTime? NgayKetThuc { get; set; }
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

            var existingRecord = await _context.OUT_MauKhaoSat
                .Where(x => x.TenMauKhaoSat == request.TenMauKhaoSat)
                .FirstOrDefaultAsync();
            if (existingRecord != null)
            {
                return Json(new { success = false, message = "Tên mẫu khảo sát đã tồn tại" });
            }
            var mauKhaoSat = new OUT_MauKhaoSat
            {
                TenMauKhaoSat = request.TenMauKhaoSat,
                NgayTao = DateTime.UtcNow, // Ngày hiện tại theo giờ UTC
                NhomCauHoiKhaoSat = request.NhomCauHoi?.ToArray(),
                CauHoiKhaoSat = request.TieuDeCauHoi?.ToArray(),
                idAdmin = adminId,
                NgayBatDau = request.NgayBatDau,
                NgayKetThuc = request.NgayKetThuc,
                SoluongKhaoSat = request.SoLuongDanhGia
            };
            try
            {
                _context.OUT_MauKhaoSat.Add(mauKhaoSat);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Thêm mẫu khảo sát người bệnh nội trú thành công" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            
        }

    }
}
