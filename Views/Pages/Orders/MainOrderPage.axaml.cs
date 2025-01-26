using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels.Orders;

namespace desktop.Views.Pages.Orders;

public partial class MainOrderPage : ContentControl
{
    public MainOrderPage(ContentControl currentPage)
    {
        InitializeComponent();
        DataContext = new MainOrderPageViewModel(currentPage);
    }
}
