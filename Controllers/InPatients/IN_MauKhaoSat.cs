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
            { var nhomCauHoiKhaoSats = await _context.IN_NhomCauHoiKhaoSat
                    .Include(n => n.CauHoiKhaoSats) // Include related entities
                    .ToListAsync();
                return PartialView("_Show_In_MauKhaoSat",nhomCauHoiKhaoSats);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while retrieving IN_NhomCauHoiKhaoSat data.");

                // Handle the error (return an error view, etc.)
                return StatusCode(500, "Internal server error");
            }
        }
        //[HttpPost]
        //public IActionResult _IN_MauKhaoSatNoiTru(string[] NhomCauHoi, string[] TieuDeCauHoi)
        //{
        //    // Lấy tất cả câu hỏi từ cơ sở dữ liệu dựa trên tiêu đề câu hỏi
        //    var allCauHoi = _context.IN_CauHoiKhaoSat
        //        .Where(c => TieuDeCauHoi.Contains(c.TieuDeCauHoi)) // Lọc câu hỏi theo tiêu đề
        //        .ToList(); // Lấy danh sách câu hỏi

        //    // Tạo mảng câu hỏi tương ứng với tiêu đề câu hỏi
        //    var cauHoiList = TieuDeCauHoi
        //        .Select(tieuDe => allCauHoi
        //            .FirstOrDefault(c => c.TieuDeCauHoi == tieuDe)?.CauHoi ?? "") // Tìm câu hỏi theo tiêu đề hoặc trả về chuỗi rỗng nếu không tìm thấy
        //        .ToArray(); // Chuyển danh sách câu hỏi thành mảng

        //    var viewModel = new KhaoSatNoiTruViewModel
        //    {
        //        IN_ThongTinChung = _context.IN_ThongTinChung.FirstOrDefault(),
        //        IN_ThongTinNguoiBenh = _context.IN_ThongTinNguoiBenh.FirstOrDefault(),
        //        IN_ThongTinYKienKhac = _context.IN_ThongTinYKienKhac.FirstOrDefault(),
        //        NhomCauHoi = NhomCauHoi,
        //        TieuDeCauHoi = TieuDeCauHoi,
        //        CauHoi = cauHoiList // Gán mảng câu hỏi vào viewModel
        //    };
        //    return PartialView("_IN_MauKhaoSatNoiTru", viewModel);
        //}
    }
}
