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
using Newtonsoft.Json;

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
        model.IN_MauKhaoSatList = await _appDbContext.IN_MauKhaoSat
            .Where(survey => survey.HienThi == true && survey.Xoa == false)
            .ToListAsync();

        model.OUT_MauKhaoSatList = await _appDbContext.OUT_MauKhaoSat
            .Where(survey => survey.HienThi == true && survey.Xoa == false)
            .ToListAsync();
        // Query for IN_ThongTinNguoiKhaoSat
        var queryIn = @"
            SELECT 
                nbenh.""SoDienThoai"",
                ykkhac.""PhanTramMongDoi"",
                ykkhac.""QuayLaiVaGioiThieu"",
                ykkhac.""YKienKhac"",
                ykkhac.""NgayTao"" AS ""NgayKhaoSat"",
                mks.""TenMauKhaoSat"",
                mks.""IdIN_MauKhaoSat"",
                nbenh.""IdIN_ThongTinNguoiBenh""
            FROM 
                ""IN_DanhGia"" dg
            LEFT JOIN 
                ""IN_MauKhaoSat"" mks ON dg.""IdIN_MauKhaoSat"" = mks.""IdIN_MauKhaoSat""
            LEFT JOIN 
                ""IN_ThongTinNguoiBenh"" nbenh ON dg.""IdIN_ThongTinNguoiBenh"" = nbenh.""IdIN_ThongTinNguoiBenh""
            LEFT JOIN 
                ""IN_ThongTinYKienKhac"" ykkhac ON nbenh.""IdIN_ThongTinNguoiBenh"" = ykkhac.""IdIN_ThongTinNguoiBenh""
            WHERE 
                mks.""HienThi"" = true 
                AND mks.""Xoa"" = false
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
                mks.""TenMauKhaoSat"",
                mks.""IdOUT_MauKhaoSat"",
                nbenh.""IdOUT_ThongTinNguoiBenh""
            FROM 
                ""OUT_DanhGia"" dg
            LEFT JOIN 
                ""OUT_MauKhaoSat"" mks ON dg.""IdOUT_MauKhaoSat"" = mks.""IdOUT_MauKhaoSat""
            LEFT JOIN 
                ""OUT_ThongTinNguoiBenh"" nbenh ON dg.""IdOUT_ThongTinNguoiBenh"" = nbenh.""IdOUT_ThongTinNguoiBenh""
            LEFT JOIN 
                ""OUT_ThongTinYKienKhac"" ykkhac ON nbenh.""IdOUT_ThongTinNguoiBenh"" = ykkhac.""IdOUT_ThongTinNguoiBenh""
            WHERE 
                mks.""HienThi"" = true 
                AND mks.""Xoa"" = false
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
            SoDienThoai = reader.IsDBNull(reader.GetOrdinal("SoDienThoai")) ? null : reader.GetString(reader.GetOrdinal("SoDienThoai")),
            NgayKhaoSat = reader.IsDBNull(reader.GetOrdinal("NgayKhaoSat"))
            ? DateTime.MinValue
            : reader.GetDateTime(reader.GetOrdinal("NgayKhaoSat")),
            QuayLaiVaGioiThieu = reader.IsDBNull(reader.GetOrdinal("QuayLaiVaGioiThieu")) ? null : reader.GetString(reader.GetOrdinal("QuayLaiVaGioiThieu")),
            PhanTramMongDoi = reader.IsDBNull(reader.GetOrdinal("PhanTramMongDoi")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("PhanTramMongDoi")),
            YKienKhac = reader.IsDBNull(reader.GetOrdinal("YKienKhac")) ? null : reader.GetString(reader.GetOrdinal("YKienKhac")),
            Ten_IN_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("TenMauKhaoSat")) ? null : reader.GetString(reader.GetOrdinal("TenMauKhaoSat")),
            IdIN_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("IdIN_MauKhaoSat")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdIN_MauKhaoSat")),
            IdIN_ThongTinNguoiBenh = reader.IsDBNull(reader.GetOrdinal("IdIN_ThongTinNguoiBenh")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdIN_ThongTinNguoiBenh"))
        };
    }

    private QL_DanhSachDanhGiaViewModel.OUT_ThongTinNguoiKhaoSat MapToOutThongTinNguoiKhaoSat(System.Data.Common.DbDataReader reader)
    {
        return new QL_DanhSachDanhGiaViewModel.OUT_ThongTinNguoiKhaoSat
        {
            SoDienThoai = reader.IsDBNull(reader.GetOrdinal("SoDienThoai")) ? null : reader.GetString(reader.GetOrdinal("SoDienThoai")),
            NgayKhaoSat = reader.IsDBNull(reader.GetOrdinal("NgayKhaoSat"))
            ? DateTime.MinValue
            : reader.GetDateTime(reader.GetOrdinal("NgayKhaoSat")),
            QuayLaiVaGioiThieu = reader.IsDBNull(reader.GetOrdinal("QuayLaiVaGioiThieu")) ? null : reader.GetString(reader.GetOrdinal("QuayLaiVaGioiThieu")),
            PhanTramMongDoi = reader.IsDBNull(reader.GetOrdinal("PhanTramMongDoi")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("PhanTramMongDoi")),
            YKienKhac = reader.IsDBNull(reader.GetOrdinal("YKienKhac")) ? null : reader.GetString(reader.GetOrdinal("YKienKhac")),
            Ten_OUT_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("TenMauKhaoSat")) ? null : reader.GetString(reader.GetOrdinal("TenMauKhaoSat")),
            IdOUT_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("IdOUT_MauKhaoSat")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdOUT_MauKhaoSat")),
            IdOUT_ThongTinNguoiBenh = reader.IsDBNull(reader.GetOrdinal("IdOUT_ThongTinNguoiBenh")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdOUT_ThongTinNguoiBenh"))

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
                    mks.""TenMauKhaoSat"",
                    mks.""IdIN_MauKhaoSat"",
                    nbenh.""IdIN_ThongTinNguoiBenh""
                FROM 
                    ""IN_DanhGia"" dg
                LEFT JOIN 
                    ""IN_MauKhaoSat"" mks ON dg.""IdIN_MauKhaoSat"" = mks.""IdIN_MauKhaoSat""
                LEFT JOIN 
                    ""IN_ThongTinNguoiBenh"" nbenh ON dg.""IdIN_ThongTinNguoiBenh"" = nbenh.""IdIN_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""IN_ThongTinYKienKhac"" ykkhac ON nbenh.""IdIN_ThongTinNguoiBenh"" = ykkhac.""IdIN_ThongTinNguoiBenh""
                WHERE 
                    mks.""HienThi"" = true 
                    AND mks.""Xoa"" = false
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
                    mks.""TenMauKhaoSat"",
                    mks.""IdIN_MauKhaoSat"",
                    nbenh.""IdIN_ThongTinNguoiBenh""
                FROM 
                    ""IN_DanhGia"" dg
                LEFT JOIN 
                    ""IN_MauKhaoSat"" mks ON dg.""IdIN_MauKhaoSat"" = mks.""IdIN_MauKhaoSat""
                LEFT JOIN 
                    ""IN_ThongTinNguoiBenh"" nbenh ON dg.""IdIN_ThongTinNguoiBenh"" = nbenh.""IdIN_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""IN_ThongTinYKienKhac"" ykkhac ON nbenh.""IdIN_ThongTinNguoiBenh"" = ykkhac.""IdIN_ThongTinNguoiBenh""
                WHERE 
                    mks.""HienThi"" = true 
                    AND mks.""Xoa"" = false
                    AND mks.""TenMauKhaoSat"" = @surveyName
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
                NgayKhaoSat = item.NgayKhaoSat != null
                ? ((DateTime?)item.NgayKhaoSat)?.ToString("dd/MM/yyyy")
                : "N/A",
                QuayLaiVaGioiThieu = item.QuayLaiVaGioiThieu,
                YKienKhac = item.YKienKhac,
                IdIN_MauKhaoSat = item.IdIN_MauKhaoSat,
                IdIN_ThongTinNguoiBenh = item.IdIN_ThongTinNguoiBenh
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
                    mks.""TenMauKhaoSat"",
                    mks.""IdOUT_MauKhaoSat"",
                    nbenh.""IdOUT_ThongTinNguoiBenh""
                FROM 
                    ""OUT_DanhGia"" dg
                LEFT JOIN 
                    ""OUT_MauKhaoSat"" mks ON dg.""IdOUT_MauKhaoSat"" = mks.""IdOUT_MauKhaoSat""
                LEFT JOIN 
                    ""OUT_ThongTinNguoiBenh"" nbenh ON dg.""IdOUT_ThongTinNguoiBenh"" = nbenh.""IdOUT_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""OUT_ThongTinYKienKhac"" ykkhac ON nbenh.""IdOUT_ThongTinNguoiBenh"" = ykkhac.""IdOUT_ThongTinNguoiBenh""
                WHERE 
                    mks.""HienThi"" = true 
                    AND mks.""Xoa"" = false
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
                    mks.""TenMauKhaoSat"",
                    mks.""IdOUT_MauKhaoSat"",
                    nbenh.""IdOUT_ThongTinNguoiBenh""
                FROM 
                    ""OUT_DanhGia"" dg
                LEFT JOIN 
                    ""OUT_MauKhaoSat"" mks ON dg.""IdOUT_MauKhaoSat"" = mks.""IdOUT_MauKhaoSat""
                LEFT JOIN 
                    ""OUT_ThongTinNguoiBenh"" nbenh ON dg.""IdOUT_ThongTinNguoiBenh"" = nbenh.""IdOUT_ThongTinNguoiBenh""
                LEFT JOIN 
                    ""OUT_ThongTinYKienKhac"" ykkhac ON nbenh.""IdOUT_ThongTinNguoiBenh"" = ykkhac.""IdOUT_ThongTinNguoiBenh""
                WHERE 
                    mks.""HienThi"" = true 
                    AND mks.""Xoa"" = false
                    AND mks.""TenMauKhaoSat"" = @surveyName
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
                NgayKhaoSat = item.NgayKhaoSat != null
                ? ((DateTime?)item.NgayKhaoSat)?.ToString("dd/MM/yyyy")
                : "N/A",
                QuayLaiVaGioiThieu = item.QuayLaiVaGioiThieu,
                YKienKhac = item.YKienKhac,
                IdOUT_MauKhaoSat = item.IdOUT_MauKhaoSat,
                IdOUT_ThongTinNguoiBenh = item.IdOUT_ThongTinNguoiBenh
            });

            return Json(new { data = formattedData });
        }
    }




    public class CauHoiViewModel
    {
        public string? MaCauHoi { get; set; }   // Mã của câu hỏi
        public string? NoiDung { get; set; }    // Nội dung của câu hỏi
        public int MucDanhGia { get; set; }
        public int DanhGia { get; set; }    // Nội dung của câu hỏi
    }

    public class NhomCauHoiViewModel
    {
        public string? MaNhomCauHoi { get; set; } // Mã nhóm câu hỏi, ví dụ: "D", "E"
        public string? NhomCauHoi { get; set; }   // Tên nhóm câu hỏi, ví dụ: "Thái độ ứng xử..."
        public int MucDanhGia { get; set; }
        public List<CauHoiViewModel>? CauHoi { get; set; } // Danh sách các câu hỏi thuộc nhóm
    }

    // ViewModel tổng thể chứa nhiều nhóm câu hỏi
    public class KhaoSatViewModel
    {
        public List<NhomCauHoiViewModel>? NhomCauHois { get; set; } // Danh sách các nhóm câu hỏi
    }

    [HttpPost]
    public async Task<ActionResult> GetEvaluates(int IdIN_MauKhaoSat, int IdIN_ThongTinNguoiBenh)
    {
        // Retrieve the survey and its details
        var mauKhaoSat = await _appDbContext.IN_MauKhaoSat
            .Where(n => n.IdIN_MauKhaoSat == IdIN_MauKhaoSat)
            .Select(n => new
            {
                n.NhomCauHoiKhaoSat,
                n.CauHoiKhaoSat,
                n.MucDanhGia // Retrieve the evaluation levels (array of integers)
            })
            .FirstOrDefaultAsync();

        // Check for null values
        var nhomCauHoiList = mauKhaoSat?.NhomCauHoiKhaoSat ?? new string[0];
        var cauHoiList = mauKhaoSat?.CauHoiKhaoSat ?? new string[0];
        var mucDanhGiaList = mauKhaoSat?.MucDanhGia ?? new int[0];

        // Retrieve all question groups based on the IDs
        var nhomCauHoi = await _appDbContext.IN_NhomCauHoiKhaoSat
            .Where(n => nhomCauHoiList.Contains(n.TieuDe))
            .Select(n => new
            {
                n.TieuDe,
                n.NoiDung
            })
            .ToListAsync();

        // Retrieve all questions based on the IDs
        var cauHoi = await _appDbContext.IN_CauHoiKhaoSat
            .Where(c => cauHoiList.Contains(c.TieuDeCauHoi))
            .Select(c => new
            {
                c.TieuDeCauHoi,
                c.CauHoi,
            })
            .ToListAsync();

        // Retrieve evaluation scores as a list
        var evaluations = await _appDbContext.IN_DanhGia
            .Where(d => d.IdIN_ThongTinNguoiBenh == IdIN_ThongTinNguoiBenh)
            .Select(d => d.DanhGia)
            .FirstOrDefaultAsync();

        evaluations = evaluations ?? new int[0];

        // Initialize an index to map evaluations to each question
        int evalIndex = 0;

        // Map data to ViewModel
        var questionGroups = nhomCauHoi.Select((n, index) => new NhomCauHoiViewModel
        {
            MaNhomCauHoi = n.TieuDe, // Assuming MaNhomCauHoi should be TieuDe
            NhomCauHoi = n.NoiDung,  // Assuming NhomCauHoi should be NoiDung
            MucDanhGia = mucDanhGiaList.Length > index ? mucDanhGiaList[index] : 5, // Use the MucDanhGia from the corresponding index
            CauHoi = cauHoi.Where(c => c.TieuDeCauHoi.StartsWith(n.TieuDe)) // Adjust as needed
                           .Select(c => new CauHoiViewModel
                           {
                               MaCauHoi = c.TieuDeCauHoi,  // MaCauHoi is TieuDeCauHoi
                               NoiDung = c.CauHoi,          // Content of the question
                               DanhGia = evalIndex < evaluations.Length ? evaluations[evalIndex++] : 0, // Assign evaluation or 0 if not available
                               MucDanhGia = mucDanhGiaList.Length > index ? mucDanhGiaList[index] : 5 // Use the same MucDanhGia as parent group
                           })
                           .ToList()
        }).ToList();

        var viewModel = new KhaoSatViewModel
        {
            NhomCauHois = questionGroups
        };

        // Return the data as JSON
        return Json(viewModel);
    }


    [HttpPost]
    public async Task<ActionResult> OUT_GetEvaluates(int IdOUT_MauKhaoSat, int IdOUT_ThongTinNguoiBenh)
    {
        // Retrieve question groups as string array
        var nhomCauHoiIds = await _appDbContext.OUT_MauKhaoSat
            .Where(n => n.IdOUT_MauKhaoSat == IdOUT_MauKhaoSat)
            .Select(n => n.NhomCauHoiKhaoSat)
            .FirstOrDefaultAsync();

        // Retrieve questions as string array
        var cauHoiIds = await _appDbContext.OUT_MauKhaoSat
            .Where(n => n.IdOUT_MauKhaoSat == IdOUT_MauKhaoSat)
            .Select(n => n.CauHoiKhaoSat)
            .FirstOrDefaultAsync();

        // Check for null values
        var nhomCauHoiList = nhomCauHoiIds ?? new string[0];
        var cauHoiList = cauHoiIds ?? new string[0];

        // Retrieve all question groups based on the IDs
        var nhomCauHoi = await _appDbContext.OUT_NhomCauHoiKhaoSat
            .Where(n => nhomCauHoiList.Contains(n.TieuDe))
            .Select(n => new
            {
                n.TieuDe,
                n.NoiDung
            })
            .ToListAsync();

        // Retrieve all questions based on the IDs
        var cauHoi = await _appDbContext.OUT_CauHoiKhaoSat
            .Where(c => cauHoiList.Contains(c.TieuDeCauHoi))
            .Select(c => new
            {
                c.TieuDeCauHoi,
                c.CauHoi,
            })
            .ToListAsync();
        // Retrieve evaluation scores as a list
        var evaluations = await _appDbContext.OUT_DanhGia
            .Where(d => d.IdOUT_ThongTinNguoiBenh == IdOUT_ThongTinNguoiBenh)
            .Select(d => d.DanhGia)
            .FirstOrDefaultAsync();


        evaluations = evaluations ?? new int[0];

        // Initialize an index to map evaluations to each question
        int evalIndex = 0;

        // Map data to ViewModel
        var questionGroups = nhomCauHoi.Select(n => new NhomCauHoiViewModel
        {
            MaNhomCauHoi = n.TieuDe, // Assuming MaNhomCauHoi should be TieuDe
            NhomCauHoi = n.NoiDung,  // Assuming NhomCauHoi should be NoiDung
            CauHoi = cauHoi.Where(c => c.TieuDeCauHoi.StartsWith(n.TieuDe)) // Adjust as needed
                           .Select(c => new CauHoiViewModel
                           {
                               MaCauHoi = c.TieuDeCauHoi,  // MaCauHoi is TieuDeCauHoi
                               NoiDung = c.CauHoi,          // Content of the question
                               DanhGia = evalIndex < evaluations.Length ? evaluations[evalIndex++] : 0 // Assign evaluation or 0 if not available
                           })
                           .ToList()
        }).ToList();

        var viewModel = new KhaoSatViewModel
        {
            NhomCauHois = questionGroups
        };

        // Return the data as JSON
        return Json(viewModel);
    }



}