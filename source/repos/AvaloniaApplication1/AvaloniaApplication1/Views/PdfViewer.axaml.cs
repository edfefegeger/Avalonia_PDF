using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging; // Добавьте это пространство имен

namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : Window
    {
        public void LoadPdf(PdfiumViewer.PdfDocument pdfDocument)
        {
            // Используйте текущий экземпляр PdfViewer для отображения PDF-контента
            var pdfViewer = new PdfViewer();
            pdfViewer.LoadPdf(pdfDocument);

            Content = pdfViewer;

        }
    }
}
