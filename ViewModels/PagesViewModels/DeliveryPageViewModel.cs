using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.DTOs.Deliveries;
using desktop.DTOs.Orders;
using desktop.Models.Entities;
using desktop.Support;
using desktop.Support.Api;
using desktop.Views.Pages;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels.PagesViewModels;

public partial class DeliveryPageViewModel: ViewModelBase
{

    public List<string> Statuses { get; set; } = ProgramHelper.GetEnumMemberValues<DeliveryStatus>();   public List<Driver> Drivers { get; set; }
    public List<Order> Orders { get; set; }
    [ObservableProperty] private ObservableCollection<Delivery> _deliveries;
    [ObservableProperty] private Delivery? _selectedDelivery;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DriverUpdateCommand))]
    private Driver? _selectedDriver;
    [NotifyCanExecuteChangedFor(nameof(StatusUpdateCommand))]
    [ObservableProperty] private string? _selectedStatus;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private CreateDeliveryRequest _createDeliveryRequest = new CreateDeliveryRequest();
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    private UpdateDeliveryRequest _updateDeliveryRequest = new UpdateDeliveryRequest();
    [ObservableProperty] private DateTimeOffset? _createDeliveryDate;

    private readonly ContentControl _currentPage;
    public DeliveryPageViewModel(ContentControl currentPage)
    {
        _currentPage = currentPage;
        _ = LoadDeliveries();
        _ = LoadOrders();
        _ = LoadDrivers();
    }

    #region Loading Data

    private async Task LoadDeliveries()
    {
        Deliveries = await ApiHelper.GetAll<ObservableCollection<Delivery>>("deliveries");
    }

    private async Task LoadOrders()
    {
        Orders = await ApiHelper.GetAll<List<Order>>("orders");
    }

    private async Task LoadDrivers()
    {
        Drivers = await ApiHelper.GetAll<List<Driver>>("drivers");
    }

    #endregion

    partial void OnCreateDeliveryDateChanged(DateTimeOffset? value)
    {
        if (value is not null)
        {
            CreateDeliveryRequest.Date = value.Value.DateTime;
        }
    }

    partial void OnSelectedDeliveryChanged(Delivery? value)
    {
        if (value is not null)
        {
            UpdateDeliveryRequest.Id = value.Id;
            UpdateDeliveryRequest.Date = value.Date;
            UpdateDeliveryRequest.Transport = value.Transport;
            UpdateDeliveryRequest.Route = value.Route;
            UpdateDeliveryRequest.Status = value.Status;
            UpdateDeliveryRequest.DriverId = value.Driver.Id;
        }
    }

    private bool CanSave() =>
        CreateDeliveryDate != null
        && CreateDeliveryRequest.DriverId > 0
        && string.IsNullOrEmpty(CreateDeliveryRequest.Transport)
        && string.IsNullOrEmpty(CreateDeliveryRequest.Route);

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task Save()
    {
        try
        {
            var delivery = await ApiHelper.Post<Delivery, CreateDeliveryRequest>(CreateDeliveryRequest, "deliveries");
            Deliveries.Add(delivery);
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

    private bool CanUpdate() =>
        UpdateDeliveryRequest != null
        && UpdateDeliveryRequest.DriverId > 0
        && UpdateDeliveryRequest.Date != null
        && string.IsNullOrEmpty(UpdateDeliveryRequest.Transport)
        && string.IsNullOrEmpty(UpdateDeliveryRequest.Route);

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task Update()
    {
        try
        {
            CreateDeliveryRequest.Status = DeliveryStatus.Scheduled;
            await ApiHelper.Put(UpdateDeliveryRequest, $"deliveries", UpdateDeliveryRequest.Id);
            var delivery = Deliveries.First(d => d.Id == UpdateDeliveryRequest.Id);
            delivery.Id = UpdateDeliveryRequest.Id;
            delivery.Date = UpdateDeliveryRequest.Date;
            delivery.Transport = UpdateDeliveryRequest.Transport;
            delivery.Route = UpdateDeliveryRequest.Route;
            delivery.Status = UpdateDeliveryRequest.Status;
            delivery.Driver = Drivers.First(d => d.Id == UpdateDeliveryRequest.DriverId);
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

    private bool CanDelete() => SelectedDelivery is not null;

    [RelayCommand]
    private async Task Delete()
    {
        try
        {
            await ApiHelper.Delete($"deliveries", SelectedDelivery.Id);
            Deliveries.Remove(_selectedDelivery);
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

    private bool CanDriverUpdate() => SelectedDelivery is not null && SelectedDriver is not null;

    [RelayCommand]
    private async Task DriverUpdate()
    {
        try
        {
            UpdateDeliveryRequest.DriverId = SelectedDriver.Id;
            await ApiHelper.Put(UpdateDeliveryRequest, $"deliveries", SelectedDelivery.Id);
            var delivery = Deliveries.First(d => d.Id == SelectedDelivery.Id);
            delivery.Driver = Drivers.First(d => d.Id == UpdateDeliveryRequest.DriverId);
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

    private bool CanStatusUpdate() => SelectedDelivery is not null && !string.IsNullOrEmpty(SelectedStatus);

    [RelayCommand(CanExecute = nameof(CanStatusUpdate))]
    private async Task StatusUpdate()
    {
        try
        {
            UpdateDeliveryRequest.Status =
                ProgramHelper.GetEnumValueFromEnumMember<DeliveryStatus>(SelectedStatus)!.Value;
            await ApiHelper.Put(UpdateDeliveryRequest, $"deliveries", SelectedDelivery.Id);
            var delivery = Deliveries.First(d => d.Id == SelectedDelivery.Id);
            delivery.Status = SelectedDelivery.Status;
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
