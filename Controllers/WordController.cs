using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using feedBackMvc.Models; // Ensure this namespace is correct
using feedBackMvc.Helpers;
using System.IO;
using System.Linq;

namespace feedBackMvc.Controllers
{
    public class WordController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly ILogger<WordController> _logger;

        public WordController(AppDbContext appDbContext, JwtTokenHelper jwtTokenHelper, ILogger<WordController> logger)
        {
            _appDbContext = appDbContext;
            _jwtTokenHelper = jwtTokenHelper;
            _logger = logger;
        }
        public int[] GetMucDanhGiaById(int id)
        {
            // Lấy MucDanhGia dựa trên ID
            var mucDanhGia = _appDbContext.IN_MauKhaoSat
                                     .Where(m => m.IdIN_MauKhaoSat == id)
                                     .Select(m => m.MucDanhGia) // Giả sử MucDanhGia là kiểu int[]
                                     .FirstOrDefault();
            return mucDanhGia; // Trả về mảng MucDanhGia

        }
        public List<IN_NhomCauHoiKhaoSat> GetNhomCauHoiByTitles(string[] nhomCauHoi)
        {
            // Lấy ra các nhóm câu hỏi tương ứng với tiêu đề
            var nhomCauHoiList = _appDbContext.IN_NhomCauHoiKhaoSat
                .Where(n => nhomCauHoi.Contains(n.TieuDe))
                .ToList();
            return nhomCauHoiList;
        }
        public List<IN_CauHoiKhaoSat> GetCauHoiByTitles(string[] cauHoi)
        {
            // Lấy ra các nhóm câu hỏi tương ứng với tiêu đề
            var cauHoiList = _appDbContext.IN_CauHoiKhaoSat
                .Where(n => cauHoi.Contains(n.TieuDeCauHoi))
                .ToList();
            return cauHoiList;
        }
        public IActionResult ExportToWord(int id)
        {
            // Lấy dữ liệu khảo sát từ database dựa trên id
            var IN_MauKhaoSat = _appDbContext.IN_MauKhaoSat.Find(id);
            if (IN_MauKhaoSat != null)
            {
                string[] nhomCauHoi = IN_MauKhaoSat?.NhomCauHoiKhaoSat ?? Array.Empty<string>();
                string[] cauHoi = IN_MauKhaoSat?.CauHoiKhaoSat ?? Array.Empty<string>();
                int[] mucDanhGia = IN_MauKhaoSat?.MucDanhGia ?? Array.Empty<int>();
                List<IN_NhomCauHoiKhaoSat> nhomCauHois = GetNhomCauHoiByTitles(nhomCauHoi);
                List<IN_CauHoiKhaoSat> cauHois = GetCauHoiByTitles(cauHoi);

            }


            if (IN_MauKhaoSat == null)
            {
                return NotFound();
            }

            // Tạo một MemoryStream để lưu trữ file Word
            using (var ms = new MemoryStream())
            {
                // Tạo tài liệu Word
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    // Add main document part
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = new Body();


                    SectionProperties sectionProperties = new SectionProperties();
                    PageMargin pageMargin = new PageMargin()
                    {
                        Top = (int)(0.79 * 1440),    // 0.79 inch, chuyển đổi sang Twips (1 inch = 1440 Twips)
                        Right = (int)(0.79 * 1440),  // 0.79 inch
                        Bottom = (int)(0.79 * 1440), // 0.79 inch
                        Left = (int)(0.98 * 1440)    // 0.98 inch
                    };
                    sectionProperties.Append(pageMargin);
                    body.Append(sectionProperties);  // Thêm thuộc tính section vào body

                    CreateSurveyDocument(body);

                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

                    AddFeedbackSection(body);
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

                    AddSurveyQuestions(body);
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

                    AddPatientInfoSection(body);
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

                    AddServiceEvaluationSection(body);
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

                    int[] mucDanhGia = GetMucDanhGiaById(id); // Gọi hàm để lấy MucDanhGia
                    // Kiểm tra xem mảng có giá trị 3 không
                    if (mucDanhGia != null && mucDanhGia.Contains(3))
                    {
                        AddServiceEvaluationSection2(body);
                        body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống
                    }
                    // Tạo bảng câu hỏi
                    CreateQuestionTable(body, IN_MauKhaoSat);
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống
                    CreateFinalEvaluationTable(body);
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống


                    // Thêm phần chữ cảm ơn
                    Paragraph thankYouParagraph = new Paragraph();
                    Run thankYouRun = new Run();
                    thankYouRun.Append(new Text("XIN TRÂN TRỌNG CẢM ƠN ÔNG/BÀ"));
                    SetRunProperties(thankYouRun, true); // Đặt độ đậm cho dòng chữ cảm ơn
                    thankYouParagraph.Append(thankYouRun);
                    thankYouParagraph.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center }); // Canh giữa nếu cần
                    body.Append(thankYouParagraph);
                    body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

