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
    public class In_CauHoiKhaoSatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly ILogger<In_CauHoiKhaoSatController> _logger;

        public In_CauHoiKhaoSatController(AppDbContext context, JwtTokenHelper jwtTokenHelper, ILogger<In_CauHoiKhaoSatController> logger)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
            _logger = logger;

        }

        public async Task<IActionResult> Show_In_CauHoiKhaoSat()
        {
            try
            {
                // Retrieve data from the database
                var cauHoiKhaoSats = _context.IN_CauHoiKhaoSat;
                // Log information  
                _logger.LogInformation("Successfully retrieved IN_CauHoiKhaoSat data.");
                return PartialView("_Show_In_CauHoiKhaoSat", cauHoiKhaoSats);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while retrieving IN_CauHoiKhaoSat data.");

                // Handle the error (return an error view, etc.)
                return StatusCode(500, "Internal server error");
            }
        }
        public class TitleAndQuestionList
        {
            public List<string>? TieuDeCauHois { get; set; }
            public List<string>? CauHois { get; set; }
            public int Id {get; set;}
        }
        [HttpPost]
        public async Task<IActionResult> ThemCauHoiKhaoSat([FromBody] TitleAndQuestionList data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (data.TieuDeCauHois.Count != data.CauHois.Count)
            {
                return BadRequest("Mismatched number of titles and contents.");
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                for (int i = 0; i < data.TieuDeCauHois.Count; i++)
                {
                    var newGroup = new IN_CauHoiKhaoSat
                    {
                        TieuDeCauHoi = data.TieuDeCauHois[i],
                        CauHoi = data.CauHois[i],
                        IdIN_NhomCauHoiKhaoSat = data.Id,
                    };
                    var existingRecord = await _context.IN_CauHoiKhaoSat
                        .Where(x => x.TieuDeCauHoi == newGroup.TieuDeCauHoi)
                        .FirstOrDefaultAsync();
                    if (existingRecord != null)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { success = false, message = "Tiêu đề đã tồn tại"});
                        //return Json(new { success = false });
                    }
                    _context.IN_CauHoiKhaoSat.Add(newGroup);
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
                var nhom = await _context.IN_CauHoiKhaoSat.FindAsync(request.Id);
                if (nhom == null)
                {
                    return Json(new { success = false, message = "Nhóm câu hỏi không tồn tại." });
                }

                _context.IN_CauHoiKhaoSat.Remove(nhom);
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
            public string TieuDeCauHoi { get; set; }
            public string CauHoi { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatCauHoiKhaoSat([FromBody] List<UpdateRequest> request)
        {
            if (request == null || !request.Any())
                {
                    return BadRequest(new { success = false, message = "Không nhận được dữ liệu cập nhật." });
                }
                try{
                    foreach(var item in request){
                        // Giả sử bạn có một DbContext tên là _context
                        var existingItem = await _context.IN_CauHoiKhaoSat.FindAsync(item.Id);
                        if(existingItem != null)
                        {
                            existingItem.TieuDeCauHoi = item.TieuDeCauHoi;
                            existingItem.CauHoi = item.CauHoi;
                            // Bạn có thể thêm các logic kiểm tra khác ở đây
                        }
                    }
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Cập nhật thành công." });
                }catch(Exception ex){
                    // Xử lý ngoại lệ và ghi log nếu cần
                    return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
                }
            }
    }
}
