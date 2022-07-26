using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace Extras.Services
{
    public class ExcelTools
    {
        public static SKBitmap bm { get; set; }
        public static ImagePartType GetImagePartTypeByBitmap(SKBitmap image)
        {
            var info = image.Info;
            
            if (ImageFormat.Bmp.Equals(image.ColorType))
                return ImagePartType.Bmp;
            else if (ImageFormat.Gif.Equals(image.ColorType))
                return ImagePartType.Gif;
            else if (ImageFormat.Png.Equals(image.ColorType))
                return ImagePartType.Png;
            else if (ImageFormat.Tiff.Equals(image.ColorType))
                return ImagePartType.Tiff;
            else if (ImageFormat.Icon.Equals(image.ColorType))
                return ImagePartType.Icon;
            else if (ImageFormat.Jpeg.Equals(image.ColorType))
                return ImagePartType.Jpeg;
            else if (ImageFormat.Emf.Equals(image.ColorType))
                return ImagePartType.Emf;
            else if (ImageFormat.Wmf.Equals(image.ColorType))
                return ImagePartType.Wmf;
            else
                throw new Exception("Image type could not be determined.");
        }

        public static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist
                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            return (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
        }

        public static void AddImage(bool createFile, string excelFile, string sheetName,
                                    string imageFileName, string imgDesc,
                                    int colNumber, int rowNumber, byte[] bits)
        {
            using (var imageStream = new FileStream(imageFileName, FileMode.Open))
            {
                AddImage(createFile, excelFile, sheetName, imageStream, imgDesc, colNumber, rowNumber, bits);
            }
        }

        public static void AddImage(WorksheetPart worksheetPart,
                                    string imageFileName, string imgDesc,
                                    int colNumber, int rowNumber, byte[] bits)
        {
            using (var imageStream = new FileStream(imageFileName, FileMode.Open))
            {
                AddImage(worksheetPart, imageStream, imgDesc, colNumber, rowNumber, bits);
            }
        }

        public static void AddImage(bool createFile, string excelFile, string sheetName,
                                    Stream imageStream, string imgDesc,
                                    int colNumber, int rowNumber, byte[] bits)
        {
            SpreadsheetDocument spreadsheetDocument = null;
            WorksheetPart worksheetPart = null;
            if (createFile)
            {
                // Create a spreadsheet document by supplying the filepath
                spreadsheetDocument = SpreadsheetDocument.Create(excelFile, SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart
                worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add Sheets to the Workbook
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook
                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = sheetName
                };
                sheets.Append(sheet);
            }
            else
            {
                // Open spreadsheet
                spreadsheetDocument = SpreadsheetDocument.Open(excelFile, true);

                // Get WorksheetPart
                worksheetPart = GetWorksheetPartByName(spreadsheetDocument, sheetName);
            }

            AddImage(worksheetPart, imageStream, imgDesc, colNumber, rowNumber, bits);

            worksheetPart.Worksheet.Save();

            spreadsheetDocument.Close();
        }

        public static void AddImage(WorksheetPart worksheetPart,
                                    Stream imageStream, string imgDesc,
                                    int colNumber, int rowNumber, byte[] bits)
        {
            // We need the image stream more than once, thus we create a memory copy
            MemoryStream imageMemStream = new MemoryStream();
            imageStream.Position = 0;
            imageStream.CopyTo(imageMemStream);
            imageStream.Position = 0;

            var drawingsPart = worksheetPart.DrawingsPart;
            if (drawingsPart == null)
                drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();

            if (!worksheetPart.Worksheet.ChildElements.OfType<Drawing>().Any())
            {
                worksheetPart.Worksheet.Append(new Drawing { Id = worksheetPart.GetIdOfPart(drawingsPart) });
            }

            if (drawingsPart.WorksheetDrawing == null)
            {
                drawingsPart.WorksheetDrawing = new Xdr.WorksheetDrawing();
            }

            var worksheetDrawing = drawingsPart.WorksheetDrawing;

            //Bitmap bm = new Bitmap(imageMemStream);
            //imageMemStream.Seek(0, SeekOrigin.Begin);
            //var bm = SKBitmap.Decode(imageMemStream);
            //bm = BitmapExtensions.LoadBitmapResource(GetType(),"SkiaSharpFormsDemos.Media.MonkeyFace.png");
            //MemoryStream stream = new MemoryStream(pics[0].Pic);
            //using (var resource = assembly.GetManifestResourceStream("embedded.png"))
            var strom = new SKManagedStream(imageStream);
            var bm = new SKBitmap();
            bm = SKBitmap.Decode(strom);
            //// pin the managed array so that the GC doesn't move it
            //var gcHandle = GCHandle.Alloc(bits, GCHandleType.Pinned);

            //// install the pixels with the color type of the pixel data
            //var info = new SKImageInfo(2, 2, SKImageInfo.PlatformColorType, SKAlphaType.Unpremul);
            //bm.InstallPixels(info, gcHandle.AddrOfPinnedObject(), info.RowBytes, delegate { gcHandle.Free(); }, null);


            //var imagePart = drawingsPart.AddImagePart(GetImagePartTypeByBitmap(bm));
            //var imagePart = drawingsPart.AddImagePart(bm);
            var imagePart = drawingsPart.AddImagePart(ImagePartType.Jpeg);
            imagePart.FeedData(imageStream);

            A.Extents extents = new A.Extents();
            long w = 1676400;
            long h = 1615440;
            var extentsCx = w;// bm.Width * (long)(914400 / bm.Width);
            var extentsCy = h;// bm.Height * (long)(914400 / bm.Height);
            bm.Dispose();

            var colOffset = 0;
            var rowOffset = 0;

            var nvps = worksheetDrawing.Descendants<Xdr.NonVisualDrawingProperties>();
            var nvpId = nvps.Count() > 0
                ? (UInt32Value)worksheetDrawing.Descendants<Xdr.NonVisualDrawingProperties>().Max(p => p.Id.Value) + 1
                : 1U;

            var oneCellAnchor = new Xdr.OneCellAnchor(
                new Xdr.FromMarker
                {
                    ColumnId = new Xdr.ColumnId((colNumber - 1).ToString()),
                    RowId = new Xdr.RowId((rowNumber - 1).ToString()),
                    ColumnOffset = new Xdr.ColumnOffset(colOffset.ToString()),
                    RowOffset = new Xdr.RowOffset(rowOffset.ToString())
                },
                new Xdr.Extent { Cx = extentsCx, Cy = extentsCy },
                new Xdr.Picture(
                    new Xdr.NonVisualPictureProperties(
                        new Xdr.NonVisualDrawingProperties { Id = nvpId, Name = "Picture " + nvpId, Description = imgDesc },
                        new Xdr.NonVisualPictureDrawingProperties(new A.PictureLocks { NoChangeAspect = true })
                    ),
                    new Xdr.BlipFill(
                        new A.Blip { Embed = drawingsPart.GetIdOfPart(imagePart), CompressionState = A.BlipCompressionValues.Print },
                        new A.Stretch(new A.FillRectangle())
                    ),
                    new Xdr.ShapeProperties(
                        new A.Transform2D(
                            new A.Offset { X = 0, Y = 0 },
                            new A.Extents { Cx = extentsCx, Cy = extentsCy }
                        ),
                        new A.PresetGeometry { Preset = A.ShapeTypeValues.Rectangle }
                    )
                ),
                new Xdr.ClientData()
            );
            worksheetDrawing.Append(oneCellAnchor);
        }
    }
}