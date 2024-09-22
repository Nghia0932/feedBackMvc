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
                if (!string.IsNullOrEmpty(cauHoi.TieuDeCauHoi))
                {
                    questionGroup.TieuDeCauHoi.Add(cauHoi.TieuDeCauHoi);
                }

                if (!string.IsNullOrEmpty(cauHoi.CauHoi))
                {
                    questionGroup.CauHoi.Add(cauHoi.CauHoi);
                }

            }
            // Thêm nhóm câu hỏi vào viewModel
            viewModel.CauHoi.Add(questionGroup);
        }
        if (mauKhaoSat.MucDanhGia != null)
        {
            viewModel.MucDanhGia.AddRange(mauKhaoSat.MucDanhGia);
        }
        if (mauKhaoSat.MucQuanTrong != null)
        {
            viewModel.MucQuanTrong.AddRange(mauKhaoSat.MucQuanTrong);
        }
        // Lấy mảng MucQuanTrong một lần duy nhất
        var mucQuanTrongArray = mauKhaoSat.MucQuanTrong;

        // Tính Max_PhanTramMongDoi
        double maxPhanTramMongDoi = 0;

        // Duyệt qua từng nhóm câu hỏi, nhân số câu hỏi với mức quan trọng tương ứng
        for (int i = 0; i < nhomCauHoiResults.Count; i++)
        {
            var nhom = nhomCauHoiResults[i];

            // Lấy danh sách các câu hỏi liên quan đến nhóm này
            var relatedQuestions = cauHoiResults
                .Where(c => c.IdIN_NhomCauHoiKhaoSat == nhom.IdIN_NhomCauHoiKhaoSat)
                .ToList();

            int soLuongCauHoi = relatedQuestions.Count;

            // Lấy mức quan trọng thứ i từ mảng MucQuanTrong
            double mucQuanTrong = mucQuanTrongArray[i];

            // Tính tổng điểm mong đợi cho nhóm này
            maxPhanTramMongDoi += soLuongCauHoi * mucQuanTrong;
        }

        viewModel.Max_PhanTramMongDoi = maxPhanTramMongDoi;

        // Bước 6: Trả view với dữ liệu đã xử lý
        return View(viewModel);
    }
    public class OutRequest
    {
        public int Id { get; set; }
    }
    [HttpPost]
    [Route("DanhGia/OUT_DanhGiaKhaoSat")]
    public async Task<IActionResult> OUT_DanhGiaKhaoSat(int Id)
    {

        var mauKhaoSat = await _appDbContext.OUT_MauKhaoSat.FindAsync(Id);
        if (mauKhaoSat == null)
        {
            return NotFound("Không tìm thấy mẫu khảo sát với ID đã cung cấp.");
        }

        var nhomCauHoiArray = mauKhaoSat.NhomCauHoiKhaoSat; // Assume it's a string array
        var cauHoiArray = mauKhaoSat.CauHoiKhaoSat;         // Assume it's a string array
        var viewModel = new KhaoSatNgoaiTruViewModel
        {
            Id = Id
        };
        // Bước 3: Lấy tất cả các nhóm câu hỏi và câu hỏi một lần từ cơ sở dữ liệu
        var nhomCauHoiResults = await _appDbContext.OUT_NhomCauHoiKhaoSat
            .Where(n => nhomCauHoiArray.Contains(n.TieuDe))
            .ToListAsync();

        var cauHoiResults = await _appDbContext.OUT_CauHoiKhaoSat
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
        var groupedItems = new Dictionary<int, KhaoSatNgoaiTruViewModel.QuestionGroup>();
        foreach (var nhom in nhomCauHoiResults)
        {
            var questionGroup = new KhaoSatNgoaiTruViewModel.QuestionGroup
            {
                TieuDeCauHoi = new List<string>(),
                CauHoi = new List<string>()
            };
            var relatedQuestions = cauHoiResults.Where(c => c.IdOUT_NhomCauHoiKhaoSat == nhom.IdOUT_NhomCauHoiKhaoSat);

            foreach (var cauHoi in relatedQuestions)
            {
                questionGroup.TieuDeCauHoi.Add(cauHoi.TieuDeCauHoi);
                questionGroup.CauHoi.Add(cauHoi.CauHoi);
            }
            if (mauKhaoSat.MucDanhGia != null)
            {
                viewModel.MucDanhGia.AddRange(mauKhaoSat.MucDanhGia);
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
        public double[]? danhGiaTong { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> Them_IN_DanhGiaKhaoSat([FromBody] CreateIN_DanhGiaKhaoSat data)
    {
        if (data == null)
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }
        var nguoiBenhList = await _appDbContext.IN_ThongTinNguoiBenh
            .Where(x => x.SoDienThoai == data.soDienThoai)
            .ToListAsync();
        if (nguoiBenhList == null || !nguoiBenhList.Any())
        {

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
                    DanhGiaTong = data.danhGiaTong,

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
        else
        {
            var idBenhNhanList = nguoiBenhList.Select(x => x.IdIN_ThongTinNguoiBenh).ToList();

            // Tìm tất cả bản ghi đánh giá tương ứng với các bệnh nhân trong danh sách
            var danhGiaList = await _appDbContext.IN_DanhGia
                .Where(x => idBenhNhanList.Contains(x.IdIN_ThongTinNguoiBenh) && x.IdIN_MauKhaoSat == data.IdIN_MauKhaoSat)
                .ToListAsync();


            var ngayDanhGiaLonNhat = danhGiaList
                .Max(x => x.NgayDanhGia);

            if (ngayDanhGiaLonNhat != null && ngayDanhGiaLonNhat == DateOnly.FromDateTime(DateTime.UtcNow))
            {
                // Tìm đánh giá có ngày đánh giá lớn nhất
                var danhGiaToUpdate = danhGiaList
                    .FirstOrDefault(x => x.NgayDanhGia == ngayDanhGiaLonNhat);

                if (danhGiaToUpdate != null)
                {
                    var idThongTinNguoiBenh = danhGiaToUpdate.IdIN_ThongTinNguoiBenh;
                    var idDanhGia = danhGiaToUpdate.IdIN_DanhGia;

                    var thongTinNguoiBenhToUpdate = nguoiBenhList
                        .FirstOrDefault(x => x.IdIN_ThongTinNguoiBenh == idThongTinNguoiBenh);

                    if (thongTinNguoiBenhToUpdate != null)
                    {
                        // Cập nhật thông tin người bệnh
                        thongTinNguoiBenhToUpdate.GioiTinh = data.gioiTinh;
                        thongTinNguoiBenhToUpdate.Tuoi = data.tuoi;
                        thongTinNguoiBenhToUpdate.SoDienThoai = data.soDienThoai;
                        thongTinNguoiBenhToUpdate.SoNgayNamVien = data.soNgayNamVien;
                        thongTinNguoiBenhToUpdate.CoSuDungBHYT = data.suDungBHYT?.Trim().ToLower() == "co";

                        _appDbContext.IN_ThongTinNguoiBenh.Update(thongTinNguoiBenhToUpdate);
                        await _appDbContext.SaveChangesAsync();

                        // Cập nhật thông tin chung
                        var thongTinChungToUpdate = await _appDbContext.IN_ThongTinChung
                            .FirstOrDefaultAsync(x => x.IdIN_ThongTinNguoiBenh == idThongTinNguoiBenh);

                        if (thongTinChungToUpdate != null)
                        {
                            thongTinChungToUpdate.TenBenhVien = data.tenBenhVien;
                            thongTinChungToUpdate.NguoiTraLoi = data.nguoiTraLoi;
                            thongTinChungToUpdate.TenKhoa = data.tenKhoa;

                            _appDbContext.IN_ThongTinChung.Update(thongTinChungToUpdate);
                            await _appDbContext.SaveChangesAsync();
                        }

                        // Cập nhật thông tin ý kiến khác
                        var thongTinYKienToUpdate = await _appDbContext.IN_ThongTinYKienKhac
                            .FirstOrDefaultAsync(x => x.IdIN_ThongTinNguoiBenh == idThongTinNguoiBenh);

                        if (thongTinYKienToUpdate != null)
                        {
                            thongTinYKienToUpdate.PhanTramMongDoi = data.phanTramDanhGia;
                            thongTinYKienToUpdate.QuayLaiVaGioiThieu = data.quayLaiText;
                            thongTinYKienToUpdate.YKienKhac = data.yKienKhac;
                            thongTinYKienToUpdate.NgayTao = DateOnly.FromDateTime(DateTime.UtcNow);

                            _appDbContext.IN_ThongTinYKienKhac.Update(thongTinYKienToUpdate);
                            await _appDbContext.SaveChangesAsync();
                        }
                        // Cập nhật đánh giá
                        danhGiaToUpdate.DanhGia = data.danhGia;
                        danhGiaToUpdate.DanhGiaTong = data.danhGiaTong;
                        danhGiaToUpdate.IdIN_MauKhaoSat = data.IdIN_MauKhaoSat;
                        danhGiaToUpdate.NgayDanhGia = DateOnly.FromDateTime(DateTime.UtcNow);
                        _appDbContext.IN_DanhGia.Update(danhGiaToUpdate);
                        await _appDbContext.SaveChangesAsync();
                        return Json(new { success = true, message = "Cập nhật thành công." });
                    }
                }
            }
            else
            {
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
        return Ok();
    }

    public class CreateOUT_DanhGiaKhaoSat
    {
        public string? tenBenhVien { get; set; }
        public string? gioiTinh { get; set; }
        public int? tuoi { get; set; }
        public string? soDienThoai { get; set; }
        public int? khoangCach { get; set; }
        public string? suDungBHYT { get; set; }
        public int IdOUT_MauKhaoSat { get; set; }
        public int? phanTramDanhGia { get; set; }
        public string? quayLaiText { get; set; }

        public int[]? danhGia { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> Them_OUT_DanhGiaKhaoSat([FromBody] CreateOUT_DanhGiaKhaoSat data)
    {
        if (data == null)
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }
        var nguoiBenhList = await _appDbContext.OUT_ThongTinNguoiBenh
            .Where(x => x.SoDienThoai == data.soDienThoai)
            .ToListAsync();
        if (nguoiBenhList == null || !nguoiBenhList.Any())
        {

            try
            {
                // Tìm IdIN_ThongTinNguoiBenh lớn nhất
                var maxId = await _appDbContext.OUT_ThongTinNguoiBenh
                    .OrderByDescending(x => x.IdOUT_ThongTinNguoiBenh)
                    .Select(x => x.IdOUT_ThongTinNguoiBenh)
                    .FirstOrDefaultAsync();

                var newId = maxId + 1;

                // Xác định giá trị của CosuDungBHYT
                bool cosuDungBHYT = data.suDungBHYT != null && data.suDungBHYT.Trim().ToLower() == "co";

                // Thêm dữ liệu vào bảng IN_ThongTinNguoiBenh
                var thongTinNguoiBenh = new OUT_ThongTinNguoiBenh
                {
                    IdOUT_ThongTinNguoiBenh = newId,
                    GioiTinh = data.gioiTinh,
                    Tuoi = data.tuoi,
                    SoDienThoai = data.soDienThoai,
                    KhoangCach = data.khoangCach,
                    CoSuDungBHYT = cosuDungBHYT
                };

                _appDbContext.OUT_ThongTinNguoiBenh.Add(thongTinNguoiBenh);
                await _appDbContext.SaveChangesAsync();

                // Thêm dữ liệu vào bảng IN_ThongTinChung
                var thongTinChung = new OUT_ThongTinChung
                {
                    TenBenhVien = data.tenBenhVien,
                    NgayDienPhieu = DateOnly.FromDateTime(DateTime.UtcNow),
                    IdOUT_ThongTinNguoiBenh = newId,
                    MaKhoa = "",

                };
                _appDbContext.OUT_ThongTinChung.Add(thongTinChung);
                await _appDbContext.SaveChangesAsync();

                var thongTinyKienKhac = new OUT_ThongTinYKienKhac
                {
                    PhanTramMongDoi = data.phanTramDanhGia,
                    QuayLaiVaGioiThieu = data.quayLaiText,
                    NgayTao = DateOnly.FromDateTime(DateTime.UtcNow),
                    IdOUT_ThongTinNguoiBenh = newId,

                };
                _appDbContext.OUT_ThongTinYKienKhac.Add(thongTinyKienKhac);
                await _appDbContext.SaveChangesAsync();

                var danhGia = new OUT_DanhGia
                {
                    DanhGia = data.danhGia,
                    IdOUT_MauKhaoSat = data.IdOUT_MauKhaoSat,
                    NgayDanhGia = DateOnly.FromDateTime(DateTime.UtcNow),
                    IdOUT_ThongTinNguoiBenh = newId,

                };
                _appDbContext.OUT_DanhGia.Add(danhGia);
                await _appDbContext.SaveChangesAsync();

                return Json(new { success = true, message = "OK" });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }

        }
        else
        {
            var idBenhNhanList = nguoiBenhList.Select(x => x.IdOUT_ThongTinNguoiBenh).ToList();

            // Tìm tất cả bản ghi đánh giá tương ứng với các bệnh nhân trong danh sách
            var danhGiaList = await _appDbContext.OUT_DanhGia
                .Where(x => idBenhNhanList.Contains(x.IdOUT_ThongTinNguoiBenh) && x.IdOUT_MauKhaoSat == data.IdOUT_MauKhaoSat)
                .ToListAsync();

            var ngayDanhGiaLonNhat = danhGiaList
                .Max(x => x.NgayDanhGia);

            if (ngayDanhGiaLonNhat != null && ngayDanhGiaLonNhat == DateOnly.FromDateTime(DateTime.UtcNow))
            {
                // Tìm đánh giá có ngày đánh giá lớn nhất
                var danhGiaToUpdate = danhGiaList
                    .FirstOrDefault(x => x.NgayDanhGia == ngayDanhGiaLonNhat);

                if (danhGiaToUpdate != null)
                {
                    var idThongTinNguoiBenh = danhGiaToUpdate.IdOUT_ThongTinNguoiBenh;
                    var idDanhGia = danhGiaToUpdate.IdOUT_DanhGia;

                    var thongTinNguoiBenhToUpdate = nguoiBenhList
                        .FirstOrDefault(x => x.IdOUT_ThongTinNguoiBenh == idThongTinNguoiBenh);

                    if (thongTinNguoiBenhToUpdate != null)
                    {
                        // Cập nhật thông tin người bệnh
                        thongTinNguoiBenhToUpdate.GioiTinh = data.gioiTinh;
                        thongTinNguoiBenhToUpdate.Tuoi = data.tuoi;
                        thongTinNguoiBenhToUpdate.SoDienThoai = data.soDienThoai;
                        thongTinNguoiBenhToUpdate.KhoangCach = data.khoangCach;
                        thongTinNguoiBenhToUpdate.CoSuDungBHYT = data.suDungBHYT?.Trim().ToLower() == "co";

                        _appDbContext.OUT_ThongTinNguoiBenh.Update(thongTinNguoiBenhToUpdate);
                        await _appDbContext.SaveChangesAsync();

                        // Cập nhật thông tin chung
                        var thongTinChungToUpdate = await _appDbContext.OUT_ThongTinChung
                            .FirstOrDefaultAsync(x => x.IdOUT_ThongTinNguoiBenh == idThongTinNguoiBenh);

                        if (thongTinChungToUpdate != null)
                        {
                            thongTinChungToUpdate.TenBenhVien = data.tenBenhVien;
                            _appDbContext.OUT_ThongTinChung.Update(thongTinChungToUpdate);
                            await _appDbContext.SaveChangesAsync();
                        }

                        // Cập nhật thông tin ý kiến khác
                        var thongTinYKienToUpdate = await _appDbContext.OUT_ThongTinYKienKhac
                            .FirstOrDefaultAsync(x => x.IdOUT_ThongTinNguoiBenh == idThongTinNguoiBenh);

                        if (thongTinYKienToUpdate != null)
                        {
                            thongTinYKienToUpdate.PhanTramMongDoi = data.phanTramDanhGia;
                            thongTinYKienToUpdate.QuayLaiVaGioiThieu = data.quayLaiText;
                            thongTinYKienToUpdate.NgayTao = DateOnly.FromDateTime(DateTime.UtcNow);

                            _appDbContext.OUT_ThongTinYKienKhac.Update(thongTinYKienToUpdate);
                            await _appDbContext.SaveChangesAsync();
                        }
                        // Cập nhật đánh giá
                        danhGiaToUpdate.DanhGia = data.danhGia;
                        danhGiaToUpdate.IdOUT_MauKhaoSat = data.IdOUT_MauKhaoSat;
                        danhGiaToUpdate.NgayDanhGia = DateOnly.FromDateTime(DateTime.UtcNow);
                        _appDbContext.OUT_DanhGia.Update(danhGiaToUpdate);
                        await _appDbContext.SaveChangesAsync();
                        return Json(new { success = true, message = "Cập nhật thành công." });
                    }
                }
            }
            else
            {
                try
                {
                    // Tìm IdIN_ThongTinNguoiBenh lớn nhất
                    var maxId = await _appDbContext.OUT_ThongTinNguoiBenh
                        .OrderByDescending(x => x.IdOUT_ThongTinNguoiBenh)
                        .Select(x => x.IdOUT_ThongTinNguoiBenh)
                        .FirstOrDefaultAsync();

                    var newId = maxId + 1;

                    // Xác định giá trị của CosuDungBHYT
                    bool cosuDungBHYT = data.suDungBHYT != null && data.suDungBHYT.Trim().ToLower() == "co";

                    // Thêm dữ liệu vào bảng IN_ThongTinNguoiBenh
                    var thongTinNguoiBenh = new OUT_ThongTinNguoiBenh
                    {
                        IdOUT_ThongTinNguoiBenh = newId,
                        GioiTinh = data.gioiTinh,
                        Tuoi = data.tuoi,
                        SoDienThoai = data.soDienThoai,
                        KhoangCach = data.khoangCach,
                        CoSuDungBHYT = cosuDungBHYT
                    };

                    _appDbContext.OUT_ThongTinNguoiBenh.Add(thongTinNguoiBenh);
                    await _appDbContext.SaveChangesAsync();

                    // Thêm dữ liệu vào bảng IN_ThongTinChung
                    var thongTinChung = new IN_ThongTinChung
                    {
                        TenBenhVien = data.tenBenhVien,
                        NgayDienPhieu = DateOnly.FromDateTime(DateTime.UtcNow),
                        IdIN_ThongTinNguoiBenh = newId,
                        MaKhoa = "",

                    };
                    _appDbContext.IN_ThongTinChung.Add(thongTinChung);
                    await _appDbContext.SaveChangesAsync();

                    var thongTinyKienKhac = new OUT_ThongTinYKienKhac
                    {
                        PhanTramMongDoi = data.phanTramDanhGia,
                        QuayLaiVaGioiThieu = data.quayLaiText,
                        NgayTao = DateOnly.FromDateTime(DateTime.UtcNow),
                        IdOUT_ThongTinNguoiBenh = newId,

                    };
                    _appDbContext.OUT_ThongTinYKienKhac.Add(thongTinyKienKhac);
                    await _appDbContext.SaveChangesAsync();

                    var danhGia = new OUT_DanhGia
                    {
                        DanhGia = data.danhGia,
                        IdOUT_MauKhaoSat = data.IdOUT_MauKhaoSat,
                        NgayDanhGia = DateOnly.FromDateTime(DateTime.UtcNow),
                        IdOUT_ThongTinNguoiBenh = newId,

                    };
                    _appDbContext.OUT_DanhGia.Add(danhGia);
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
        return Ok();
    }
    [HttpPost]
    public IActionResult IN_CheckAndRetrievePatientInfo(string phoneNumber, int id)
    {
        // Lấy tất cả IdIN_ThongTinNguoiBenh có SoDienThoai khớp với số điện thoại truyền vào
        var ids = _appDbContext.IN_ThongTinNguoiBenh
            .Where(p => p.SoDienThoai == phoneNumber)
            .Select(p => p.IdIN_ThongTinNguoiBenh)
            .ToList();

        if (ids.Count != 0)
        {
            // Tìm ngày đánh giá lớn nhất ứng với các IdIN_ThongTinNguoiBenh trong bảng IN_DanhGia
            var maxNgayDanhGia = _appDbContext.IN_DanhGia
                .Where(d => ids.Contains(d.IdIN_ThongTinNguoiBenh) && d.IdIN_MauKhaoSat == id)
                .OrderByDescending(d => d.NgayDanhGia)
                .Select(d => new { d.NgayDanhGia, d.IdIN_ThongTinNguoiBenh })
                .FirstOrDefault();

            if (maxNgayDanhGia != null)
            {
                // Lấy thông tin bệnh nhân từ bảng IN_ThongTinNguoiBenh
                var patientInfo = _appDbContext.IN_ThongTinNguoiBenh
                    .FirstOrDefault(p => p.IdIN_ThongTinNguoiBenh == maxNgayDanhGia.IdIN_ThongTinNguoiBenh);

                if (patientInfo != null)
                {
                    // Lấy thông tin chung từ bảng IN_ThongTinChung
                    var generalInfo = _appDbContext.IN_ThongTinChung
                        .FirstOrDefault(p => p.IdIN_ThongTinNguoiBenh == patientInfo.IdIN_ThongTinNguoiBenh);

                    // Lấy thông tin ý kiến khác từ bảng IN_ThongTinYKienKhac
                    var additionalInfo = _appDbContext.IN_ThongTinYKienKhac
                        .FirstOrDefault(p => p.IdIN_ThongTinNguoiBenh == patientInfo.IdIN_ThongTinNguoiBenh);

                    // Tạo đối tượng chứa tất cả thông tin cần thiết
                    var result = new
                    {
                        GioiTinh = patientInfo.GioiTinh,
                        Tuoi = patientInfo.Tuoi,
                        SoNgayNamVien = patientInfo.SoNgayNamVien,
                        CoSuDungBHYT = patientInfo.CoSuDungBHYT == true ? "Co" : "Khong",
                        TenBenhVien = generalInfo?.TenBenhVien,
                        NguoiTraLoi = generalInfo?.NguoiTraLoi,
                        TenKhoa = generalInfo?.TenKhoa,
                        PhanTramMongDoi = additionalInfo?.PhanTramMongDoi,
                        YKienKhac = additionalInfo?.YKienKhac
                    };

                    // Trả về dữ liệu dưới dạng JSON nếu có kết quả
                    return Json(result);
                }
            }
        }

        // Nếu không tìm thấy bất kỳ thông tin gì, không trả về phản hồi nào
        return new EmptyResult();
    }
    [HttpPost]
    public IActionResult OUT_CheckAndRetrievePatientInfo(string phoneNumber, int id)
    {
        // Lấy tất cả IdIN_ThongTinNguoiBenh có SoDienThoai khớp với số điện thoại truyền vào
        var ids = _appDbContext.OUT_ThongTinNguoiBenh
            .Where(p => p.SoDienThoai == phoneNumber)
            .Select(p => p.IdOUT_ThongTinNguoiBenh)
            .ToList();

        if (ids.Count != 0)
        {
            // Tìm ngày đánh giá lớn nhất ứng với các IdIN_ThongTinNguoiBenh trong bảng IN_DanhGia
            var maxNgayDanhGia = _appDbContext.OUT_DanhGia
                .Where(d => ids.Contains(d.IdOUT_ThongTinNguoiBenh) && d.IdOUT_MauKhaoSat == id)
                .OrderByDescending(d => d.NgayDanhGia)
                .Select(d => new { d.NgayDanhGia, d.IdOUT_ThongTinNguoiBenh })
                .FirstOrDefault();

            if (maxNgayDanhGia != null)
            {
                // Lấy thông tin bệnh nhân từ bảng IN_ThongTinNguoiBenh
                var patientInfo = _appDbContext.OUT_ThongTinNguoiBenh
                    .FirstOrDefault(p => p.IdOUT_ThongTinNguoiBenh == maxNgayDanhGia.IdOUT_ThongTinNguoiBenh);

                if (patientInfo != null)
                {
                    // Lấy thông tin chung từ bảng IN_ThongTinChung
                    var generalInfo = _appDbContext.OUT_ThongTinChung
                        .FirstOrDefault(p => p.IdOUT_ThongTinNguoiBenh == patientInfo.IdOUT_ThongTinNguoiBenh);

                    // Lấy thông tin ý kiến khác từ bảng IN_ThongTinYKienKhac
                    var additionalInfo = _appDbContext.OUT_ThongTinYKienKhac
                        .FirstOrDefault(p => p.IdOUT_ThongTinNguoiBenh == patientInfo.IdOUT_ThongTinNguoiBenh);

                    // Tạo đối tượng chứa tất cả thông tin cần thiết
                    var result = new
                    {
                        GioiTinh = patientInfo.GioiTinh,
                        Tuoi = patientInfo.Tuoi,
                        KhoangCach = patientInfo.KhoangCach,
                        CoSuDungBHYT = patientInfo.CoSuDungBHYT == true ? "Co" : "Khong",
                        TenBenhVien = generalInfo?.TenBenhVien,
                        PhanTramMongDoi = additionalInfo?.PhanTramMongDoi,
                    };

                    // Trả về dữ liệu dưới dạng JSON nếu có kết quả
                    return Json(result);
                }
            }
        }

        // Nếu không tìm thấy bất kỳ thông tin gì, không trả về phản hồi nào
        return new EmptyResult();
    }


}