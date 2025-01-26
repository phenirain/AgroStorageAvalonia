using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class ClientPage : UserControl
{
    public ClientPage()
    {
        InitializeComponent();
        DataContext = new ClientPageViewModel();
    }
}
