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
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels;

public partial class AuthWindowViewModel: ViewModelBase
{
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
    private string username;
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
    private string password;

    private bool CanLogIn => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    
    [RelayCommand(CanExecute = nameof(CanLogIn))]
    public async Task LogIn()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return;
            }
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
        catch (RequestException ex)
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