using System;
using System.Linq;
using GemBox.Spreadsheet;
using Extras.Models;
using System.Collections.Generic;
using System.IO;

namespace Extras.Services
{
    public class Gemmers
    {
        private string AppFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string GetGemmer(List<Extra> ixtris)
        {
            try
            {
                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                var fileName = $"Extras-" +App.Database.GetCurrentProjectAsync().Result + ".xlsx";
                Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");
                var filePath = Path.Combine(AppFolder, fileName);
                var workbook = new ExcelFile();
                
                foreach (var area in ixtris.GroupBy(x => x.SiteArea))
                {
                    CreateWorkSheet(area.ToList(), area.Key, workbook);
                }
                workbook.Save(filePath);
                return filePath;
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void CreateWorkSheet(List<Extra> ixtris, string sheetName, ExcelFile workbook)
        {

            List<string> headers = new List<string>
                { "Ref Num", "Date", "Description", "Rate", "Men", "Hours", "Value" };
            var worksheet = workbook.Worksheets.Add(sheetName);


            // Write title to Excel cell.
            var range = worksheet.Cells.GetSubrange("A1:F1");
            range.Merged = true;

            range.Value = App.Database.GetCurrentProjectAsync().Result.ProjectName +  " - Dayworks";
            range.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            //range.Style.Font.Size = 20;
            worksheet.Cells["A1"].Style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Accent5Lighter60Pct),
                SpreadsheetColor.FromName(ColorName.Accent5Lighter60Pct));
            worksheet.Rows["1"].Style = workbook.Styles[BuiltInCellStyleName.Heading1];

            // Set columns width.
            worksheet.Columns["A"].SetWidth(10, LengthUnit.ZeroCharacterWidth);
            worksheet.Columns["B"].SetWidth(16, LengthUnit.ZeroCharacterWidth);
            worksheet.Columns["C"].SetWidth(90, LengthUnit.ZeroCharacterWidth);
            worksheet.Columns["D"].SetWidth(16, LengthUnit.ZeroCharacterWidth);
            worksheet.Columns["E"].SetWidth(20, LengthUnit.ZeroCharacterWidth);
            worksheet.Columns["F"].SetWidth(9, LengthUnit.ZeroCharacterWidth);
            worksheet.Columns["G"].SetWidth(11, LengthUnit.ZeroCharacterWidth);

            // Write header data to Excel cells.
            //for (int col = 0; col < skyscrapers.GetLength(1); col++)
            //    worksheet.Cells[3, col].Value = skyscrapers[0, col];
            for (int col = 0; col < headers.Count(); col++)
                worksheet.Cells[2, col].Value = headers[col];

            // Set header cells formatting.
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.FillPattern.SetPattern(FillPatternStyle.Solid, SpreadsheetColor.FromName(ColorName.Accent2Lighter60Pct),
                SpreadsheetColor.FromName(ColorName.Accent2Lighter60Pct));
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
            style.WrapText = true;
            style.Borders.SetBorders(MultipleBorders.Right | MultipleBorders.Top, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);
            worksheet.Cells.GetSubrange("A3:G3").Style = style;
            var row = 3;
            foreach (var extr in ixtris)
            {
                var cell = worksheet.Cells[row, 0];
                cell.Value = extr.ID;
                cell.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
                cell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell = worksheet.Cells[row, 1];
                cell.Value = extr.Date.Value.ToShortDateString();
                cell.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
                cell = worksheet.Cells[row, 2];
                cell.Value = extr.Description;
                cell.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
                cell.Style.WrapText = true;
                cell = worksheet.Cells[row, 3];
                cell.Value = extr.Rate;
                cell.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
                cell.Style.NumberFormat = "€   #,##0.00";
                cell = worksheet.Cells[row, 4];
                cell.Value = extr.Men;
                cell.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
                cell = worksheet.Cells[row, 5];
                cell.Value = extr.Hours;
                cell.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
                cell = worksheet.Cells[row, 6];
                cell.Value = extr.LaborCost;
                cell.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
                cell.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
                cell.Style.NumberFormat = "€   #,##0.00";
                row++;
            }
            var cill = worksheet.Cells[row, 0];
            cill.Value = "Total";
            cill.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
            cill.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
            cill.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
            cill.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;

            cill = worksheet.Cells[row, 6];
            cill.Value = ixtris.Sum(x => x.LaborCost);
            cill.Style.Borders[IndividualBorder.Right].LineStyle = LineStyle.Thin;
            cill.Style.Borders[IndividualBorder.Left].LineStyle = LineStyle.Thin;
            cill.Style.Borders[IndividualBorder.Top].LineStyle = LineStyle.Thin;
            cill.Style.Borders[IndividualBorder.Bottom].LineStyle = LineStyle.Thin;
            cill.Style.NumberFormat = "€   #,##0.00";

            worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
        }

    }
}
