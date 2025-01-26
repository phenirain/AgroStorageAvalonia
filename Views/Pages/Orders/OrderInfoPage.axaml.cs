using Avalonia.Controls;
using desktop.Models.Entities;
using desktop.ViewModels.PagesViewModels.Orders;

namespace desktop.Views.Pages.Orders;

public partial class OrderInfoPage : ContentControl
{
    public OrderInfoPage(Order order, ContentControl currentPage)
    {
        InitializeComponent();
        DataContext = new OrderInfoPageViewModel(order, currentPage);
    }
}