                    // Thêm body vào tài liệu
                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }
                // Đặt tên file động dựa trên TenMauKhaoSat
                string fileName = $"{IN_MauKhaoSat.TenMauKhaoSat}.docx";

                // Trả về file với tên file động
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

            }
        }
        private void CreateSurveyDocument(Body body)
        {
            // Tạo bảng
            Table table = new Table();

            // Tạo thuộc tính cho bảng
            TableProperties tableProperties = new TableProperties();
            TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
            tableProperties.Append(tableWidth);
            table.Append(tableProperties);

            // Tạo hàng đầu tiên
            TableRow headerRow = new TableRow();

            // Cột 1
            TableCell headerCell1 = new TableCell();
            headerCell1.Append(CreateFormattedParagraph("BỘ Y TẾ"));
            headerCell1.Append(new Paragraph());
            headerCell1.Append(CreateFormattedParagraph("MẪU SỐ 1"));
            headerCell1.Append(new Paragraph());
            headerCell1.Append(CreateFormattedParagraph("(RÚT GỌN)"));
            headerRow.Append(headerCell1);

            // Cột 2 (ô trống)
            TableCell headerCell2 = new TableCell();
            headerRow.Append(headerCell2);

            // Cột 3
            TableCell headerCell3 = new TableCell();

            // Tạo hàng nhập (inputRow)
            TableRow inputRow = new TableRow();

            // Hàm tạo ô vuông
            Func<TableCell> createSquareInputCell = () =>
            {
                TableCell inputCell = new TableCell();
                TableCellProperties cellProperties = new TableCellProperties();
                cellProperties.Append(new TableCellWidth() { Width = "500", Type = TableWidthUnitValues.Pct });

                // Đường viền
                cellProperties.Append(new TableCellBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 15 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 15 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 15 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 15 }
                ));
                cellProperties.Append(new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center });

                // Văn bản cho ô (sử dụng ký tự Unicode)
                Run run = new Run(new Text("\u2610")); // Unicode cho ô vuông trống
                Paragraph p = new Paragraph(run);
                inputCell.Append(cellProperties);
                inputCell.Append(p);

                return inputCell;
            };

            // Thêm 5 ô vuông bên trái
            for (int i = 0; i < 5; i++)
            {
                inputRow.Append(createSquareInputCell());
            }

            // Thêm ô trống ở giữa
            TableCell emptyCell = new TableCell(new Paragraph(new Run(new Text(""))));
            inputRow.Append(emptyCell);

            // Thêm 5 ô vuông bên phải
            for (int i = 0; i < 5; i++)
            {
                inputRow.Append(createSquareInputCell());
            }

            // Thêm hàng nhập vào headerCell3
            headerCell3.Append(inputRow);
            headerCell3.Append(new Paragraph()); // Thêm đoạn văn trống
            headerCell3.Append(CreateFormattedParagraph("PHIẾU KHẢO SÁT Ý KIẾN NGƯỜI BỆNH NỘI TRÚ"));

            // Thêm cột 3 vào hàng đầu
            headerRow.Append(headerCell3);

            // Thêm hàng đầu vào bảng
            table.Append(headerRow);

            // Thêm bảng vào body
            body.Append(table);
        }

        private Paragraph CreateFormattedParagraph(string text)
        {
            Run run = new Run(new Text(text))
            {
                RunProperties = new RunProperties
                {
                    Bold = new Bold(), // In đậm
                    FontSize = new FontSize() { Val = "26" }, // Kích thước 13 (đơn vị 1/2 pt)
                    RunFonts = new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" } // Font Times New Roman
                }
            };

            return new Paragraph(run)
            {
                ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center }) // Canh giữa
            };
        }

        private void AddFeedbackSection(Body body)
        {
            // Thêm đoạn văn dưới tiêu đề
            Paragraph paraDescription = new Paragraph();
            Run runDescription = new Run();
            string descriptionText = "Nhằm mục tiêu nâng cao chất lượng khám, chữa bệnh, đáp ứng sự hài lòng người bệnh, Bộ Y tế và bệnh viện tổ chức khảo sát để tìm hiểu nguyện vọng người bệnh. Các ý kiến quý báu này sẽ giúp ngành y tế khắc phục khó khăn, từng bước cải tiến chất lượng để phục vụ người dân tốt hơn. Bộ Y tế bảo đảm giữ bí mật thông tin và không ảnh hưởng đến việc điều trị. Xin trân trọng cảm ơn!";
            runDescription.Append(new Text(descriptionText));
            SetRunProperties(runDescription, false, true); // Không in đậm nhưng in nghiêng
            paraDescription.Append(runDescription);
            body.Append(paraDescription);

        }

        private void SetRunProperties(Run run, bool isBold, bool isItalic = false)
        {
            RunProperties runProperties = new RunProperties();

            // Thiết lập độ đậm nếu isBold là true
            if (isBold)
            {
                runProperties.Append(new Bold());
            }

            // Thiết lập in nghiêng nếu isItalic là true
            if (isItalic)
            {
                runProperties.Append(new Italic());
            }

            // Thiết lập font chữ là "Times New Roman" và kích thước là 13
            runProperties.Append(new RunFonts() { Ascii = "Times New Roman", HighAnsi = "Times New Roman" });
            runProperties.Append(new FontSize() { Val = "26" }); // Font size 13pt = 26 half-points

            // Bạn có thể thiết lập thêm các thuộc tính khác tại đây
            run.PrependChild(runProperties);
        }
        private void AddSurveyQuestions(Body body)
        {
            // Thêm câu hỏi 1
            Paragraph paraQuestion1 = new Paragraph();
            Run runQuestion1 = new Run();
            runQuestion1.Append(new Text("1. Tên bệnh viện: . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .  . . ."));
            SetRunProperties(runQuestion1, false); // Không in đậm
            paraQuestion1.Append(runQuestion1);
            body.Append(paraQuestion1);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // Thêm câu hỏi 2
            Paragraph paraQuestion2 = new Paragraph();
            Run runQuestion2 = new Run();
            runQuestion2.Append(new Text("2. Ngày điền phiếu: . . . . . . . . . . . . . . . "));
            SetRunProperties(runQuestion2, false); // Không in đậm
            paraQuestion2.Append(runQuestion2);
            body.Append(paraQuestion2);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            Paragraph paraQuestion3 = new Paragraph();
            Run runQuestion3 = new Run();
            runQuestion3.Append(new Text("3. Người trả lời:\t"));
            SetRunProperties(runQuestion3, false); // Không in đậm

            // Tạo checkbox cho "Người bệnh"
            Run runCheckbox1 = new Run();
            runCheckbox1.Append(new Text("☐ Người bệnh\t"));  // Ô checkbox trống cho "Người bệnh"
            SetRunProperties(runCheckbox1, false);

            // Tạo checkbox cho "Người nhà"
            Run runCheckbox2 = new Run();
            runCheckbox2.Append(new Text("☐ Người nhà\t"));  // Ô checkbox trống cho "Người nhà"
            SetRunProperties(runCheckbox2, false);

            // Thêm các checkbox vào đoạn văn
            paraQuestion3.Append(runQuestion3);
            paraQuestion3.Append(runCheckbox1);
            paraQuestion3.Append(runCheckbox2);
            body.Append(paraQuestion3);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // Thêm câu hỏi 4
            Paragraph paraQuestion4 = new Paragraph();
            Run runQuestion4 = new Run();
            runQuestion4.Append(new Text("4. Tên khoa nằm điều trị trước ra viện: . . . . . . . . . . . . . . . . . . . . . . . . . . "));
            SetRunProperties(runQuestion4, false); // Không in đậm
            paraQuestion4.Append(runQuestion4);
            body.Append(paraQuestion4);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // Thêm câu hỏi 5
            Paragraph paraQuestion5 = new Paragraph();
            Run runQuestion5 = new Run();
            runQuestion5.Append(new Text("5. Mã khoa (do BV ghi): . . . . . . . . . . . . . . .  ."));
            SetRunProperties(runQuestion5, false); // Không in đậm
            paraQuestion5.Append(runQuestion5);
            body.Append(paraQuestion5);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống
        }
        private void AddPatientInfoSection(Body body)
        {
            // Thêm tiêu đề phần "THÔNG TIN NGƯỜI BỆNH"
            Paragraph paraTitle = new Paragraph();
            Run runTitle = new Run();
            runTitle.Append(new Text("THÔNG TIN NGƯỜI BỆNH"));
            SetRunProperties(runTitle, true); // In đậm
            paraTitle.Append(runTitle);
            body.Append(paraTitle);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // A1. Giới tính
            Paragraph paraQuestionA1 = new Paragraph();
            Run runQuestionA1 = new Run();
            runQuestionA1.Append(new Text("A1. Giới tính:\t"));
            SetRunProperties(runQuestionA1, false);

            Run runMaleCheckbox = new Run();
            runMaleCheckbox.Append(new Text("☐ Nam\t")); // Checkbox cho Nam
            SetRunProperties(runMaleCheckbox, false);

            Run runFemaleCheckbox = new Run();
            runFemaleCheckbox.Append(new Text("☐ Nữ\t")); // Checkbox cho Nữ
            SetRunProperties(runFemaleCheckbox, false);

            paraQuestionA1.Append(runQuestionA1);
            paraQuestionA1.Append(runMaleCheckbox);
            paraQuestionA1.Append(runFemaleCheckbox);
            body.Append(paraQuestionA1);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // A2. Tuổi
            Paragraph paraQuestionA2 = new Paragraph();
            Run runQuestionA2 = new Run();
            runQuestionA2.Append(new Text("A2. Tuổi: ..............  . .\t A3. Số di động (bắt buộc): .................................. "));
            SetRunProperties(runQuestionA2, false);
            paraQuestionA2.Append(runQuestionA2);
            body.Append(paraQuestionA2);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // A4. Tổng số ngày nằm viện
            Paragraph paraQuestionA4 = new Paragraph();
            Run runQuestionA4 = new Run();
            runQuestionA4.Append(new Text("A4. Tổng số ngày nằm viện: ............................. ngày"));
            SetRunProperties(runQuestionA4, false);
            paraQuestionA4.Append(runQuestionA4);
            body.Append(paraQuestionA4);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // A5. Ông/Bà có sử dụng thẻ BHYT cho lần điều trị này không?
            Paragraph paraQuestionA5 = new Paragraph();
            Run runQuestionA5 = new Run();
            runQuestionA5.Append(new Text("A5. Ông/Bà có sử dụng thẻ BHYT cho lần điều trị này không?\t"));
            SetRunProperties(runQuestionA5, false);

            Run runYesCheckbox = new Run();
            runYesCheckbox.Append(new Text("☐ Có\t")); // Checkbox cho Có
            SetRunProperties(runYesCheckbox, false);

            Run runNoCheckbox = new Run();
            runNoCheckbox.Append(new Text("☐ Không")); // Checkbox cho Không
            SetRunProperties(runNoCheckbox, false);

            paraQuestionA5.Append(runQuestionA5);
            paraQuestionA5.Append(runYesCheckbox);
            paraQuestionA5.Append(runNoCheckbox);
            body.Append(paraQuestionA5);
        }
        private void AddServiceEvaluationSection(Body body)
        {
            // Thêm tiêu đề
            Paragraph paraTitle = new Paragraph();
            Run runTitle = new Run();
            runTitle.Append(new Text("ĐÁNH GIÁ VIỆC SỬ DỤNG DỊCH VỤ Y TẾ"));
            SetRunProperties(runTitle, true); // In đậm
            paraTitle.Append(runTitle);
            body.Append(paraTitle);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // Thêm lời hướng dẫn
            Paragraph paraInstruction = new Paragraph();
            Run runInstruction = new Run();
            string instructionText = "Ông/Bà đánh dấu gạch chéo vào một số từ 1 đến 5, tương ứng với mức độ hài lòng hoặc nhận xét từ rất kém đến rất tốt cho từng câu hỏi dưới đây:";
            runInstruction.Append(new Text(instructionText));
            SetRunProperties(runInstruction, false); // Không in đậm
            paraInstruction.Append(runInstruction);
            body.Append(paraInstruction);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // Tạo bảng đánh giá
            Table table = new Table();

            // Tạo hàng đầu tiên chứa các số ① đến ⑤
            TableRow row1 = new TableRow();
            AddTableCell(row1, "① là:", true, "2500"); // Thêm chiều rộng
            AddTableCell(row1, "② là:", true, "2000"); // Thêm chiều rộng
            AddTableCell(row1, "③ là:", true, "1900"); // Thêm chiều rộng
            AddTableCell(row1, "④ là:", true, "1500"); // Thêm chiều rộng
            AddTableCell(row1, "⑤ là:", true, "1500"); // Thêm chiều rộng
            table.Append(row1);

            // Tạo hàng thứ hai chứa các mức độ hài lòng
            TableRow row2 = new TableRow();
            AddTableCell(row2, "Rất không hài lòng", false, "2500"); // Thêm chiều rộng
            AddTableCell(row2, "Không hài lòng", false, "2000"); // Thêm chiều rộng
            AddTableCell(row2, "Bình thường", false, "1900"); // Thêm chiều rộng
            AddTableCell(row2, "Hài lòng", false, "1500"); // Thêm chiều rộng
            AddTableCell(row2, "Rất hài lòng", false, "1500"); // Thêm chiều rộng
            table.Append(row2);
            // Thêm bảng vào body
            body.Append(table);
        }
        private void AddServiceEvaluationSection2(Body body)
        {
            // Thêm lời hướng dẫn
            Paragraph paraInstruction = new Paragraph();
            Run runInstruction = new Run();
            string instructionText = "Hoặc ông/Bà đánh dấu gạch chéo vào một số từ 1 đến 3, tương ứng với mức độ hài lòng hoặc nhận xét từ rất kém đến rất tốt cho từng câu hỏi dưới đây:";
            runInstruction.Append(new Text(instructionText));
            SetRunProperties(runInstruction, false); // Không in đậm
            paraInstruction.Append(runInstruction);
            body.Append(paraInstruction);
            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống

            // Tạo bảng đánh giá
            Table table = new Table();
            TableProperties tblProperties = new TableProperties();

            // Thêm thuộc tính căn giữa
            tblProperties.Append(new TableJustification() { Val = TableRowAlignmentValues.Center });
            table.AppendChild(tblProperties); // Thêm thuộc tính vào bảng

            // Tạo hàng đầu tiên chứa các số ① đến ⑤
            TableRow row1 = new TableRow();
            AddTableCell(row1, "① là:", true, "2500"); // Thêm chiều rộng
            AddTableCell(row1, "② là:", true, "2000"); // Thêm chiều rộng
            AddTableCell(row1, "③ là:", true, "1900"); // Thêm chiều rộng

            table.Append(row1);

            // Tạo hàng thứ hai chứa các mức độ hài lòng
            TableRow row2 = new TableRow();
            AddTableCell(row2, "Không hài lòng", false, "2500"); // Thêm chiều rộng
            AddTableCell(row2, "Bình thường", false, "2000"); // Thêm chiều rộng
            AddTableCell(row2, "Hài lòng", false, "1900"); // Thêm chiều rộng
            table.Append(row2);
            // Thêm bảng vào body
            body.Append(table);

            body.Append(new Paragraph(new Run(new Text("")))); // Dòng trống
        }

        // Hàm phụ trợ để thêm ô vào hàng bảng
        private void AddTableCell(TableRow row, string text, bool isBold, string width)
        {
            TableCell cell = new TableCell();

            // Đặt chiều rộng của ô
            TableCellProperties cellProperties = new TableCellProperties();
            TableCellWidth cellWidth = new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = width }; // Đơn vị Dxa (1/20 của điểm)
            cellProperties.Append(cellWidth);
            cell.Append(cellProperties);

            Paragraph para = new Paragraph();
            Run run = new Run();
            run.Append(new Text(text));
            SetRunProperties(run, isBold); // Đặt in đậm nếu cần
            para.Append(run);
            cell.Append(para);
            row.Append(cell);
        }
        private void CreateQuestionTable(Body body, IN_MauKhaoSat IN_MauKhaoSat)
        {
            string[] nhomCauHoi = IN_MauKhaoSat.NhomCauHoiKhaoSat ?? Array.Empty<string>();
            string[] cauHoi = IN_MauKhaoSat.CauHoiKhaoSat ?? Array.Empty<string>();
            int[] mucDanhGia = IN_MauKhaoSat.MucDanhGia ?? Array.Empty<int>(); // Lấy mức đánh giá cho mẫu khảo sát

            List<IN_NhomCauHoiKhaoSat> nhomCauHois = GetNhomCauHoiByTitles(nhomCauHoi);
            List<IN_CauHoiKhaoSat> cauHois = GetCauHoiByTitles(cauHoi);
            int k = 0;
            // Duyệt qua các nhóm câu hỏi
            foreach (var nhom in nhomCauHois)
            {
                // Create a new table
                Table table = new Table();

                // Define the borders for the table
                TableBorders tblBorders = new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 }
                );

                // Set the width of the table to 5000 (50% width in Pct)
                TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

                // Combine the width and borders into TableProperties
                TableProperties tableProperties = new TableProperties();
                tableProperties.Append(tableWidth);      // Add the width
                tableProperties.Append(tblBorders);      // Add the borders

                // Append the properties to the table
                table.AppendChild(tableProperties);

                // Tạo hàng tiêu đề cho nhóm câu hỏi
                TableRow headerRow = new TableRow();
                TableCell headerCell = new TableCell(new Paragraph(new Run(new Text($"{nhom.TieuDe}: {nhom.NoiDung}"))));
                SetCellFormatting(headerCell, true); // In đậm cho tiêu đề
                headerCell.Append(new TableCellProperties(new TableCellWidth() { Width = "5000", Type = TableWidthUnitValues.Pct }));
                headerRow.Append(headerCell);
                table.Append(headerRow);


                // Lọc các câu hỏi tương ứng với nhóm hiện tại
                var filteredCauHois = cauHois
                    .Where(c => c.IdIN_NhomCauHoiKhaoSat == nhom.IdIN_NhomCauHoiKhaoSat)
                    .ToList();

                // Thêm các câu hỏi vào bảng
                foreach (var CauHoi in filteredCauHois)
                {
                    TableRow row = new TableRow();

                    // Tạo ô cho tiêu đề câu hỏi
                    TableCell titleCell = new TableCell(new Paragraph(new Run(new Text(CauHoi.TieuDeCauHoi))));
                    SetCellFormatting(titleCell, false); // Định dạng bình thường
                    titleCell.Append(new TableCellProperties(new TableCellWidth() { Width = "300", Type = TableWidthUnitValues.Pct })); // Chiều rộng 500
                    row.Append(titleCell);

                    // Tạo ô cho nội dung câu hỏi
                    TableCell questionCell = new TableCell(new Paragraph(new Run(new Text(CauHoi.CauHoi))));
                    SetCellFormatting(questionCell, false); // Định dạng bình thường
                    questionCell.Append(new TableCellProperties(new TableCellWidth() { Width = "3700", Type = TableWidthUnitValues.Pct })); // Chiều rộng 3500
                    row.Append(questionCell);

                    // Định nghĩa mảng các ký tự đánh giá đặc biệt
                    string[] SpecialRatingChars = { "①", "②", "③", "④", "⑤" };
                    // Trong phần code hiển thị mức đánh giá
                    // Hiển thị mức đánh giá dựa vào mucDanhGia
                    int ratingLevel = mucDanhGia[k]; // Lấy mức đánh giá cho nhóm câu hỏi hiện tại
                    string ratingText = string.Join(" ", SpecialRatingChars.Take(ratingLevel)); // Tạo chuỗi từ ① đến mức đánh giá
                    TableCell ratingCell = new TableCell(new Paragraph(new Run(new Text(ratingText))));
                    SetCellFormatting(ratingCell, false); // Định dạng bình thường
                    ratingCell.Append(new TableCellProperties(new TableCellWidth() { Width = "1000", Type = TableWidthUnitValues.Pct })); // Chiều rộng 1000
                    row.Append(ratingCell);
                    table.Append(row);


                }
                k++;

                // Thêm bảng vào tài liệu
                body.Append(table);
                body.Append(new Paragraph()); // Thêm dòng trống giữa các nhóm

            }
        }

        // Phương thức SetCellFormatting
        private void SetCellFormatting(TableCell cell, bool isBold)
        {
            // Thiết lập định dạng cho ô
            // Thiết lập khoảng cách giữa các dòng
            var paragraphProperties = new ParagraphProperties();
            var spacingBetweenLines = new SpacingBetweenLines()
            {
                After = "800",  // Khoảng cách sau đoạn (tính bằng Twips, 200 Twips = 10pt)
                Before = "800"  // Khoảng cách trước đoạn
            };
            paragraphProperties.Append(spacingBetweenLines);
            var paragraph = new Paragraph();
            var run = new Run(new Text(cell.InnerText)); // Lấy văn bản từ ô hiện tại
            Paragraph emptyParagraph = new Paragraph(new Run());

            SetRunProperties(run, isBold);
            paragraph.Append(run);
            // Xóa văn bản ban đầu trong ô
            cell.RemoveAllChildren();
            cell.Append(paragraph);
            cell.Append(emptyParagraph);
        }
        private void CreateFinalEvaluationTable(Body body)
        {
            // Create a new table
            Table table = new Table();

            // Define the borders for the table
            TableBorders tblBorders = new TableBorders(
                new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 },
                new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 1 }
            );

            // Set the width of the table to 5000 (50% width in Pct)
            TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

            // Combine the width and borders into TableProperties
            TableProperties tableProperties = new TableProperties();
            tableProperties.Append(tableWidth);      // Add the width
            tableProperties.Append(tblBorders);      // Add the borders

            // Append the properties to the table
            table.AppendChild(tableProperties);

            // G1 row
            TableRow row1 = new TableRow();

            // Cell 1 - Width 6% (600 Pct)
            TableCell cell1_1 = new TableCell(new Paragraph(new Run(new Text("G1"))));
            SetCellFormatting(cell1_1, true); // Định dạng bình thường
            TableCellProperties cell1_1Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "300" }); // Chiều rộng là 6%
            cell1_1.Append(cell1_1Props);

            // Cell 2 - Width 74% (7400 Pct)
            TableCell cell1_2 = new TableCell();

            // Tạo một Paragraph mới để chứa tất cả các Run
            Paragraph paragraph = new Paragraph();

            // Tạo và thêm các Run vào Paragraph
            var cell1_2_run1 = new Run(new Text("Đánh giá chung, bệnh viện đã "));
            SetRunProperties(cell1_2_run1, false, false);
            paragraph.Append(cell1_2_run1);
            paragraph.Append(new Run(new Text("\u00A0")));
            var cell1_2_runBold = new Run(new Text(" đáp ứng được bao nhiêu % so với mong đợi "))
            {
                RunProperties = new RunProperties(new Bold())
            };
            SetRunProperties(cell1_2_runBold, true, false);
            paragraph.Append(cell1_2_runBold);
            paragraph.Append(new Run(new Text("\u00A0")));
            var cell1_2_run3 = new Run(new Text(" của Ông/Bà trước khi nằm viện? "));
            SetRunProperties(cell1_2_run3, false, false);
            paragraph.Append(cell1_2_run3);

            // Thêm Paragraph vào ô
            cell1_2.Append(paragraph);
            cell1_2.Append(new Paragraph(new Run()));


            // Tạo một Paragraph mới cho câu giải thích với khoảng cách dòng
            Paragraph explanationParagraph = new Paragraph(new Run(new Text("(điền số từ 0% đến 100% hoặc có thể điền trên 100% nếu bệnh viện điều trị tốt, vượt quá mong đợi của Ông/Bà)")))
            {
                ParagraphProperties = new ParagraphProperties(
                    new SpacingBetweenLines() { After = "200" } // Khoảng cách dòng, ví dụ 200 (điểm)
                )
            };
            SetRunProperties(explanationParagraph.Elements<Run>().First(), false, true); // Định dạng in nghiêng cho Run
            // Thêm Paragraph giải thích vào ô
            cell1_2.Append(explanationParagraph);

            TableCellProperties cell1_2Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "3700" }); // Chiều rộng là 74%
            cell1_2.Append(cell1_2Props);

            // Cell 3 - Width 20% (2000 Pct)
            TableCell cell1_3 = new TableCell(new Paragraph(new Run(new Text("%"))));
            SetCellFormatting(cell1_3, false);
            TableCellProperties cell1_3Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "1000" }); // Chiều rộng là 20%
            cell1_3.Append(cell1_3Props);

            // Append cells to the row
            row1.Append(cell1_1, cell1_2, cell1_3);

            // Append row to the table
            table.Append(row1);


            // G2 row
            TableRow row2 = new TableRow();


            TableCell cell2_1 = new TableCell(new Paragraph(new Run(new Text("G2"))));
            SetCellFormatting(cell2_1, true); // Định dạng bình thường
            TableCellProperties cell2_1Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "300" });
            cell2_1.Append(cell2_1Props);

            TableCell cell2_2 = new TableCell();
            cell2_2.Append(new Paragraph(new Run(new Text("Nếu có nhu cầu khám, chữa những bệnh tương tự, Ông/Bà có quay trở lại hoặc giới thiệu cho người khác đến không?"))));
            SetCellFormatting(cell2_2, false); // Định dạng bình thường
            TableCellProperties cell2_2Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "1700" });
            cell2_2.Append(cell2_2Props);


            TableCell cell2_3 = new TableCell();
            // Thêm câu hỏi và khoảng trống
            Run run1 = new Run(new Text("1. Chắc chắn không bao giờ quay lại"));
            SetRunProperties(run1, false, false);
            cell2_3.Append(new Paragraph(run1));
            cell2_3.Append(new Paragraph(new Run()));

            Run run2 = new Run(new Text("2. Không muốn quay lại nhưng có ít lựa chọn khác"));
            SetRunProperties(run2, false, false);
            cell2_3.Append(new Paragraph(run2));
            cell2_3.Append(new Paragraph(new Run()));

            Run run3 = new Run(new Text("3. Muốn chuyển tuyến sang bệnh viện khác"));
            SetRunProperties(run3, false, false);
            cell2_3.Append(new Paragraph(run3));
            cell2_3.Append(new Paragraph(new Run()));

            Run run4 = new Run(new Text("4. Có thể sẽ quay lại"));
            SetRunProperties(run4, false, false);
            cell2_3.Append(new Paragraph(run4));
            cell2_3.Append(new Paragraph(new Run()));

            TableCellProperties cell2_3Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "3000" });
            cell2_3.Append(cell2_3Props);

            // Append cells to the row
            row2.Append(cell2_1, cell2_2, cell2_3);

            // Append row to the table
            table.Append(row2);


            // H row
            TableRow row3 = new TableRow();


            TableCell cell3_1 = new TableCell(new Paragraph(new Run(new Text("H"))));
            SetRunProperties(cell3_1.Elements<Paragraph>().First().Elements<Run>().First(), false, false); // Định dạng ô H
            SetCellFormatting(cell3_1, true);
            TableCellProperties cell3_1Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "300" }); // Chiều rộng là 6%
            cell3_1.Append(cell3_1Props);


            TableCell cell3_2 = new TableCell();
            Run run = new Run(new Text("Ông/Bà có ý kiến gì khác, xin ghi rõ?"));
            SetRunProperties(run, false, false);
            cell3_2.Append(new Paragraph(run));
            cell3_2.Append(new Paragraph(new Run()));
            SetRunProperties(cell3_2.Elements<Paragraph>().First().Elements<Run>().First(), false, false); // Định dạng ô câu hỏi
            TableCellProperties cell3_2Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "1700" }); // Chiều rộng là 74%
            cell3_2.Append(cell3_2Props);




            TableCell cell3_3 = new TableCell(new Paragraph(new Run(new Text("             ")))); // Một khoảng trắng, có thể thay bằng một TextBox nếu cần
            SetRunProperties(cell3_3.Elements<Paragraph>().First().Elements<Run>().First(), false, false); // Định dạng ô trống
            TableCellProperties cell3_3Props = new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Pct, Width = "3000" }); // Chiều rộng là 20%

            cell3_3.Append(cell3_3Props);

            // Append cells to the row
            row3.Append(cell3_1, cell3_2, cell3_3);

            // Append row to the table
            table.Append(row3);

            body.Append(table);
        }
        private void PrintInputRow(TableRow inputRow)
        {
            foreach (TableCell cell in inputRow.Elements<TableCell>())
            {
                // Lấy nội dung văn bản từ mỗi ô
                string cellText = GetCellText(cell);
                Console.Write(cellText); // In nội dung ô và cách nhau bằng tab
            }
            Console.WriteLine(); // Xuống dòng sau khi in tất cả các ô
        }

        private string GetCellText(TableCell cell)
        {
            // Lấy nội dung văn bản từ ô
            string text = string.Empty;
            foreach (var paragraph in cell.Elements<Paragraph>())
            {
                foreach (var run in paragraph.Elements<Run>())
                {
                    foreach (var textElement in run.Elements<Text>())
                    {
                        text += textElement.Text;
                    }
                }
            }
            return text;
        }


    }
}
