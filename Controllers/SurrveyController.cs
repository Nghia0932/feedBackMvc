//using Microsoft.AspNetCore.Mvc;
//using feedBackMvc.Models;
//using System.Threading.Tasks;

//namespace feedBackMvc.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class SurrveyController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public SurrveyController(AppDbContext context)
//        {
//            _context = context;
//        }

//        [HttpPost]
//        [Route("submit")]
//        public async Task<IActionResult> Submit([FromBody] IN_Surrveys surrvey)
//        {
//            if (ModelState.IsValid)
//            {
//                 surrvey.CreatedDate = DateTime.UtcNow;

//                _context.IN_Surrveys.Add(surrvey);
//                await _context.SaveChangesAsync();
//                return Ok(new { message = "Submission successful" });
//            }

//            return BadRequest(ModelState);
//        }
//    }
//}
