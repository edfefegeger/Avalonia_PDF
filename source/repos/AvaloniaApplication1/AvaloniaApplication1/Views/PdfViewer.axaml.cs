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

namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : UserControl
    {
        private PdfDocument _pdfDocument;
        private int _currentPageIndex = 0;

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading PDF: {ex.Message}");
                // Дополнительная обработка ошибки
            }
        }


        private void UpdateDisplayedPage()
        {
            if (_pdfDocument != null && _currentPageIndex >= 0 && _currentPageIndex < _pdfDocument.PageCount)
            {
                var page = _pdfDocument.Pages[_currentPageIndex];

                // Используйте Dispatcher.UIThread.Post для выполнения кода в основном потоке
                Dispatcher.UIThread.Post(() =>
                {
                    // Обновление UI с использованием page
                });
            }
        }


        public void ScrollPage(int delta)
        {
            if (_pdfDocument != null)
            {
                _currentPageIndex = Math.Max(0, Math.Min(_pdfDocument.PageCount - 1, _currentPageIndex + delta));
                UpdateDisplayedPage();
            }
        }
    }
}
