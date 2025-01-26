using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.DTOs.Orders;
using desktop.Exceptions;
using desktop.Models.Entities;
using desktop.Support;
using desktop.Support.Api;
using desktop.Views.Pages;
using desktop.Views.Pages.Orders;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels.PagesViewModels.Orders;

public partial class MainOrderPageViewModel: ViewModelBase
{

    #region Filtering
    public List<string> OrderStatuses { get; set; }
    [ObservableProperty] private string _selectedFilterStatus;
    #endregion

    #region Static Properties

    public List<Client> Clients { get; set; } = new ();
    public List<Product> Products { get; set; } = new ();

    #endregion

    #region DataBindings

    [ObservableProperty] private ObservableCollection<Order> _orders;
    [ObservableProperty] private Order? _selectedOrder;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private CreateOrderRequest _createOrder = new CreateOrderRequest();
    [ObservableProperty] private UpdateOrderRequest _updateOrder = new UpdateOrderRequest();

    #endregion

    ContentControl _currentPage;

    public MainOrderPageViewModel(ContentControl currentPage)
    {
        OrderStatuses = ProgramHelper.GetEnumMemberValues<OrderStatus>();
        // Task.Run(() => LoadOrders());
        // Task.Run(() => LoadClients());
        // Task.Run(() => LoadProducts());œ\
    }

    #region Loading Data

    private async Task LoadOrders()
    {
        Orders = await ApiHelper.GetAll<ObservableCollection<Order>>("orders");
    }

    private async Task LoadClients()
    {
        Clients = await ApiHelper.GetAll<List<Client>>("clients");
    }

    private async Task LoadProducts()
    {
        Products = await ApiHelper.GetAll<List<Product>>("products");
    }

    #endregion

    partial void OnSelectedFilterStatusChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            _ = LoadOrders();
            return;
        }
        var enumValue = ProgramHelper.GetEnumValueFromEnumMember<OrderStatus>(value);
        if (enumValue is not null)
            Orders = new ObservableCollection<Order>(Orders.Where(o => o.Status == enumValue));
    }

    partial void OnSelectedOrderChanged(Order? value)
    {
        if (value is not null)
        {
            UpdateOrder.Id = value.Id;
            UpdateOrder.ProductId = value.Product.Id;
            UpdateOrder.ClientId = value.Client.Id;
            UpdateOrder.Date = value.Date;
            UpdateOrder.Status = value.Status;
            UpdateOrder.Quantity = value.Quantity;
            UpdateOrder.TotalPrice = value.TotalPrice;
        }
    }

    private bool CanSave => CreateOrder.ProductId > 0 && CreateOrder.ClientId > 0 && CreateOrder.Quantity > 0;

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task Save()
    {
        try
        {
            CreateOrder.Date = DateTime.Now;
            CreateOrder.TotalPrice =
                Products.First(p => p.Id == CreateOrder.ProductId).Price * CreateOrder.Quantity;
            CreateOrder.Status = OrderStatus.Reserved;
            await ApiHelper.Post<Order, CreateOrderRequest>(CreateOrder, "orders/reserve");
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

    private bool CanUpdate => UpdateOrder.Id > 0 && UpdateOrder.ProductId > 0 && UpdateOrder.ClientId > 0 && UpdateOrder.Quantity > 0;

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task Update()
    {
        try
        {
            UpdateOrder.TotalPrice =
                Products.First(p => p.Id == UpdateOrder.ProductId).Price * UpdateOrder.Quantity;
            await ApiHelper.Put<UpdateOrderRequest>(UpdateOrder, $"orders", UpdateOrder.Id);
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

    private bool CanDelete => SelectedOrder is not null;

    [RelayCommand]
    private async Task Delete()
    {
        try
        {
            await ApiHelper.Delete($"orders", SelectedOrder!.Id);
            Orders.Remove(SelectedOrder);
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
    private async Task GoToMoreInfo()
    {
        _currentPage.Content = new OrderInfoPage(SelectedOrder!, _currentPage);
    }

    [RelayCommand]
    private async Task Back()
    {
        _currentPage.Content = new MainPage();
    }
}
