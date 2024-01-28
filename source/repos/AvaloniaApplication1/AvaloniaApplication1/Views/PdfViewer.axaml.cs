using Avalonia.Controls;
using QuestPDF;
using QuestPDF.Infrastructure;



namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : Window
    {
        public void LoadPdf(byte[] pdfBytes)
        {
            // ����������� ������� ��������� PdfViewer ��� ����������� PDF-��������
            var pdfDocument = new PdfDocumentBuilder().BuildPdf(pdfBytes);
            Content = pdfDocument;
        }
    }
}
