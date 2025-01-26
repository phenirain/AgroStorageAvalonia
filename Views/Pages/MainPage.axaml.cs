using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class MainPage : UserControl
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = new MainPageViewModel();
    }
}
