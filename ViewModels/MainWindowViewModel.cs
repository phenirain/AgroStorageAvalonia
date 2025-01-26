using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.Views.Pages;
using desktop.Views.Pages.Orders;

namespace desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ContentControl _currentPage;

    private readonly string _role;

    public MainWindowViewModel(string role)
    {
        CurrentPage = new MainPage();
        _role = role;
    }

    private bool CanGoToStorage() => _role is "Manager" or "StorageManager";

    [RelayCommand(CanExecute = nameof(CanGoToStorage))]
    private async Task GoToStorage()
    {
        CurrentPage.Content = new StoragePage(CurrentPage);
    }

    private bool CanGoToOrders() => _role is "Manager" or "SalesManager";

    [RelayCommand(CanExecute = nameof(CanGoToOrders))]
    private async Task GoToOrders()
    {
        CurrentPage.Content = new MainOrderPage(CurrentPage);
    }

    private bool CanGoToDeliveries() => _role is "Manager" or "Logician";
    [RelayCommand(CanExecute = nameof(CanGoToDeliveries))]
    private async Task GoToDeliveries()
    {
        CurrentPage.Content = new DeliveryPage(CurrentPage);
    }

    private bool CanGoToReports() => _role == "Manager";

    [RelayCommand(CanExecute = nameof(CanGoToReports))]
    private async Task GoToReports()
    {
        CurrentPage.Content = new ReportsPage(CurrentPage);
    }

    private bool CanGoToClients() => _role is "Manager" or "SalesManager";

    [RelayCommand(CanExecute = nameof(CanGoToClients))]
    private async Task GoToClients()
    {
        CurrentPage.Content = new ClientPage(CurrentPage);
    }
}
