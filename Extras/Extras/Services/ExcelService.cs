using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Extras.Models;
using Extras.Views;

namespace Extras.Services
{
    public class ExcelService
    {
        private string AppFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private Cell ConstructCell(string value, CellValues dataTypes) =>
            new Cell()
            {  
                 CellValue = new CellValue(value),
                 DataType = new EnumValue<CellValues>(dataTypes)
            };

        public string GenerateExcel(String fileName, List<string> shtNames)
        {
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

            // Creating the SpreadsheetDocument in the indicated FilePath
            var filePath = Path.Combine(AppFolder, fileName);
            var document = SpreadsheetDocument.Create(Path.Combine(AppFolder, fileName), SpreadsheetDocumentType.Workbook);

            var wbPart = document.AddWorkbookPart();
            wbPart.Workbook = new Workbook();

            //kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk
            var part = wbPart.AddNewPart<WorksheetPart>();
            part.Worksheet = new Worksheet(new SheetData());
            UInt32Value cnt = 1;
            foreach (var item in shtNames)
            {
                //  Here are created the sheets, you can add all the child sheets that you need.
                var sheets = wbPart.Workbook.AppendChild
                    (
                       new Sheets(
                                new Sheet()
                                {
                                    Id = wbPart.GetIdOfPart(part),
                                    SheetId = 1,
                                    Name = item
                                }
                            )
                    );
                cnt++;
            }
        
            // Just save and close you Excel file
            wbPart.Workbook.Save();
            document.Close();
            // Dont't forget return the filePath
            return filePath;
        }

        public void InsertDataIntoSheet(string fileName, string sheetName, ExcelStructure data)
        {
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

            using (var document = SpreadsheetDocument.Open(fileName, true))
            {
                var wbPart = document.WorkbookPart;
                
                DocumentFormat.OpenXml.Spreadsheet.Workbook workbook = wbPart.Workbook;
                DocumentFormat.OpenXml.Spreadsheet.Sheet s = workbook.Descendants<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Where(sht => sht.Name == sheetName).FirstOrDefault();
                WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(s.Id);
                DocumentFormat.OpenXml.Spreadsheet.SheetData sheetdata = wsPart.Worksheet.Elements<DocumentFormat.OpenXml.Spreadsheet.SheetData>().FirstOrDefault();
                var row = sheetdata.AppendChild(new Row());

                foreach (var header in data.Headers)
                {
                    row.Append(ConstructCell(header, CellValues.String));
                }

                foreach (var value in data.Values)
                {
                    var dataRow = sheetdata.AppendChild(new Row());

                    foreach (var dataElement in value)
                    {
                        dataRow.Append(ConstructCell(dataElement, CellValues.String));
                    }
                }

                wbPart.Workbook.Save();
            }
        }

        public string ExportToExcel(List<Extra> sleected)
        {
            var fileName = $"Extras-{Guid.NewGuid()}.xlsx";
            var grpBy = sleected.GroupBy(x => x.SiteArea);
            List<string> names = new List<string>();
            foreach (var area in grpBy)
            {
                names.Add(area.Key);
            }
            string filepath = GenerateExcel(fileName, names);
            var data = new ExcelStructure
            {
                Headers = new List<string>() { "Date", "Description", "Men", "Hours", "Rate", "LaborCost", }
            };
            
            foreach (var area in grpBy)
            {
                foreach (var extr in area)
                {
                    data.Values.Add(new List<string>() {extr.Date.ToString(), extr.Description,
                    extr.Men.ToString(), extr.Hours.ToString(), extr.Rate.ToString(), extr.LaborCost.ToString() });
                }
                InsertDataIntoSheet(filepath, area.Key, data);
                
            }

            return filepath;
        }
    }
}
