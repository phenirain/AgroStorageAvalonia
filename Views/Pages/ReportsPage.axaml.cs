using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class ReportsPage : UserControl
{
    public ReportsPage()
    {
        InitializeComponent();
        DataContext = new ReportsPageViewModel();
    }
}
