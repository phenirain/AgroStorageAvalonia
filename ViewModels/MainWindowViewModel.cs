using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.ViewModels.Pages;
using desktop.ViewModels.Pages.Orders;

namespace desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private UserControl _currentPage;

    private readonly string _role;

    public MainWindowViewModel(string role)
    {
        CurrentPage = new MainPage();
        _role = role;
    }

    private bool CanGoToStorage() => _role is "Manager" or "StorageManager";

    [RelayCommand(CanExecute = nameof(CanGoToStorage))]
    public async Task GoToStorage()
    {
        CurrentPage = new StoragePage();
    }

    private bool CanGoToOrders() => _role is "Manager" or "SalesManager";

    [RelayCommand(CanExecute = nameof(CanGoToOrders))]
    public async Task GoToOrders()
    {
        CurrentPage = new MainOrderPage();
    }

    private bool CanGoToDeliveries() => _role is "Manager" or "Logician";
    [RelayCommand(CanExecute = nameof(CanGoToDeliveries))]
    public async Task GoToDeliveries()
    {
        CurrentPage = new DeliveryPage();
    }

    private bool CanGoToReports() => _role == "Manager";

    [RelayCommand(CanExecute = nameof(CanGoToReports))]
    public async Task GoToReports()
    {
        CurrentPage = new ReportsPage();
    }

    private bool CanGoToClients() => _role is "Manager" or "SalesManager";

    [RelayCommand(CanExecute = nameof(CanGoToClients))]
    public async Task GoToClients()
    {
        CurrentPage = new ClientPage();
    }
}
