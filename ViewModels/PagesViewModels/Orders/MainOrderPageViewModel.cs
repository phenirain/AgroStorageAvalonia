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

    [ObservableProperty]private ObservableCollection<Client> _clients = new ();
    [ObservableProperty]private ObservableCollection<Product> _products = new ();

    #endregion

    #region DataBindings

    [ObservableProperty] private ObservableCollection<Order> _orders;
    private List<Order> _allOrders;
    [ObservableProperty] private Order? _selectedOrder;
    [ObservableProperty]
    private CreateOrderRequest _createOrder = new CreateOrderRequest();
    [ObservableProperty] private Client? _createOrderClient;
    [ObservableProperty] private Product? _createOrderProduct;
    [ObservableProperty] private UpdateOrderRequest _updateOrder = new UpdateOrderRequest();
    [ObservableProperty] private Client? _updateOrderClient;
    [ObservableProperty] private Product? _updateOrderProduct;

    #endregion

    ContentControl _currentPage;

    public MainOrderPageViewModel(ContentControl currentPage)
    {
        OrderStatuses = ProgramHelper.GetEnumMemberValues<OrderStatus>();
        _currentPage = currentPage;
        Task.Run(() => LoadOrders());
        Task.Run(() => LoadClients());
        Task.Run(() => LoadProducts());
    }

    #region Loading Data

    private async Task LoadOrders()
    {
        Orders = await ApiHelper.GetAll<ObservableCollection<Order>>("orders");
        _allOrders = Orders.ToList();
    }

    private async Task LoadClients()
    {
        Clients = await ApiHelper.GetAll<ObservableCollection<Client>>("clients");
    }

    private async Task LoadProducts()
    {
        Products = await ApiHelper.GetAll<ObservableCollection<Product>>("products");
    }

    #endregion
    
    #region Change Handlers

    partial void OnCreateOrderClientChanged(Client? value)
    {
        if (value is not null)
            CreateOrder.ClientId = value.Id;
    }

    partial void OnCreateOrderProductChanged(Product? value)
    {
        if (value is not null)
            CreateOrder.ProductId = value.Id;
    }
    
    partial void OnUpdateOrderClientChanged(Client? value)
    {
        if (value is not null) 
            UpdateOrder.ClientId = value.Id;
    }
    
    partial void OnUpdateOrderProductChanged(Product? value)
    {
        if (value is not null)
            UpdateOrder.ProductId = value.Id;
    }


    partial void OnSelectedFilterStatusChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Orders = new ObservableCollection<Order>(_allOrders);
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
            UpdateOrderClient = Clients.First(c => c.Id == value.Client.Id);
            UpdateOrderProduct = Products.First(p => p.Id == value.Product.Id);
            UpdateOrder = new UpdateOrderRequest()
            {
                Id = value.Id,
                ProductId = value.Product.Id,
                ClientId = value.Client.Id,
                Date = value.Date,
                Status = value.Status,
                Quantity = value.Quantity,
            };
        }
    }
    #endregion

    private bool CanSave => CreateOrder.ProductId > 0 && CreateOrder.ClientId > 0 && CreateOrder.Quantity > 0;

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            if (!CanSave)
                return;
            CreateOrder.Date = DateTime.Now;
            CreateOrder.TotalPrice =
                Products.First(p => p.Id == CreateOrder.ProductId).Price * CreateOrder.Quantity;
            CreateOrder.Status = OrderStatus.Reserved;
            var order = await ApiHelper.Post<Order, CreateOrderRequest>(CreateOrder, "orders/reserve");
            Orders.Add(order);
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

    [RelayCommand]
    private async Task Update()
    {
        try
        {
            if (!CanUpdate)
                return;
            UpdateOrder.TotalPrice =
                Products.First(p => p.Id == UpdateOrder.ProductId).Price * UpdateOrder.Quantity;
            await ApiHelper.Put<UpdateOrderRequest>(UpdateOrder, $"orders", UpdateOrder.Id);
            var order = _allOrders.First(o => o.Id == UpdateOrder.Id);
            order.Product = Products.First(p => p.Id == UpdateOrder.ProductId);
            order.Client = Clients.First(c => c.Id == UpdateOrder.ClientId);
            order.TotalPrice = UpdateOrder.TotalPrice;
            order.Quantity = UpdateOrder.Quantity;
            order.Status = UpdateOrder.Status;
            Orders = new ObservableCollection<Order>(_allOrders);
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
            if (!CanUpdate)
                return;
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
