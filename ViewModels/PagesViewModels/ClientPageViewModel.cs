using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using desktop.Views.Pages;

namespace desktop.ViewModels.PagesViewModels;

public partial class ClientPageViewModel: ViewModelBase
{

    private ContentControl _currentPage;

    public ClientPageViewModel(ContentControl currentPage)
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
    private async Task Back()
    {
        _currentPage.Content = new MainPage();
    }
}
