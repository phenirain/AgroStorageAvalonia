using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using desktop.Views.Pages;

namespace desktop.ViewModels.PagesViewModels;

public partial class DeliveryPageViewModel: ViewModelBase
{

    private readonly ContentControl _currentPage;
    public DeliveryPageViewModel(ContentControl currentPage)
    {

    }

    [RelayCommand]
    private async Task Save()
    {

    }

    [RelayCommand]
    private async Task Update()
    {

    }

    [RelayCommand]
    private async Task Delete()
    {

    }

    [RelayCommand]
    private async Task DriverUpdate()
    {

    }

    [RelayCommand]
    private async Task StatusUpdate()
    {

    }

    [RelayCommand]
    private async Task Back()
    {
        _currentPage.Content = new MainPage();
    }
}
