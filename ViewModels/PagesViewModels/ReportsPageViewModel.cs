using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using desktop.Views.Pages;

namespace desktop.ViewModels.PagesViewModels;

public partial class ReportsPageViewModel: ViewModelBase
{
    private readonly ContentControl _currentPage;

    public ReportsPageViewModel(ContentControl currentPage)
    {
        _currentPage = currentPage;
    }

    [RelayCommand]
    private async Task CreateStorageRemainsReportCommand()
    {

    }

    [RelayCommand]
    private async Task CreateCompletedOrdersReportCommand()
    {

    }

    [RelayCommand]
    private async Task CreateRevenueReportCommand()
    {

    }

    [RelayCommand]
    private async Task Back()
    {
        _currentPage.Content = new MainPage();
    }
}
