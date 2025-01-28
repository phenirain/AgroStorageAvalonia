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

    public List<string> Statuses { get; set; } = ProgramHelper.GetEnumMemberValues<DeliveryStatus>();
    [ObservableProperty] private ObservableCollection<Driver> _drivers;
    [ObservableProperty ]private ObservableCollection<Order> _orders;
    [ObservableProperty] private ObservableCollection<Delivery> _deliveries;
    [ObservableProperty] private Delivery? _selectedDelivery;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DriverUpdateCommand))]
    private Driver? _selectedDriver;
    [NotifyCanExecuteChangedFor(nameof(StatusUpdateCommand))]
    [ObservableProperty] private string? _selectedStatus;
    [ObservableProperty]
    private CreateDeliveryRequest _createDeliveryRequest = new CreateDeliveryRequest();
    [ObservableProperty]
    private UpdateDeliveryRequest _updateDeliveryRequest = new UpdateDeliveryRequest();
    [ObservableProperty] private DateTimeOffset? _createDeliveryDate;
    [ObservableProperty] private Order _createDeliveryOrderItem;
    [ObservableProperty] private DateTimeOffset? _updateDeliveryDate;
    [ObservableProperty] private Order _updateDeliveryOrderItem;

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
        Orders = await ApiHelper.GetAll<ObservableCollection<Order>>("orders");
    }

    private async Task LoadDrivers()
    {
        Drivers = await ApiHelper.GetAll<ObservableCollection<Driver>>("deliveries/drivers");
    }

    #endregion

    #region ChangeHandlers

    partial void OnCreateDeliveryOrderItemChanged(Order? value)
    {
        if (value is not null)
            CreateDeliveryRequest.OrderId = value.Id;
    }

    partial void OnUpdateDeliveryOrderItemChanged(Order? value)
    {
        if (value is not null)
        {
            UpdateDeliveryRequest.OrderId = value.Id;
        }
    }
    
    partial void OnCreateDeliveryDateChanged(DateTimeOffset? value)
    {
        if (value is not null)
        {
            CreateDeliveryRequest.Date = value.Value.DateTime;
        }
    }

    partial void OnUpdateDeliveryDateChanged(DateTimeOffset? value)
    {
        if (value is not null)
        {
            UpdateDeliveryRequest.Date = value.Value.DateTime;
        }
    }

    partial void OnSelectedDeliveryChanged(Delivery? value)
    {
        if (value is not null)
        {
            UpdateDeliveryOrderItem = Orders.First(o => o.Id == value.Order.Id);
            UpdateDeliveryDate = new DateTimeOffset(value.Date);;
            UpdateDeliveryRequest = new UpdateDeliveryRequest()
            {
                Id = value.Id,
                Date = value.Date,
                OrderId = value.Order.Id,
                Transport = value.Transport,
                Route = value.Route,
                Status = value.Status,
                DriverId = value.Driver?.Id ?? 1
            };
        }
    }

    #endregion
    private bool CanSave() =>
        CreateDeliveryDate != null
        && !string.IsNullOrEmpty(CreateDeliveryRequest.Transport)
        && !string.IsNullOrEmpty(CreateDeliveryRequest.Route);

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            if (!CanSave())
                return;
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
        && UpdateDeliveryRequest.Date != null
        && string.IsNullOrEmpty(UpdateDeliveryRequest.Transport)
        && string.IsNullOrEmpty(UpdateDeliveryRequest.Route);

    [RelayCommand]
    private async Task Update()
    {
        try
        {
            if (!CanUpdate())
                return;
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
            if (!CanDelete())
                return;
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
            if (!CanDriverUpdate())
                return;
            UpdateDeliveryRequest.DriverId = SelectedDriver?.Id ?? 1;
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

    [RelayCommand]
    private async Task StatusUpdate()
    {
        try
        {
            if (!CanStatusUpdate())
                return;
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
