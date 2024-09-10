using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using feedBackMvc.Models;
using Microsoft.EntityFrameworkCore;
using feedBackMvc.Helpers;

namespace feedBackMvc.Controllers;

public class DanhGiaController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenHelper _jwtTokenHelper;

    private readonly ILogger<DanhGiaController> _logger;

    public DanhGiaController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<DanhGiaController> logger)
    {
        _appDbContext = appDbContext;
        _jwtTokenHelper = jwtTokenHelper;
        _logger = logger;
    }
    public class IdRequest
    {
        public int Id { get; set; }
    }
    [HttpPost]
    [Route("DanhGia/IN_DanhGiaKhaoSat")]
    public async Task<IActionResult> IN_DanhGiaKhaoSat(int Id)
    {

        var mauKhaoSat = await _appDbContext.IN_MauKhaoSat.FindAsync(Id);
        if (mauKhaoSat == null)
        {
            return NotFound("Không tìm thấy mẫu khảo sát với ID đã cung cấp.");
        }

        var nhomCauHoiArray = mauKhaoSat.NhomCauHoiKhaoSat; // Assume it's a string array
        var cauHoiArray = mauKhaoSat.CauHoiKhaoSat;         // Assume it's a string array
        var viewModel = new KhaoSatNoiTruViewModel
        {
            Id = Id
        };
        // Bước 3: Lấy tất cả các nhóm câu hỏi và câu hỏi một lần từ cơ sở dữ liệu
        var nhomCauHoiResults = await _appDbContext.IN_NhomCauHoiKhaoSat
            .Where(n => nhomCauHoiArray.Contains(n.TieuDe))
            .ToListAsync();

        var cauHoiResults = await _appDbContext.IN_CauHoiKhaoSat
            .Where(ch => cauHoiArray.Contains(ch.TieuDeCauHoi))
            .ToListAsync();
        _logger.LogInformation("================================================================================");
        _logger.LogInformation("Fetched survey with Id: {Id}, NhomCauHoiKhaoSatlayDuoc: {NhomCauHoiKhaoSat}, CauHoiKhaoSat: {CauHoiKhaoSat}",
          Id, string.Join(",", nhomCauHoiResults), string.Join(",", cauHoiResults.Count));
        _logger.LogInformation("===============================================================================");

        foreach (var nhom in nhomCauHoiResults)
        {
            viewModel.NhomCauHoi.Add($"{nhom.TieuDe}: {nhom.NoiDung}");
        }
        var groupedItems = new Dictionary<int, KhaoSatNoiTruViewModel.QuestionGroup>();
        foreach (var nhom in nhomCauHoiResults)
        {
            var questionGroup = new KhaoSatNoiTruViewModel.QuestionGroup
            {
                TieuDeCauHoi = new List<string>(),
                CauHoi = new List<string>()
            };
            var relatedQuestions = cauHoiResults.Where(c => c.IdIN_NhomCauHoiKhaoSat == nhom.IdIN_NhomCauHoiKhaoSat);

            foreach (var cauHoi in relatedQuestions)
            {
                questionGroup.TieuDeCauHoi.Add(cauHoi.TieuDeCauHoi);
                questionGroup.CauHoi.Add(cauHoi.CauHoi);
            }
            // Thêm nhóm câu hỏi vào viewModel
            viewModel.CauHoi.Add(questionGroup);
        }
        // Bước 6: Trả view với dữ liệu đã xử lý
        return View(viewModel);
    }
    public class CreateIN_DanhGiaKhaoSat
    {
        public string? tenBenhVien { get; set; }
        public DateOnly? ngayDienPhieu { get; set; }
        public string? tenKhoa { get; set; }
        public string? nguoiTraLoi { get; set; }
        public string? gioiTinh { get; set; }
        public int? tuoi { get; set; }
        public string? soDienThoai { get; set; }
        public int? soNgayNamVien { get; set; }
        public string? suDungBHYT { get; set; }
        public int IdIN_MauKhaoSat { get; set; }
        public int? phanTramDanhGia { get; set; }
        public string? quayLaiText { get; set; }
        public string? yKienKhac { get; set; }
        public int[]? danhGia { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> Them_IN_DanhGiaKhaoSat([FromBody] CreateIN_DanhGiaKhaoSat data)
    {
        if (data == null)
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }
        try
        {
            // Tìm IdIN_ThongTinNguoiBenh lớn nhất
            var maxId = await _appDbContext.IN_ThongTinNguoiBenh
                .OrderByDescending(x => x.IdIN_ThongTinNguoiBenh)
                .Select(x => x.IdIN_ThongTinNguoiBenh)
                .FirstOrDefaultAsync();

            var newId = maxId + 1;

            // Xác định giá trị của CosuDungBHYT
            bool cosuDungBHYT = data.suDungBHYT != null && data.suDungBHYT.Trim().ToLower() == "co";

            // Thêm dữ liệu vào bảng IN_ThongTinNguoiBenh
            var thongTinNguoiBenh = new IN_ThongTinNguoiBenh
            {
                IdIN_ThongTinNguoiBenh = newId,
                GioiTinh = data.gioiTinh,
                Tuoi = data.tuoi,
                SoDienThoai = data.soDienThoai,
                SoNgayNamVien = data.soNgayNamVien,
                CoSuDungBHYT = cosuDungBHYT
            };

            _appDbContext.IN_ThongTinNguoiBenh.Add(thongTinNguoiBenh);
            await _appDbContext.SaveChangesAsync();

            // Thêm dữ liệu vào bảng IN_ThongTinChung
            var thongTinChung = new IN_ThongTinChung
            {
                TenBenhVien = data.tenBenhVien,
                NgayDienPhieu = DateOnly.FromDateTime(DateTime.UtcNow),
                NguoiTraLoi = data.nguoiTraLoi,
                TenKhoa = data.tenKhoa,
                IdIN_ThongTinNguoiBenh = newId,
                MaKhoa = "",

            };
            _appDbContext.IN_ThongTinChung.Add(thongTinChung);
            await _appDbContext.SaveChangesAsync();

            var thongTinyKienKhac = new IN_ThongTinYKienKhac
            {
                PhanTramMongDoi = data.phanTramDanhGia,
                QuayLaiVaGioiThieu = data.quayLaiText,
                YKienKhac = data.yKienKhac,
                NgayTao = DateOnly.FromDateTime(DateTime.UtcNow),
                IdIN_ThongTinNguoiBenh = newId,

            };
            _appDbContext.IN_ThongTinYKienKhac.Add(thongTinyKienKhac);
            await _appDbContext.SaveChangesAsync();

            var danhGia = new IN_DanhGia
            {
                DanhGia = data.danhGia,
                IdIN_MauKhaoSat = data.IdIN_MauKhaoSat,
                NgayDanhGia = DateOnly.FromDateTime(DateTime.UtcNow),
                IdIN_ThongTinNguoiBenh = newId,

            };
            _appDbContext.IN_DanhGia.Add(danhGia);
            await _appDbContext.SaveChangesAsync();

            return Json(new { success = true, message = "OK" });
        }
        catch (Exception ex)
        {
            // Xử lý lỗi
            return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
        }
    }



}