using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using feedBackMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace feedBackMvc.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<HomeController> _logger;

    public HomeController(AppDbContext appDbContext, ILogger<HomeController> logger)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Feedback()
    {
        return View();
    }
    public async Task<IActionResult> Surrvey()
    {
        // Fetch data from the database
        var inMauKhaoSat = _appDbContext.IN_MauKhaoSat.ToList();
        var outMauKhaoSat = _appDbContext.OUT_MauKhaoSat.ToList();
        var countSurveyIN = await _appDbContext.IN_DanhGia
            .GroupBy(dg => dg.IdIN_MauKhaoSat)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .ToListAsync();
        var countSurveyINDictionary = countSurveyIN.ToDictionary(g => g.Key, g => g.Count);

        // Asynchronously count the number of surveys in OUT_DanhGia grouped by IdOUT_MauKhaoSat
        var countSurveyOUT = await _appDbContext.OUT_DanhGia
            .GroupBy(dg => dg.IdOUT_MauKhaoSat)
            .Select(g => new { Key = g.Key, Count = g.Count() })
            .ToListAsync();
        var countSurveyOUTDictionary = countSurveyOUT.ToDictionary(g => g.Key, g => g.Count);
        // Create the view model

        var viewModel = new KhaoSatViewModel
        {
            IN_MauKhaoSatList = inMauKhaoSat,
            OUT_MauKhaoSatList = outMauKhaoSat,
            CountSurvey_IN_MauKhaoSat = countSurveyINDictionary,
            CountSurvey_OUT_MauKhaoSat = countSurveyOUTDictionary
        };
        // Pass the view model to the view
        return View(viewModel);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
