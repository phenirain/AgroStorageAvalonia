using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class ClientPage : ContentControl
{
    public ClientPage(ContentControl currentPage)
    {
        InitializeComponent();
        DataContext = new ClientPageViewModel(currentPage);
    }
}
