using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models; // Ensure this namespace is correct
using feedBackMvc.Helpers;
using System;

using Microsoft.Extensions.Logging;

public class DashboardController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly ILogger<DashboardController> _logger;


    public DashboardController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<DashboardController> logger)
    {
        _appDbContext = appDbContext;
        _jwtTokenHelper = jwtTokenHelper;
        _logger = logger;
    }
    [HttpGet]
    public async Task<IActionResult> Get_Dashboard()
    {
        var DashboardViewModel = new DashboardViewModel();

        // Fetch the IdIN_MauKhaoSat where TrangThai is true
        var inMauKhaoSat = await _appDbContext.IN_MauKhaoSat
            .FirstOrDefaultAsync(m => m.TrangThai == true);

        var OUT_MauKhaoSat = await _appDbContext.OUT_MauKhaoSat
            .FirstOrDefaultAsync(m => m.TrangThai == true);

        if (inMauKhaoSat == null && OUT_MauKhaoSat == null)
        {
            return PartialView("_None_Dashboard");
        }
        else
        {
            if (inMauKhaoSat != null)
            {
                int idIN_MauKhaoSat = inMauKhaoSat.IdIN_MauKhaoSat;

                if (inMauKhaoSat.NhomCauHoiKhaoSat != null && inMauKhaoSat.NhomCauHoiKhaoSat.Any())
                {
                    // Fetch NoiDung for each TieuDe in NhomCauHoiKhaoSat
                    DashboardViewModel.IN_MangCauHoi = await _appDbContext.IN_NhomCauHoiKhaoSat
                        .Where(n => inMauKhaoSat.NhomCauHoiKhaoSat.Contains(n.TieuDe))
                        .Select(n => n.NoiDung)
                        .ToListAsync();

                    // Fetch DanhGiaTong from IN_DanhGia using the dynamic idIN_MauKhaoSat
                    var danhGiaList = await _appDbContext.IN_DanhGia
                        .Where(d => d.IdIN_MauKhaoSat == idIN_MauKhaoSat)
                        .Select(d => d.DanhGiaTong)
                        .ToListAsync();

                    // Transpose the DanhGiaTong list
                    if (danhGiaList.Any() && danhGiaList.All(d => d != null))
                    {
                        int length = danhGiaList[0].Count();
                        DashboardViewModel.IN_DanhGiaTong = Enumerable.Range(0, length)
                            .Select(i => danhGiaList.Select(inner => inner[i]).ToList())
                            .ToList();
                    }
                    else
                    {
                        DashboardViewModel.IN_DanhGiaTong = new List<List<double>>();
                    }
                }
                var thongTinNguoiBenhIds = await _appDbContext.IN_DanhGia
               .Where(d => d.IdIN_MauKhaoSat == inMauKhaoSat.IdIN_MauKhaoSat)
               .Select(d => d.IdIN_ThongTinNguoiBenh)
               .ToListAsync();

                if (thongTinNguoiBenhIds.Any())
                {
                    // Step 3: Fetch records from IN_ThongTinNguoiBenh based on the retrieved IDs
                    var khoaList = await _appDbContext.IN_ThongTinChung
                        .Where(tb => thongTinNguoiBenhIds.Contains(tb.IdIN_ThongTinNguoiBenh))
                        .Select(tb => tb.TenKhoa)
                        .ToListAsync();

                    // Step 4: Count the number of occurrences of each TenKhoa
                    var khoaCounts = khoaList
                        .GroupBy(khoa => khoa)
                        .Select(group => new
                        {
                            TenKhoa = group.Key,
                            Count = group.Count()
                        })
                        .ToList();

                    // Store the results in your view model
                    DashboardViewModel.IN_KhoaCounts = khoaCounts.Select(k => new KhoaCountViewModel
                    {
                        TenKhoa = k.TenKhoa,
                        Count = k.Count
                    }).ToList();

                    var phanTramMongDoiList = await _appDbContext.IN_ThongTinYKienKhac
                        .Where(tb => thongTinNguoiBenhIds.Contains(tb.IdIN_ThongTinNguoiBenh) && tb.PhanTramMongDoi.HasValue)
                        .Select(tb => tb.PhanTramMongDoi.Value) // This will return a list of integers
                        .ToListAsync();

                    // Store the results in your view model
                    DashboardViewModel.IN_PhanTramMongDoi = phanTramMongDoiList;

                    // Group the data by TenKhoa and calculate the total percentage expectation and count for each department
                    // Step 1: Fetch records from IN_ThongTinChung based on the retrieved IDs and get TenKhoa
                    var khoaLists = await _appDbContext.IN_ThongTinChung
                        .Where(tb => thongTinNguoiBenhIds.Contains(tb.IdIN_ThongTinNguoiBenh))
                        .Select(tb => new { tb.IdIN_ThongTinNguoiBenh, tb.TenKhoa })
                        .ToListAsync();

                    // Step 2: Fetch PhanTramMongDoi from IN_ThongTinYKienKhac based on thongTinNguoiBenhIds
                    var phanTramMongDoiLists = await _appDbContext.IN_ThongTinYKienKhac
                        .Where(tb => thongTinNguoiBenhIds.Contains(tb.IdIN_ThongTinNguoiBenh) && tb.PhanTramMongDoi.HasValue)
                        .Select(tb => new { tb.IdIN_ThongTinNguoiBenh, tb.PhanTramMongDoi })
                        .ToListAsync();

                    // Step 3: Combine khoaList and phanTramMongDoiList based on IdIN_ThongTinNguoiBenh
                    var khoaPhanTramMongDoi = from khoa in khoaLists
                                              join phanTram in phanTramMongDoiLists
                                              on khoa.IdIN_ThongTinNguoiBenh equals phanTram.IdIN_ThongTinNguoiBenh
                                              select new
                                              {
                                                  khoa.TenKhoa,
                                                  PhanTramMongDoi = phanTram.PhanTramMongDoi.Value
                                              };
                    // Step 4: Group the data by TenKhoa and calculate the total percentage expectation for each department
                    var khoaGroupData = khoaPhanTramMongDoi
                        .GroupBy(k => k.TenKhoa)
                        .Select(group => new
                        {
                            TenKhoa = group.Key,
                            TotalPhanTramMongDoi = group.Sum(g => g.PhanTramMongDoi),
                            Count = group.Count()
                        })
                        .ToList();

                    // Step 5: Calculate the average percentage expectation for each department
                    var phanTramMongDoiTheoKhoa = khoaGroupData
                        .Select(k => new
                        {
                            TenKhoa = k.TenKhoa,
                            PhanTramMongDoi = k.TotalPhanTramMongDoi / k.Count // Calculating the average percentage expectation
                        })
                        .ToList();

                    // Store the results in your view model
                    DashboardViewModel.IN_PhanTramMongDoiTheoKhoa = phanTramMongDoiTheoKhoa.Select(p => new PhanTramMongDoiViewModel
                    {
                        TenKhoa = p.TenKhoa,
                        PhanTramMongDoi = p.PhanTramMongDoi
                    }).ToList();
                }
            }
            if (OUT_MauKhaoSat != null)
            {
                int idOUT_MauKhaoSat = OUT_MauKhaoSat.IdOUT_MauKhaoSat;

                if (OUT_MauKhaoSat.NhomCauHoiKhaoSat != null && OUT_MauKhaoSat.NhomCauHoiKhaoSat.Any())
                {
                    // Fetch NoiDung for each TieuDe in NhomCauHoiKhaoSat
                    DashboardViewModel.OUT_MangCauHoi = await _appDbContext.OUT_NhomCauHoiKhaoSat
                        .Where(n => OUT_MauKhaoSat.NhomCauHoiKhaoSat.Contains(n.TieuDe))
                        .Select(n => n.NoiDung)
                        .ToListAsync();

                    // Fetch DanhGiaTong from IN_DanhGia using the dynamic idIN_MauKhaoSat
                    var danhGiaList = await _appDbContext.OUT_DanhGia
                        .Where(d => d.IdOUT_MauKhaoSat == idOUT_MauKhaoSat)
                        .Select(d => d.DanhGiaTong)
                        .ToListAsync();

                    // Transpose the DanhGiaTong list
                    if (danhGiaList.Any() && danhGiaList.All(d => d != null))
                    {
                        int length = danhGiaList[0].Count();
                        DashboardViewModel.OUT_DanhGiaTong = Enumerable.Range(0, length)
                            .Select(i => danhGiaList.Select(inner => inner[i]).ToList())
                            .ToList();
                    }
                    else
                    {
                        DashboardViewModel.OUT_DanhGiaTong = new List<List<double>>();
                    }
                }
                var OUT_thongTinNguoiBenhIds = await _appDbContext.OUT_DanhGia
               .Where(d => d.IdOUT_MauKhaoSat == OUT_MauKhaoSat.IdOUT_MauKhaoSat)
               .Select(d => d.IdOUT_ThongTinNguoiBenh)
               .ToListAsync();

                if (OUT_thongTinNguoiBenhIds.Any())
                {
                    var phanTramMongDoiList = await _appDbContext.OUT_ThongTinYKienKhac
                        .Where(tb => OUT_thongTinNguoiBenhIds.Contains(tb.IdOUT_ThongTinNguoiBenh) && tb.PhanTramMongDoi.HasValue)
                        .Select(tb => tb.PhanTramMongDoi.Value) // This will return a list of integers
                        .ToListAsync();
                    // Store the results in your view model
                    DashboardViewModel.OUT_PhanTramMongDoi = phanTramMongDoiList;
                }
            }
        }

        return PartialView("_QL_Dashboard", DashboardViewModel);
    }

}