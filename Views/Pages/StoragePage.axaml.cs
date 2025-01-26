using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class StoragePage : ContentControl
{
    public StoragePage(ContentControl currentPage)
    {
        InitializeComponent();
        DataContext = new StoragePageViewModel(currentPage);
    }
}
