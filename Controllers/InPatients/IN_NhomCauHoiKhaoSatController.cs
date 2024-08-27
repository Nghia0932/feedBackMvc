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
                // Return the partial view
                // ViewData["PartialView"] = "~/Views/InPatients/In_NhomCauHoiKhaoSat/_Show_In_NhomCauHoiKhaoSat.cshtml";
                //return View("~/Views/AdminManager/AdminManager.cshtml", nhomCauHoiKhaoSats);  
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

    if (data.TieuDes.Count != data.NoiDungs.Count)
    {
        return BadRequest("Mismatched number of titles and contents.");
    }

    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        for (int i = 0; i < data.TieuDes.Count; i++)
        {
            var newGroup = new IN_NhomCauHoiKhaoSat
            {
                TieuDe = data.TieuDes[i],
                NoiDung = data.NoiDungs[i],
                idAdmin = adminId
            };

            var existingRecord = await _context.IN_NhomCauHoiKhaoSat
                .Where(x => x.TieuDe == newGroup.TieuDe)
                .FirstOrDefaultAsync();

            if (existingRecord != null)
            {
                await transaction.RollbackAsync();
                return Conflict($"A record with the same TieuDe '{newGroup.TieuDe}' already exists.");
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
    }
}


[HttpPost]
public async Task<IActionResult> XoaNhomCauHoiKhaoSat(int id)
{
    try
    {
        var nhom = await _context.IN_NhomCauHoiKhaoSat.FindAsync(id);
        _logger.LogInformation("Nhóm câu hỏi cần xóa: {@id}", id);
        if (nhom != null)
        {
            _context.IN_NhomCauHoiKhaoSat.Remove(nhom);
            await _context.SaveChangesAsync();
            // Trả về thông tin thành công và yêu cầu tải lại partial view
            return Json(new { success = true, reloadPartialView = true });
        }
        else
        {
            _logger.LogInformation("Nhóm câu hỏi không tìm thấy: {@id}", id);
            return Json(new { success = false, message = "Not found" });
        }
    }
    catch (DbUpdateConcurrencyException ex)
    {
        // Xử lý lỗi xung đột concurrency
        _logger.LogError(ex, "Xung đột concurrency khi xóa nhóm câu hỏi: {@id}", id);
        return Json(new { success = false, message = "Concurrency error: Data may have been modified or deleted." });
    }
    catch (Exception ex)
    {
        // Xử lý lỗi chung
        _logger.LogError(ex, "Lỗi khi xóa nhóm câu hỏi: {@id}", id);
        return Json(new { success = false, message = ex.Message });
    }
}


    }
}
