using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;
using Avalonia.Interactivity;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaApplication1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _greeting;

        public string Greeting
        {
            get => _greeting;
            set => this.RaiseAndSetIfChanged(ref _greeting, value);
        }

        public async Task SelectPdfAsync()
        {
            var fileDialog = new OpenFileDialog()
            {
                Title = "Choose PDF file",
                Filters = new List<FileDialogFilter> { new FileDialogFilter { Name = "PDF Files", Extensions = new List<string> { "pdf" } } }
            };

            var result = await fileDialog.ShowAsync(GetMainWindow());

            if (result != null && result.Length > 0)
            {
                string selectedFile = result[0];
                // Здесь можно добавить код для обработки выбранного PDF-файла
                // Например, передать путь к файлу в другой компонент для отображения содержимого.
            }
        }

        private Window GetMainWindow()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            throw new InvalidOperationException("Main window is not available.");
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
