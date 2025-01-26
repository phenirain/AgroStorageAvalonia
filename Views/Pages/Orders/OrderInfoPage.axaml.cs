using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels.Orders;

namespace desktop.Views.Pages.Orders;

public partial class OrderInfoPage : UserControl
{
    public OrderInfoPage()
    {
        InitializeComponent();
        DataContext = new OrderInfoPageViewModel();
    }
}
