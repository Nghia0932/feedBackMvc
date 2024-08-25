using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models;
using Microsoft.Extensions.Logging;

namespace feedBackMvc.Controllers.InPatients
{
    public class In_NhomCauHoiKhaoSatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<In_NhomCauHoiKhaoSatController> _logger;

        public In_NhomCauHoiKhaoSatController(AppDbContext context, ILogger<In_NhomCauHoiKhaoSatController> logger)
        {
            _context = context;
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

    }
}
