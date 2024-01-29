using Avalonia.Controls;
using Avalonia.Media.Imaging;
using PdfiumViewer;
using System;
using System.IO;
namespace AvaloniaApplication1.Views
{
    public partial class PdfViewer : UserControl
    {
        private PdfDocument _pdfDocument;
        private int _currentPageIndex = 0;

        public PdfViewer()
        {
            InitializeComponent();
        }

        public void LoadPdf(byte[] pdfContent)
        {
            using (MemoryStream stream = new MemoryStream(pdfContent))
            {
                _pdfDocument = PdfDocument.Load(stream);
                UpdateDisplayedPage();
            }
        }

        private void UpdateDisplayedPage()
        {
            if (_pdfDocument != null && _currentPageIndex >= 0 && _currentPageIndex < _pdfDocument.PageCount)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    var pdfRenderer = new PdfRenderer();
                    pdfRenderer.Load(_pdfDocument);



                    stream.Position = 0;
                    var bitmap = new Bitmap(stream);
                    pdfImage.Source = bitmap;
                }
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
