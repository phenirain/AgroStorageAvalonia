using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.DTOs.Clients;
using desktop.Models.Entities;
using desktop.Support.Api;
using desktop.Views.Pages;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels.PagesViewModels;

public partial class ClientPageViewModel: ViewModelBase
{
    private ContentControl _currentPage;

    [ObservableProperty] private ObservableCollection<Client> _clients;
    private List<Client> _allClients;
    [ObservableProperty] private Client? _selectedClient;
    [ObservableProperty] private string _searchText;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private CreateClientRequest _createClient = new CreateClientRequest();
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    private UpdateClientRequest _updateClient = new UpdateClientRequest();

    public ClientPageViewModel(ContentControl currentPage)
    {
        _ = LoadClients();
    }

    partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Clients = new ObservableCollection<Client>(_allClients);
            return;
        }
        Clients = new ObservableCollection<Client>(_allClients.Where(c => c.CompanyName.Contains(value)));
    }

    partial void OnSelectedClientChanged(Client? value)
    {
        if (value is not null)
        {
            UpdateClient = new UpdateClientRequest()
            {
                Id = value.Id,
                CompanyName = value.CompanyName,
                ContactPerson = value.ContactPerson,
                TelephoneNumber = value.TelephoneNumber,
                Email = value.Email
            };
        }
    }

    private async Task LoadClients()
    {
        Clients = await ApiHelper.GetAll<ObservableCollection<Client>>("clients");
        _allClients = Clients.ToList();
    }

    private bool CanSave =>
        !string.IsNullOrEmpty(CreateClient.CompanyName) &&
              !string.IsNullOrEmpty(CreateClient.ContactPerson) &&
              !string.IsNullOrEmpty(CreateClient.TelephoneNumber) &&
              !string.IsNullOrEmpty(CreateClient.Email);


    [RelayCommand]
    private async Task Save()
    {
        try
        {
            if (!CanSave)
                return;
            var client = await ApiHelper.Post<Client, CreateClientRequest>(CreateClient, "clients");
            Clients.Add(client);
        }
        catch (Exception ex)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Request Error",
                    ContentMessage = ex.Message,
                    Icon = Icon.Question
                });
            await messageBox.ShowAsync();
        }
    }

    private bool CanUpdate =>
        UpdateClient.Id != 0 &&
        !string.IsNullOrEmpty(UpdateClient.CompanyName) &&
        !string.IsNullOrEmpty(UpdateClient.ContactPerson) &&
        !string.IsNullOrEmpty(UpdateClient.TelephoneNumber) &&
        !string.IsNullOrEmpty(UpdateClient.Email);


    [RelayCommand]
    private async Task Update()
    {
        try
        {
            if (!CanUpdate)
                return;
            await ApiHelper.Put(UpdateClient, $"clients", UpdateClient.Id);
            var client = _allClients.First(c => c.Id == UpdateClient.Id);
            client.CompanyName = UpdateClient.CompanyName;
            client.ContactPerson = UpdateClient.ContactPerson;
            client.TelephoneNumber = UpdateClient.TelephoneNumber;
            client.Email = UpdateClient.Email;
        }
        catch (Exception ex)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Request Error",
                    ContentMessage = ex.Message,
                    Icon = Icon.Question
                });
            await messageBox.ShowAsync();
        }
    }

    private bool CanDelete => SelectedClient is not null;

    [RelayCommand]
    private async Task Delete()
    {
        try
        {
            if (!CanDelete)
                return;
            await ApiHelper.Delete($"clients", SelectedClient!.Id);
            Clients.Remove(SelectedClient);
        }
        catch (Exception ex)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Request Error",
                    ContentMessage = ex.Message,
                    Icon = Icon.Question
                });
            await messageBox.ShowAsync();
        }
    }

    [RelayCommand]
    private async Task Back()
    {
        _currentPage.Content = new MainPage();
    }
}
