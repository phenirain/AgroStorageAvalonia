using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class DeliveryPage : ContentControl
{
    public DeliveryPage(ContentControl currentPage)
    {
        InitializeComponent();
        DataContext = new DeliveryPageViewModel(currentPage);
    }
}
