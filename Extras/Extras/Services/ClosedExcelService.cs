//using ClosedXML.Excel;
//using ClosedXML.Excel.Drawings;
//using Extras.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xamarin.Forms.Internals;
//using System.Drawing.Imaging;
//using SkiaSharp;
//using System.Reflection;
//using Extras.Views;

//namespace Extras.Services
//{
//    public class ClosedExcelService
//    {
//		private string AppFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

//		public async Task<string> Export(List<Extra> extrs)
//		{
//            var fileName = $"Extras-{Guid.NewGuid()}.xlsx";
//            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");

//            var filePath = Path.Combine(AppFolder, fileName);
//            //string[] files = Directory.GetFiles(AppFolder);

//   //         var assembly = IntrospectionExtensions.GetTypeInfo(typeof(VeiwAll)).Assembly;
//			//string fileNime = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "poopoo.xlsx");

//			//if (!File.Exists(fileName))
//			//{
//			//	var f = "f";
//			//}

//			var grpBy = extrs.GroupBy(x => x.SiteArea);

//            using (IXLWorkbook workbook = new XLWorkbook())
//            {
//                foreach (var area in grpBy)
//                {
//                    List<ExcelExtra> excellers = new List<ExcelExtra>();
//                    foreach (var tim in area)
//                    {
//                        excellers.Add(new ExcelExtra(tim.JobSite, tim.Men, tim.Description, tim.Hours, tim.Rate, tim.Date.Value.ToShortDateString()));
//                    }
//                    IXLWorksheet ws = workbook.AddWorksheet(area.Key);
//                    var table = ws.FirstCell().InsertTable<ExcelExtra>(excellers, false);

//                    var total = area.Sum(x => x.LaborCost);                   
//                    table.ShowTotalsRow = true;
//					table.TotalsRow().Cell(7).Value = total;
//					table.TotalsRow().Cell(7).Style.NumberFormat.Format = "€   #,##0.00";

					

//					for (int i = 0; i < area.Count(); i++)
//                    {
//                        ws.Row(i + 2).Cell(4).Style.NumberFormat.Format = "€   #,##0.00";
//                        ws.Row(i + 2).Cell(7).Style.NumberFormat.Format = "€   #,##0.00";
//                    }
//                    table.Field(0).TotalsRowLabel = "Total";
//					//ws.Columns().AdjustToContents();
//                }
				
//                workbook.SaveAs(filePath);

//            }
//            //await addPics(extrs, filePath);
//			return filePath;
//		}

//        //private async Task addPics(IXLWorksheet ws, int i, List<Extra> allExrtas, string exFile)
//		private async Task addPics(List<Extra> allExrtas, string exFile)
//		{
//			var areas = allExrtas.GroupBy(x => x.SiteArea).ToArray();
//			var id = areas[0].Select(x => x.ID).FirstOrDefault();
//			List<Pics> pics = await App.Database.GetPicsAsync(id);
//			try
//			{
//				//MemoryStream stream = new MemoryStream(pics[0].Pic);
//				//using (var resource = assembly.GetManifestResourceStream("embedded.png"))
//				//using (var strom = new SKManagedStream(stream))
//				//MemoryStream stram = new MemoryStream();
//				//            using (var imageStream = new FileStream(pics[0].FileName, FileMode.Open))
//				//            {
//				//                imageStream.CopyTo(stram);
//				//            }

//				var assembly = IntrospectionExtensions.GetTypeInfo(typeof(VeiwAll)).Assembly;
//				Stream stream = assembly.GetManifestResourceStream("Extras.mice.jpg");
//				string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "poopoo.xlsx");
				
//				if (!File.Exists(fileName))
//				{
//					var f = "f";
//                }
                
//				ExcelTools.AddImage(false, fileName, "Sheet1",
//									stream, "Image description",
//									9 /* column */, 9 /* row */, pics[0].Pic);
//			}
//			catch (Exception e)
//			{
//				var cvcvcv = e.Message;
//			}
//			//var cnt = 1;
//			//         foreach (var pic in pics)
//			//         {
//			//	MemoryStream stream = new MemoryStream(pic.Pic);
//			//	var image = ws.AddPicture(stream);
//			//	image.MoveTo(ws.Cell("B3"));
//			//	image.Scale(.5); // optional: resize picture
//			//					 //MemoryStream stream = new MemoryStream(pic.Pic);
//			//					 //IXLPicture pc = 
//			//					 //{
//			//					 //	NoChangeAspect = true,
//			//					 //	NoMove = true,
//			//					 //	NoResize = true,
//			//					 //	ImageStream = stream,
//			//					 //	Name = "Test Image"
//			//					 //};
//			//					 //ws.Row(6).Cell(9).SetValue(300);
//			//					 //var image = ws.AddPicture(pic.FileName).MoveTo((IXLCell)ws.Row(i + 1).Cell(8 + cnt).Address).Scale(.5); // optional: resize picture
//			//	//var image = ws.AddPicture(pic.FileName).MoveTo(ws.Cell("I9")).Scale(0.5); // optional: resize picture
//			//	cnt++;
//			//}
//		}


//    }
//}
