using Microsoft.AspNetCore.Mvc;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace feedBackMvc.Controllers
{
    public class QRCodeController : Controller
    {
        public IActionResult GenerateQRCode_IN_DanhGiaKhaoSat(int Id)
        {
            string ipAddress = "192.168.9.120";
            string port = "4000";
            string url = $"http://{ipAddress}:{port}/DanhGia/IN_DanhGiaKhaoSat?Id={Id}";
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("Invalid URL.");
            }

            try
            {
                var barcodeWriter = new BarcodeWriter<Bitmap>
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 500,
                        Width = 500,
                        Margin = 1
                    },
                    Renderer = new BitmapRenderer()
                };

                using (var bitmap = barcodeWriter.Write(url))
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        var qrCodeImage = stream.ToArray();
                        // Convert to base64
                        string base64Image = Convert.ToBase64String(qrCodeImage);
                        return Content("data:image/png;base64," + base64Image);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public IActionResult GenerateQRCode_OUT_DanhGiaKhaoSat(int Id)
        {
            string ipAddress = "192.168.9.120"; // Địa chỉ IP local của bạn
            string port = "4000"; // Port ứng dụng của bạn
            string url = $"http://{ipAddress}:{port}/DanhGia/OUT_DanhGiaKhaoSat?Id={Id}";
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("Invalid URL.");
            }

            try
            {
                var barcodeWriter = new BarcodeWriter<Bitmap>
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 500,
                        Width = 500,
                        Margin = 1
                    },
                    Renderer = new BitmapRenderer()
                };

                using (var bitmap = barcodeWriter.Write(url))
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        var qrCodeImage = stream.ToArray();
                        // Convert to base64
                        string base64Image = Convert.ToBase64String(qrCodeImage);
                        return Content("data:image/png;base64," + base64Image);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public IActionResult GenerateQRCode_ORTHER_DanhGiaKhaoSat(int Id)
        {
            string ipAddress = "192.168.9.120"; // Địa chỉ IP local của bạn
            string port = "4000"; // Port ứng dụng của bạn
            string url = $"http://{ipAddress}:{port}/DanhGia/ORTHER_DanhGiaKhaoSat?Id={Id}";
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("Invalid URL.");
            }

            try
            {
                var barcodeWriter = new BarcodeWriter<Bitmap>
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 500,
                        Width = 500,
                        Margin = 1
                    },
                    Renderer = new BitmapRenderer()
                };

                using (var bitmap = barcodeWriter.Write(url))
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        var qrCodeImage = stream.ToArray();
                        // Convert to base64
                        string base64Image = Convert.ToBase64String(qrCodeImage);
                        return Content("data:image/png;base64," + base64Image);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
