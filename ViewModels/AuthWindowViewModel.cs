using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.DTOs.Auth;
using desktop.Exceptions;
using desktop.Support.Api;
using desktop.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels;

public partial class AuthWindowViewModel: ViewModelBase
{
    [NotifyCanExecuteChangedFor(nameof(AuthCommand))]
    [ObservableProperty]
    private string username;
    [NotifyCanExecuteChangedFor(nameof(AuthCommand))]
    [ObservableProperty]
    private string password;

    private bool CanLogIn => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    
    [RelayCommand]
    // [RelayCommand]
    public async Task Auth()
    {
        try
        {
            if (!CanLogIn) return;
            var currentWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.Windows.FirstOrDefault(w => w.IsActive)
                : null;
            AuthRequest request = new()
            {
                Login = Username,
                Password = Password
            };
            AuthResponse response = await ApiHelper.Post<AuthResponse, AuthRequest>(request, "auth");
            if (response.Role != null)
            {
                MainWindow w = new MainWindow(response.Role);
                w.Show();
                currentWindow.Close();
            }
            else
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = response.Status,
                        ContentMessage = response.Message,
                        Icon = Icon.Question
                    });
                await messageBox.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Error",
                    ContentMessage = ex.Message,
                    Icon = Icon.Question
                });
            await messageBox.ShowAsync();
        }
    }
}
