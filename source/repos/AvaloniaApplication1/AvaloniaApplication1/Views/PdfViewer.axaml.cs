using Avalonia.Controls;
using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Avalonia.Media;
using Avalonia.Layout;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : UserControl
    {
        private PdfDocument _pdfDocument;
        private int _currentPageIndex = 0;

        // ���� � �������� ��� ������������ ���������� �������� PDF-�����
        private TaskCompletionSource<bool> _loadPdfTaskCompletionSource = new TaskCompletionSource<bool>();
        public Task LoadPdfTask => _loadPdfTaskCompletionSource.Task;



        public PdfViewer()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void LoadPdf(byte[] pdfContent)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(pdfContent))
                {
                    _pdfDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                    UpdateDisplayedPage();
                }

                // ��������� �������� TaskCompletionSource ��� ���������� �������� PDF-�����
                _loadPdfTaskCompletionSource.TrySetResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading PDF: {ex.Message}");
                // �������������� ��������� ������
            }
        }



        private void UpdateDisplayedPage()
        {
            var textBlock = this.FindControl<TextBlock>("pdfTextBlock");
            textBlock.Text = $"Page {_currentPageIndex + 1} of {_pdfDocument.PageCount}";

            if (_pdfDocument != null && _currentPageIndex >= 0 && _currentPageIndex < _pdfDocument.PageCount)
            {
                var page = _pdfDocument.Pages[_currentPageIndex];

                // Set the width of the displayed page
                

                // ... Rest of the code
            }
        }



        public void ScrollPage(int delta)
        {
            if (_pdfDocument != null)
            {
                int newPageIndex = Math.Max(0, Math.Min(_pdfDocument.PageCount - 1, _currentPageIndex + delta));

                if (newPageIndex != _currentPageIndex)
                {
                    _currentPageIndex = newPageIndex;
                    UpdateDisplayedPage();
                }
            }
        }

    }
}
