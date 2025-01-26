using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class StoragePage : UserControl
{
    public StoragePage()
    {
        InitializeComponent();
        DataContext = new StoragePageViewModel();
    }
}
