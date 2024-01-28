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
using PdfiumViewer;

namespace AvaloniaApplication1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public async Task SelectPdfAsync()
        {
            var fileDialog = new OpenFileDialog()
            {
                Title = "Выберите файл PDF",
                Filters = new List<FileDialogFilter>(new FileDialogFilter[]
                {
                    new FileDialogFilter { Name = "PDF Files", Extensions = new List<string> { "pdf" } }
                })
            };

            var result = await fileDialog.ShowAsync(GetMainWindow());

            if (result != null && result.Length > 0)
            {
                string selectedFile = result[0];
                // Вызов метода для отображения PDF в PdfViewer
                DisplayPdfAsync(selectedFile);
            }
        }

        private void DisplayPdfAsync(string filePath)
        {
            try
            {
                var pdfBytes = LoadPdfFile(filePath);

                using (var stream = new MemoryStream(pdfBytes))
                {
                    var document = PdfDocument.Load(stream);

                    // Произведите действия с загруженным документом, например, отобразите его в PdfViewer
                    var pdfViewer = new AvaloniaApplication1.Views.PdfViewer();
                    pdfViewer.LoadPdf(document);

                    if (GetMainWindow().Content is Panel mainPanel)
                    {
                        mainPanel.Children.Add(pdfViewer);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии PDF: {ex.Message}");
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

        private ICommand _selectPdfCommand;

        public ICommand SelectPdfCommand
        {
            get
            {
                return _selectPdfCommand ??= new RelayCommand(
                    async () => await SelectPdfAsync(),
                    () => true
                );
            }
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

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public async void Execute(object parameter) => await _execute();

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
