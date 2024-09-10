using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using feedBackMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace feedBackMvc.Controllers;

public class DanhGiaController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DanhGiaController> _logger;

    public DanhGiaController(AppDbContext appDbContext, ILogger<DanhGiaController> logger)
    {
        _appDbContext = appDbContext;
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
}