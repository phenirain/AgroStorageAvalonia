using Avalonia.Controls;
using desktop.ViewModels.PagesViewModels;

namespace desktop.Views.Pages;

public partial class DeliveryPage : UserControl
{
    public DeliveryPage()
    {
        InitializeComponent();
        DataContext = new DeliveryPageViewModel();
    }
}
