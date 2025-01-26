using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class ReportsPage : ContentControl
{
    public ReportsPage(ContentControl currentPage)
    {
        InitializeComponent();
        DataContext = new ReportsPageViewModel(currentPage);
    }
}
