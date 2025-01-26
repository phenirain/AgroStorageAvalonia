using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class MainPage : ContentControl
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = new MainPageViewModel();
    }
}
