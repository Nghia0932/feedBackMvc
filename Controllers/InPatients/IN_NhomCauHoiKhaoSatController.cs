using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models;
using Microsoft.Extensions.Logging;
using feedBackMvc.Helpers;

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
        public List<string> TieuDes { get; set; }
        public List<string> NoiDungs { get; set; }
    }
    [HttpPost]
    public IActionResult ThemNhomCauHoiKhaoSat([FromBody] TitleAndContentList data)
    {
        if (ModelState.IsValid)
        {
            // Get the admin ID from the session or another source
            var token = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(token) || !_jwtTokenHelper.TryParseToken(token, out var adminId))
            {
                return BadRequest("Invalid token or admin ID.");
            }

            // Ensure the data lengths match
            if (data.TieuDes.Count != data.NoiDungs.Count)
            {
                return BadRequest("Mismatched number of titles and contents.");
            }

            // Add each title and content to the database
            for (int i = 0; i < data.TieuDes.Count; i++)
            {
                var newGroup = new IN_NhomCauHoiKhaoSat
                {
                    TieuDe = data.TieuDes[i],
                    NoiDung = data.NoiDungs[i],
                    idAdmin = adminId
                };
                 _context.IN_NhomCauHoiKhaoSat.Add(newGroup);
            }
             _context.SaveChanges();
            return Json(new { success = true }); 
           
        }
        return BadRequest(ModelState);
    }
  

    [HttpPost]
    public IActionResult XoaNhomCauHoiKhaoSat( int id)
    {
        try
        {
            var nhom = _context.IN_NhomCauHoiKhaoSat.Find(id);
            _logger.LogInformation("Nhóm câu hỏi cần xóa: {@id}", id);
            if (nhom != null)
            {
                _context.IN_NhomCauHoiKhaoSat.Remove(nhom);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Not found" });
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            return Json(new { success = false, message = ex.Message });
        }
    }
    }
}
