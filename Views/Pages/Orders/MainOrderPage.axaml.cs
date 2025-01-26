using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels.Orders;

namespace desktop.Views.Pages.Orders;

public partial class MainOrderPage : UserControl
{
    public MainOrderPage()
    {
        InitializeComponent();
        DataContext = new MainOrderPageViewModel();
    }
}
