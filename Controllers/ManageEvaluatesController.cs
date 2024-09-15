using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedBackMvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using feedBackMvc.Helpers;
using System;
using Microsoft.Extensions.Logging;
using Dapper;
using Npgsql;
using System.Data; // Giả sử bạn đang dùng PostgreSQL

public class ManageEvaluatesController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly ILogger<ManageEvaluatesController> _logger;
    private readonly string _connectionString;

    public ManageEvaluatesController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<ManageEvaluatesController> logger, IConfiguration configuration)
    {
        _appDbContext = appDbContext;
        _jwtTokenHelper = jwtTokenHelper;
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection"); ;
    }


    public async Task<IActionResult> GetManageEvaluates()
    {
        var model = new QL_DanhSachDanhGiaViewModel();
        model.IN_MauKhaoSatList = await _appDbContext.IN_MauKhaoSat.ToListAsync();
        model.OUT_MauKhaoSatList = await _appDbContext.OUT_MauKhaoSat.ToListAsync();
        // Query for IN_ThongTinNguoiKhaoSat
        var queryIn = @"
            SELECT 
                nbenh.""SoDienThoai"",
                ykkhac.""PhanTramMongDoi"",
                ykkhac.""QuayLaiVaGioiThieu"",
                ykkhac.""YKienKhac"",
                ykkhac.""NgayTao"" AS ""NgayKhaoSat"",
                mks.""TenMauKhaoSat""
            FROM 
                ""IN_DanhGia"" dg
            LEFT JOIN 
                ""IN_MauKhaoSat"" mks ON dg.""IdIN_MauKhaoSat"" = mks.""IdIN_MauKhaoSat""
            LEFT JOIN 
                ""IN_ThongTinNguoiBenh"" nbenh ON dg.""IdIN_ThongTinNguoiBenh"" = nbenh.""IdIN_ThongTinNguoiBenh""
            LEFT JOIN 
                ""IN_ThongTinYKienKhac"" ykkhac ON nbenh.""IdIN_ThongTinNguoiBenh"" = ykkhac.""IdIN_ThongTinNguoiBenh""
            ORDER BY 
                mks.""TenMauKhaoSat"", ykkhac.""NgayTao"" ASC";

        model.IN_TTNguoiKhaoSat = await ExecuteQuery(queryIn, MapToInThongTinNguoiKhaoSat);

        // Query for OUT_ThongTinNguoiKhaoSat
        var queryOut = @"
            SELECT 
                nbenh.""SoDienThoai"",
                ykkhac.""PhanTramMongDoi"",
                ykkhac.""QuayLaiVaGioiThieu"",
                ykkhac.""YKienKhac"",
                ykkhac.""NgayTao"" AS ""NgayKhaoSat"",
                mks.""TenMauKhaoSat""
            FROM 
                ""OUT_DanhGia"" dg
            LEFT JOIN 
                ""OUT_MauKhaoSat"" mks ON dg.""IdOUT_MauKhaoSat"" = mks.""IdOUT_MauKhaoSat""
            LEFT JOIN 
                ""OUT_ThongTinNguoiBenh"" nbenh ON dg.""IdOUT_ThongTinNguoiBenh"" = nbenh.""IdOUT_ThongTinNguoiBenh""
            LEFT JOIN 
                ""OUT_ThongTinYKienKhac"" ykkhac ON nbenh.""IdOUT_ThongTinNguoiBenh"" = ykkhac.""IdOUT_ThongTinNguoiBenh""
            ORDER BY 
                mks.""TenMauKhaoSat"", ykkhac.""NgayTao"" ASC";

        model.OUT_TTNguoiKhaoSat = await ExecuteQuery(queryOut, MapToOutThongTinNguoiKhaoSat);

        // Return the model to the view
        return PartialView("_QL_DanhSachDanhGia", model);
    }


    private async Task<List<T>> ExecuteQuery<T>(string query, Func<System.Data.Common.DbDataReader, T> mapFunction)
    {
        var resultList = new List<T>();

        await using (var command = _appDbContext.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = query;
            command.CommandType = System.Data.CommandType.Text;

            if (command.Connection.State != System.Data.ConnectionState.Open)
                await command.Connection.OpenAsync();

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    resultList.Add(mapFunction(reader));
                }
            }
        }

        return resultList;
    }

    private QL_DanhSachDanhGiaViewModel.IN_ThongTinNguoiKhaoSat MapToInThongTinNguoiKhaoSat(System.Data.Common.DbDataReader reader)
    {
        return new QL_DanhSachDanhGiaViewModel.IN_ThongTinNguoiKhaoSat
        {
            SoDienThoai = reader.GetString(reader.GetOrdinal("SoDienThoai")),
            NgayKhaoSat = reader.GetDateTime(reader.GetOrdinal("NgayKhaoSat")),
            QuayLaiVaGioiThieu = reader.IsDBNull(reader.GetOrdinal("QuayLaiVaGioiThieu")) ? null : reader.GetString(reader.GetOrdinal("QuayLaiVaGioiThieu")),
            PhanTramMongDoi = reader.IsDBNull(reader.GetOrdinal("PhanTramMongDoi")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("PhanTramMongDoi")),
            YKienKhac = reader.IsDBNull(reader.GetOrdinal("YKienKhac")) ? null : reader.GetString(reader.GetOrdinal("YKienKhac")),
            Ten_IN_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("TenMauKhaoSat")) ? null : reader.GetString(reader.GetOrdinal("TenMauKhaoSat"))
        };
    }

    private QL_DanhSachDanhGiaViewModel.OUT_ThongTinNguoiKhaoSat MapToOutThongTinNguoiKhaoSat(System.Data.Common.DbDataReader reader)
    {
        return new QL_DanhSachDanhGiaViewModel.OUT_ThongTinNguoiKhaoSat
        {
            SoDienThoai = reader.GetString(reader.GetOrdinal("SoDienThoai")),
            NgayKhaoSat = reader.GetDateTime(reader.GetOrdinal("NgayKhaoSat")),
            QuayLaiVaGioiThieu = reader.IsDBNull(reader.GetOrdinal("QuayLaiVaGioiThieu")) ? null : reader.GetString(reader.GetOrdinal("QuayLaiVaGioiThieu")),
            PhanTramMongDoi = reader.IsDBNull(reader.GetOrdinal("PhanTramMongDoi")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("PhanTramMongDoi")),
            YKienKhac = reader.IsDBNull(reader.GetOrdinal("YKienKhac")) ? null : reader.GetString(reader.GetOrdinal("YKienKhac")),
            Ten_OUT_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("TenMauKhaoSat")) ? null : reader.GetString(reader.GetOrdinal("TenMauKhaoSat"))
        };
    }

    [HttpPost]
    public async Task<ActionResult> GetSurveyData(string surveyName)
    {
        var query = string.Empty;
        if (surveyName == "all")
        {
            query = @"
                SELECT 
                    nbenh.""SoDienThoai"",
                    ykkhac.""PhanTramMongDoi"",
                    ykkhac.""QuayLaiVaGioiThieu"",
                    ykkhac.""YKienKhac"",
                    ykkhac.""NgayTao"" AS ""NgayKhaoSat"",
                    mks.""TenMauKhaoSat""
                FROM 
                    ""IN_DanhGia"" dg
                LEFT JOIN 
                    ""IN_MauKhaoSat"" mks ON dg.""IdIN_MauKhaoSat"" = mks.""IdIN_MauKhaoSat""
                LEFT JOIN 
                    ""IN_ThongTinNguoiBenh"" nbenh ON dg.""IdIN_ThongTinNguoiBenh"" = nbenh.""IdIN_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""IN_ThongTinYKienKhac"" ykkhac ON nbenh.""IdIN_ThongTinNguoiBenh"" = ykkhac.""IdIN_ThongTinNguoiBenh""
                ORDER BY 
                    mks.""TenMauKhaoSat"", ykkhac.""NgayTao"" ASC";
        }
        else
        {
            query = @"
                SELECT 
                    nbenh.""SoDienThoai"",
                    ykkhac.""PhanTramMongDoi"",
                    ykkhac.""QuayLaiVaGioiThieu"",
                    ykkhac.""YKienKhac"",
                    ykkhac.""NgayTao"" AS ""NgayKhaoSat"",
                    mks.""TenMauKhaoSat""
                FROM 
                    ""IN_DanhGia"" dg
                LEFT JOIN 
                    ""IN_MauKhaoSat"" mks ON dg.""IdIN_MauKhaoSat"" = mks.""IdIN_MauKhaoSat""
                LEFT JOIN 
                    ""IN_ThongTinNguoiBenh"" nbenh ON dg.""IdIN_ThongTinNguoiBenh"" = nbenh.""IdIN_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""IN_ThongTinYKienKhac"" ykkhac ON nbenh.""IdIN_ThongTinNguoiBenh"" = ykkhac.""IdIN_ThongTinNguoiBenh""
                WHERE 
                    mks.""TenMauKhaoSat"" = @surveyName
                ORDER BY 
                    mks.""TenMauKhaoSat"", ykkhac.""NgayTao"" ASC";
        }
        using (IDbConnection db = new NpgsqlConnection(_connectionString))
        {
            var parameters = new { surveyName };
            var data = await db.QueryAsync(query, parameters);

            // Format date to "yyyy-MM-dd"
            var formattedData = data.Select(item => new
            {

                SoDienThoai = item.SoDienThoai,
                PhanTramMongDoi = item.PhanTramMongDoi,
                NgayKhaoSat = ((DateTime)item.NgayKhaoSat).ToString("dd/MM/yyyy"), // Format date here
                QuayLaiVaGioiThieu = item.QuayLaiVaGioiThieu,
                YKienKhac = item.YKienKhac,
            });

            return Json(new { data = formattedData });
        }
    }
    [HttpPost]
    public async Task<ActionResult> OUT_GetSurveyData(string surveyName)
    {
        var query = string.Empty;
        if (surveyName == "all")
        {
            query = @"
                SELECT 
                    nbenh.""SoDienThoai"",
                    ykkhac.""PhanTramMongDoi"",
                    ykkhac.""QuayLaiVaGioiThieu"",
                    ykkhac.""YKienKhac"",
                    ykkhac.""NgayTao"" AS ""NgayKhaoSat"",
                    mks.""TenMauKhaoSat""
                FROM 
                    ""OUT_DanhGia"" dg
                LEFT JOIN 
                    ""OUT_MauKhaoSat"" mks ON dg.""IdOUT_MauKhaoSat"" = mks.""IdOUT_MauKhaoSat""
                LEFT JOIN 
                    ""OUT_ThongTinNguoiBenh"" nbenh ON dg.""IdOUT_ThongTinNguoiBenh"" = nbenh.""IdOUT_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""OUT_ThongTinYKienKhac"" ykkhac ON nbenh.""IdOUT_ThongTinNguoiBenh"" = ykkhac.""IdOUT_ThongTinNguoiBenh""
                ORDER BY 
                    mks.""TenMauKhaoSat"", ykkhac.""NgayTao"" ASC";
        }
        else
        {
            query = @"
                SELECT 
                    nbenh.""SoDienThoai"",
                    ykkhac.""PhanTramMongDoi"",
                    ykkhac.""QuayLaiVaGioiThieu"",
                    ykkhac.""YKienKhac"",
                    ykkhac.""NgayTao"" AS ""NgayKhaoSat"",
                    mks.""TenMauKhaoSat""
                FROM 
                    ""OUT_DanhGia"" dg
                LEFT JOIN 
                    ""OUT_MauKhaoSat"" mks ON dg.""IdOUT_MauKhaoSat"" = mks.""IdOUT_MauKhaoSat""
                LEFT JOIN 
                    ""OUT_ThongTinNguoiBenh"" nbenh ON dg.""IdOUT_ThongTinNguoiBenh"" = nbenh.""IdOUT_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""OUT_ThongTinYKienKhac"" ykkhac ON nbenh.""IdOUT_ThongTinNguoiBenh"" = ykkhac.""IdOUT_ThongTinNguoiBenh""
                WHERE 
                    mks.""TenMauKhaoSat"" = @surveyName
                ORDER BY 
                    mks.""TenMauKhaoSat"", ykkhac.""NgayTao"" ASC";
        }
        using (IDbConnection db = new NpgsqlConnection(_connectionString))
        {
            var parameters = new { surveyName };
            var data = await db.QueryAsync(query, parameters);

            // Format date to "yyyy-MM-dd"
            var formattedData = data.Select(item => new
            {
                SoDienThoai = item.SoDienThoai,
                PhanTramMongDoi = item.PhanTramMongDoi,
                NgayKhaoSat = ((DateTime)item.NgayKhaoSat).ToString("dd/MM/yyyy"), // Format date here
                QuayLaiVaGioiThieu = item.QuayLaiVaGioiThieu,
                YKienKhac = item.YKienKhac,
            });

            return Json(new { data = formattedData });
        }
    }



}
