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
        private Avalonia.Media.Imaging.Bitmap[] _pageBitmaps;
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
                // Освободить ресурсы текущего документа перед загрузкой нового
                if (_pdfDocument != null)
                {
                    _pdfDocument.Dispose();
                    _pdfDocument = null;
                }

                using (MemoryStream stream = new MemoryStream(pdfContent))
                {
                    _pdfDocument = PdfDocument.Load(stream);
                    _pageBitmaps = new Avalonia.Media.Imaging.Bitmap[_pdfDocument.PageCount];

                    // Рендер всех страниц
                    for (int i = 0; i < _pdfDocument.PageCount; i++)
                    {
                        _pageBitmaps[i] = RenderPdfPage(i);
                    }

                    _currentPageIndex = 0; // Начать с первой страницы
                    _loadPdfTaskCompletionSource.TrySetResult(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading PDF: {ex.Message}");
                // Дополнительная обработка ошибки
            }
        }

        private Avalonia.Media.Imaging.Bitmap RenderPdfPage(int pageIndex)
        {
            try
            {
                if (_pdfDocument == null)
                {
                    Console.WriteLine("PDF document is null.");
                    return null;
                }

                var pdfBitmap = _pdfDocument.Render(pageIndex, 96, 96, PdfRenderFlags.Annotations);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (memoryStream.CanWrite)
                    {
                        pdfBitmap.Save(memoryStream, ImageFormat.Png);
                        memoryStream.Position = 0;

                        return new Avalonia.Media.Imaging.Bitmap(memoryStream);
                    }
                    else
                    {
                        Console.WriteLine("MemoryStream is closed.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error rendering PDF page: {ex.Message}");
                return null;
            }
        }

        private void UpdateDisplayedPage()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                var imageControl = this.FindControl<Avalonia.Controls.Image>("pdfImage");
                imageControl.Source = _pageBitmaps[_currentPageIndex];

                var textBlock = this.FindControl<Avalonia.Controls.TextBlock>("pdfTextBlock");
                textBlock.Text = $"Страница {_currentPageIndex + 1} из {_pdfDocument.PageCount}";
            });
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
