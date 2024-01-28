using Avalonia.Controls;
using PdfSharp.Pdf;
using System;

namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : UserControl
    {
        public void LoadPdf(PdfSharp.Pdf.PdfDocument pdfDocument)
        {
            Console.WriteLine("Loading PDF...");
            try
            {
                // Используйте текущий экземпляр PdfViewer для отображения PDF-контента
                // Обратите внимание, что теперь используется PdfSharp.Pdf.PdfDocument
                Content = pdfDocument;
                Console.WriteLine("PDF loaded successfully.");
            }
            catch (Exception ex)
            {
                // Выводите информацию об ошибке
                Console.WriteLine($"Error loading PDF: {ex.Message}");
            }
        }
    }
}
