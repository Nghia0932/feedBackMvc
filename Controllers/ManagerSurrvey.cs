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

        var inMauKhaoSatList = await _appDbContext.IN_MauKhaoSat
                   .Where(survey => survey.Xoa == false)
                   .ToListAsync();

        var outMauKhaoSatList = await _appDbContext.OUT_MauKhaoSat
            .Where(survey => survey.Xoa == false)
            .ToListAsync();

        // Log the number of records fetched
        _logger.LogInformation($"Fetched {inMauKhaoSatList.Count} IN_MauKhaoSat records");
        _logger.LogInformation($"Fetched {outMauKhaoSatList.Count} OUT_MauKhaoSat records");
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


        // Create a view model to pass both sets of data to the view
        var surveyData = new KhaoSatViewModel
        {
            IN_MauKhaoSatList = inMauKhaoSatList,
            OUT_MauKhaoSatList = outMauKhaoSatList,
            CountSurvey_IN_MauKhaoSat = countSurveyINDictionary,
            CountSurvey_OUT_MauKhaoSat = countSurveyOUTDictionary
        };

        // Return the partial view and pass the data
        return PartialView("_QL_MauKhaoSat", surveyData);
    }
    [HttpGet]
    public async Task<IActionResult> GetDeleteSurrvey()
    {
        // Log the start of the action
        _logger.LogInformation("Starting GetSurrvey action");

        var inMauKhaoSatList = await _appDbContext.IN_MauKhaoSat
                   .Where(survey => survey.Xoa == true)
                   .ToListAsync();

        var outMauKhaoSatList = await _appDbContext.OUT_MauKhaoSat
            .Where(survey => survey.Xoa == true)
            .ToListAsync();

        // Log the number of records fetched
        _logger.LogInformation($"Fetched {inMauKhaoSatList.Count} IN_MauKhaoSat records");
        _logger.LogInformation($"Fetched {outMauKhaoSatList.Count} OUT_MauKhaoSat records");
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


        // Create a view model to pass both sets of data to the view
        var surveyData = new KhaoSatViewModel
        {
            IN_MauKhaoSatList = inMauKhaoSatList,
            OUT_MauKhaoSatList = outMauKhaoSatList,
            CountSurvey_IN_MauKhaoSat = countSurveyINDictionary,
            CountSurvey_OUT_MauKhaoSat = countSurveyOUTDictionary
        };

        // Return the partial view and pass the data
        return PartialView("_QL_MauKhaoSatXoa", surveyData);
    }

}
