using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using feedBackMvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using feedBackMvc.Helpers;
using System;
using Microsoft.Extensions.Logging;

public class ManageInfoSurrveierController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly ILogger<ManageInfoSurrveierController> _logger;

    public ManageInfoSurrveierController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<ManageInfoSurrveierController> logger)
    {
        _appDbContext = appDbContext;
        _jwtTokenHelper = jwtTokenHelper;
        _logger = logger;
    }
    [Route("ManageInfoSurrveier/GetManageInfoSurrveiers")]
    public async Task<IActionResult> GetManageInfoSurrveier()
    {
        var model = new QL_ThongTinNguoiKhaoSatViewModel();
        model.IN_MauKhaoSatList = await _appDbContext.IN_MauKhaoSat.ToListAsync();
        model.OUT_MauKhaoSatList = await _appDbContext.OUT_MauKhaoSat.ToListAsync();


        // Query for IN_ThongTinNguoiKhaoSat
        var queryIn = @"
        SELECT 
            mks.""TenMauKhaoSat"",
            nbenh.""SoDienThoai"",
            chung.""TenBenhVien"", 
            chung.""NgayDienPhieu"" AS ""NgayKhaoSat"",
            chung.""NguoiTraLoi"",
            nbenh.""GioiTinh"", 
            nbenh.""Tuoi"", 
            nbenh.""SoNgayNamVien"", 
            nbenh.""CoSuDungBHYT"",
            chung.""TenKhoa""
        FROM 
            ""IN_DanhGia"" dg
        LEFT JOIN 
            ""IN_MauKhaoSat"" mks ON dg.""IdIN_MauKhaoSat"" = mks.""IdIN_MauKhaoSat""
        LEFT JOIN 
            ""IN_ThongTinNguoiBenh"" nbenh ON dg.""IdIN_ThongTinNguoiBenh"" = nbenh.""IdIN_ThongTinNguoiBenh""
        LEFT JOIN 
            ""IN_ThongTinChung"" chung ON nbenh.""IdIN_ThongTinNguoiBenh"" = chung.""IdIN_ThongTinNguoiBenh""
        ORDER BY 
            mks.""TenMauKhaoSat"", chung.""NgayDienPhieu"" ASC";


        model.IN_TTNguoiKhaoSat = await ExecuteQuery(queryIn, MapToInThongTinNguoiKhaoSat);

        // Query for OUT_ThongTinNguoiKhaoSat
        var queryOut = @"
        SELECT 
            mks.""TenMauKhaoSat"",
            nbenh.""SoDienThoai"",
            chung.""TenBenhVien"", 
            chung.""NgayDienPhieu"" AS ""NgayKhaoSat"",
            chung.""NguoiTraLoi"",
            nbenh.""GioiTinh"", 
            nbenh.""Tuoi"", 
            nbenh.""SoNgayNamVien"", 
            nbenh.""CoSuDungBHYT"",
            chung.""TenKhoa""
        FROM 
            ""OUT_DanhGia"" dg
        LEFT JOIN 
            ""OUT_MauKhaoSat"" mks ON dg.""IdOUT_MauKhaoSat"" = mks.""IdOUT_MauKhaoSat""
        LEFT JOIN 
            ""OUT_ThongTinNguoiBenh"" nbenh ON dg.""IdOUT_ThongTinNguoiBenh"" = nbenh.""IdOUT_ThongTinNguoiBenh""
        LEFT JOIN 
            ""OUT_ThongTinChung"" chung ON nbenh.""IdOUT_ThongTinNguoiBenh"" = chung.""IdOUT_ThongTinNguoiBenh""
        ORDER BY 
            mks.""TenMauKhaoSat"", chung.""NgayDienPhieu"" ASC";

        model.OUT_TTNguoiKhaoSat = await ExecuteQuery(queryOut, MapToOutThongTinNguoiKhaoSat);

        // Return the model to the view
        return PartialView("_QL_ThongTinNguoiKhaoSat", model);
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

    private QL_ThongTinNguoiKhaoSatViewModel.IN_ThongTinNguoiKhaoSat MapToInThongTinNguoiKhaoSat(System.Data.Common.DbDataReader reader)
    {
        return new QL_ThongTinNguoiKhaoSatViewModel.IN_ThongTinNguoiKhaoSat
        {
            TenBenhVien = reader.GetString(reader.GetOrdinal("TenBenhVien")),
            NgayKhaoSat = reader.GetDateTime(reader.GetOrdinal("NgayKhaoSat")),
            SoDienThoai = reader.GetString(reader.GetOrdinal("SoDienThoai")),
            NguoiTraLoi = reader.GetString(reader.GetOrdinal("NguoiTraLoi")),
            GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
            Tuoi = reader.IsDBNull(reader.GetOrdinal("Tuoi")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Tuoi")),
            SoNgayNamVien = reader.IsDBNull(reader.GetOrdinal("SoNgayNamVien")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SoNgayNamVien")),
            CoSuDungBHYT = reader.IsDBNull(reader.GetOrdinal("CoSuDungBHYT")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("CoSuDungBHYT")),
            TenKhoa = reader.IsDBNull(reader.GetOrdinal("TenKhoa")) ? null : reader.GetString(reader.GetOrdinal("TenKhoa")),
            Ten_IN_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("TenMauKhaoSat")) ? null : reader.GetString(reader.GetOrdinal("TenMauKhaoSat"))
        };
    }

    private QL_ThongTinNguoiKhaoSatViewModel.OUT_ThongTinNguoiKhaoSat MapToOutThongTinNguoiKhaoSat(System.Data.Common.DbDataReader reader)
    {
        return new QL_ThongTinNguoiKhaoSatViewModel.OUT_ThongTinNguoiKhaoSat
        {
            TenBenhVien = reader.GetString(reader.GetOrdinal("TenBenhVien")),
            NgayKhaoSat = reader.GetDateTime(reader.GetOrdinal("NgayKhaoSat")),
            SoDienThoai = reader.GetString(reader.GetOrdinal("SoDienThoai")),
            NguoiTraLoi = reader.IsDBNull(reader.GetOrdinal("NguoiTraLoi")) ? null : reader.GetString(reader.GetOrdinal("NguoiTraLoi")),
            GioiTinh = reader.GetString(reader.GetOrdinal("GioiTinh")),
            Tuoi = reader.IsDBNull(reader.GetOrdinal("Tuoi")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Tuoi")),
            SoNgayNamVien = reader.IsDBNull(reader.GetOrdinal("SoNgayNamVien")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SoNgayNamVien")),
            CoSuDungBHYT = reader.IsDBNull(reader.GetOrdinal("CoSuDungBHYT")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("CoSuDungBHYT")),
            TenKhoa = reader.IsDBNull(reader.GetOrdinal("TenKhoa")) ? null : reader.GetString(reader.GetOrdinal("TenKhoa")),
            Ten_OUT_MauKhaoSat = reader.IsDBNull(reader.GetOrdinal("TenMauKhaoSat")) ? null : reader.GetString(reader.GetOrdinal("TenMauKhaoSat"))
        };
    }

}
