using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging; // �������� ��� ������������ ����

namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : Window
    {
        public void LoadPdf(PdfiumViewer.PdfDocument pdfDocument)
        {
            // ����������� ������� ��������� PdfViewer ��� ����������� PDF-��������
            var pdfViewer = new PdfViewer();
            pdfViewer.LoadPdf(pdfDocument);

            Content = pdfViewer;

        }
    }
}
