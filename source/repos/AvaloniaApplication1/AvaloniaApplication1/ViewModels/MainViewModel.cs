<<<<<<< HEAD
﻿using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AvaloniaApplication1.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private string? _selectedPdfPath;
        private readonly Window _mainWindow;

        public MainViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
            SelectPdfCommand = ReactiveCommand.CreateFromTask(SelectPdf);
        }

        public string Greeting => "Welcome to Avalonia!";

        public string? SelectedPdfPath
        {
            get => _selectedPdfPath;
            set => this.RaiseAndSetIfChanged(ref _selectedPdfPath, value);
        }

        public ReactiveCommand<Unit, Unit> SelectPdfCommand { get; }

        private async Task SelectPdf()
        {
            var fileDialog = new OpenFileDialog();

            var selectedFile = await fileDialog.ShowAsync(_mainWindow);

            if (selectedFile != null && selectedFile.Length > 0)
            {
                SelectedPdfPath = selectedFile[0];
            }
        }
    }
=======
﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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

                try
                {
                    // Вызов метода для отображения PDF в PdfViewer
                    DisplayPdfAsync(selectedFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error displaying PDF: {ex.Message}");
                }
            }
        }


        private void DisplayPdfAsync(string filePath)
        {
            try
            {
                Console.WriteLine($"Attempting to open PDF file: {filePath}");

                var pdfContent = LoadPdfFile(filePath);

                // Создайте новый экземпляр PdfViewer и загрузите в него содержимое PDF
                var pdfViewer = new AvaloniaApplication1.Views.PdfViewer();
                pdfViewer.LoadPdf(pdfContent);

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



        private async void OpenPdfViewerWindow(AvaloniaApplication1.Views.PdfViewer pdfViewer)
        {
            var newWindow = new Window
            {
                Title = "PDF Viewer Window",
                Width = 800,
                Height = 600,
                Content = new Avalonia.Controls.ScrollViewer
                {
                    Name = "scrollViewer",
                    HorizontalScrollBarVisibility = (Avalonia.Controls.Primitives.ScrollBarVisibility)ScrollBarVisibility.Disabled,
                    VerticalScrollBarVisibility = (Avalonia.Controls.Primitives.ScrollBarVisibility)ScrollBarVisibility.Auto,
                    Content = pdfViewer,
                },
            };

            newWindow.PointerWheelChanged += (sender, e) =>
            {
                if (e.Delta.Y > 0)
                    pdfViewer.ScrollPage(-1);
                else
                    pdfViewer.ScrollPage(1);
            };

            await pdfViewer.LoadPdfTask;

            newWindow.Show();
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

        public async void Execute(object? parameter) => await ExecuteAsync();

        public async Task ExecuteAsync()
        {
            if (CanExecute(null))
            {
                await _execute();
            }
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

>>>>>>> 50432bc8e79851b838f51c1bf911896fd60cb0ea
}
