using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using PdfiumViewer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : UserControl
    {
        private PdfDocument _pdfDocument;
        private Avalonia.Media.Imaging.Bitmap _currentBitmap;
        private int _currentPageIndex = 0;
        private TaskCompletionSource<bool> _loadPdfTaskCompletionSource = new TaskCompletionSource<bool>();
        public Task LoadPdfTask => _loadPdfTaskCompletionSource.Task;

        public PdfViewer()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void LoadPdf(byte[] pdfContent)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(pdfContent))
                {
                    _pdfDocument = PdfDocument.Load(stream);
                    _currentBitmap = RenderPdfPage(_currentPageIndex);
                    UpdateDisplayedPage();
                }

                _loadPdfTaskCompletionSource.TrySetResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading PDF: {ex.Message}");
                // Дополнительная обработка ошибки
            }
        }

        private Avalonia.Media.Imaging.Bitmap RenderPdfPage(int pageIndex)
        {
            var pdfBitmap = _pdfDocument.Render(pageIndex, 96, 96, PdfRenderFlags.Annotations);

            // Преобразование System.Drawing.Image в Avalonia.Media.Imaging.Bitmap
            using (MemoryStream memoryStream = new MemoryStream())
            {
                pdfBitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;
                return new Avalonia.Media.Imaging.Bitmap(memoryStream);
            }
        }

        private void UpdateDisplayedPage()
        {
            var imageControl = this.FindControl<Avalonia.Controls.Image>("pdfImage");
            imageControl.Source = _currentBitmap;

            var textBlock = this.FindControl<Avalonia.Controls.TextBlock>("pdfTextBlock");
            textBlock.Text = $"Страница {_currentPageIndex + 1} из {_pdfDocument.PageCount}";
        }

        public void ScrollPage(int delta)
        {
            if (_pdfDocument != null)
            {
                int newPageIndex = Math.Max(0, Math.Min(_pdfDocument.PageCount - 1, _currentPageIndex + delta));

                if (newPageIndex != _currentPageIndex)
                {
                    _currentPageIndex = newPageIndex;
                    _currentBitmap = RenderPdfPage(_currentPageIndex);
                    UpdateDisplayedPage();
                }
            }
        }
    }
}
