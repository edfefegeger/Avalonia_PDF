using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaApplication1.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace AvaloniaApplication1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICommand _selectPdfCommand;
        // Добавьте это свойство в класс MainViewModel
        public string Greeting => "Привет, Avalonia!";
        public MainViewModel()
        {
            _selectPdfCommand = new RelayCommand(
                async () => await SelectPdfAsync(),
                () => true
            );
        }

        public ICommand SelectPdfCommand => _selectPdfCommand;

        public async Task SelectPdfAsync()
        {
            Console.WriteLine("SelectPdfAsync is called");
            var fileDialog = new OpenFileDialog
            {
                Title = "Выберите файл PDF"
            };

            var result = await fileDialog.ShowAsync(GetMainWindow());

            if (result != null && result.Length > 0)
            {
                string selectedFile = result[0];
                Console.WriteLine($"Selected PDF file: {selectedFile}");
                // Вызов метода для отображения PDF в PdfViewer
                DisplayPdfAsync(selectedFile);
            }
        }

        private void DisplayPdfAsync(string filePath)
        {
            try
            {
                Console.WriteLine($"Attempting to open PDF file: {filePath}");
                var pdfDocument = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);

                // Создайте новый экземпляр PdfViewer и загрузите в него PDF-документ
                var pdfViewer = new AvaloniaApplication1.Views.PdfViewer();
                pdfViewer.LoadPdf(pdfDocument);

                // Откройте новое окно с PdfViewer
                OpenPdfViewerWindow(pdfViewer);

                Console.WriteLine("PDF loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening PDF: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void OpenPdfViewerWindow(PdfViewer pdfViewer)
        {
            var newWindow = new Window
            {
                Title = "PDF Viewer Window",
                Content = pdfViewer,
                Width = 800, // Установите желаемую ширину
                Height = 600, // Установите желаемую высоту
            };

            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                newWindow.Show();
            }
        }



        private byte[] LoadPdfFile(string filePath)
        {
            // Здесь вы должны реализовать загрузку содержимого PDF-файла в виде массива байт
            // Пример: использование System.IO.File.ReadAllBytes(filePath)
            return System.IO.File.ReadAllBytes(filePath);
        }

        private Window GetMainWindow()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            throw new InvalidOperationException("Главное окно недоступно.");
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Func<Task> _execute;

        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;


        public async void Execute(object? parameter) => await _execute();

        public event EventHandler? CanExecuteChanged;




        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
