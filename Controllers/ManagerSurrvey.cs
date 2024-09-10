using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using feedBackMvc.Models; // Ensure this namespace is correct
using feedBackMvc.Helpers;
using System;

using Microsoft.Extensions.Logging;

public class ManagerSurrveyController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly ILogger<ManagerSurrveyController> _logger;


    public ManagerSurrveyController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<ManagerSurrveyController> logger)
    {
        _appDbContext = appDbContext;
        _jwtTokenHelper = jwtTokenHelper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetSurrvey()
    {
        // Log the start of the action
        _logger.LogInformation("Starting GetSurrvey action");

        // Fetch data from IN_MauKhaoSat
        var inMauKhaoSatList = await _appDbContext.IN_MauKhaoSat.ToListAsync();

        // Fetch data from OUT_MauKhaoSat
        var outMauKhaoSatList = await _appDbContext.OUT_MauKhaoSat.ToListAsync();


        // Log the number of records fetched
        _logger.LogInformation($"Fetched {inMauKhaoSatList.Count} IN_MauKhaoSat records");
        _logger.LogInformation($"Fetched {outMauKhaoSatList.Count} OUT_MauKhaoSat records");


        // Create a view model to pass both sets of data to the view
        var surveyData = new KhaoSatViewModel
        {
            IN_MauKhaoSatList = inMauKhaoSatList,
            OUT_MauKhaoSatList = outMauKhaoSatList,
        };

        // Return the partial view and pass the data
        return PartialView("_KhaoSat", surveyData);
    }
}
