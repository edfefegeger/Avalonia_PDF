using System.Reactive;
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
}
