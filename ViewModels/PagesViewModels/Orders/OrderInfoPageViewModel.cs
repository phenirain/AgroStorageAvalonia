using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.DTOs.Orders;
using desktop.Models.Entities;
using desktop.Support;
using desktop.Support.Api;
using desktop.Views.Pages.Orders;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels.PagesViewModels.Orders;

public partial class OrderInfoPageViewModel: ViewModelBase
{

    public List<string> OrderStatuses { get; set; } = ProgramHelper.GetEnumMemberValues<OrderStatus>();

    [ObservableProperty] private Order _order;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateStatusCommand))]
    private string _status;
    private OrderStatus _lastStatus;
    private readonly UpdateOrderRequest _updateOrder = new UpdateOrderRequest();

    private ContentControl _currentPage;

    public OrderInfoPageViewModel(Order order, ContentControl currentPage)
    {
        Order = order;
        _lastStatus = order.Status;
        _currentPage = currentPage;
    }

    [RelayCommand]
    private async Task Back()
    {
        _currentPage.Content = new MainOrderPage(_currentPage);
    }

    private bool CanUpdateStatus =>
        (_lastStatus != OrderStatus.Completed
         || _lastStatus != OrderStatus.Canceled
         || _lastStatus != OrderStatus.Delivering)
        && string.IsNullOrEmpty(Status);

    [RelayCommand]
    private async Task UpdateStatus()
    {
        if (!CanUpdateStatus)
            return;
        _updateOrder.Id = Order.Id;
        _updateOrder.ProductId = Order.Product.Id;
        _updateOrder.ClientId = Order.Client.Id;
        _updateOrder.Date = Order.Date;
        _updateOrder.Status = ProgramHelper.GetEnumValueFromEnumMember<OrderStatus>(Status)!.Value;
        _updateOrder.Quantity = Order.Quantity;
        _updateOrder.TotalPrice = Order.TotalPrice;

        try
        {
            if (_updateOrder.Status == OrderStatus.Completed)
            {
                await ApiHelper.Put<UpdateOrderRequest>(_updateOrder, "orders/complete", _updateOrder.Id);
                return;
            }
            await ApiHelper.Put<UpdateOrderRequest>(_updateOrder, $"orders", Order.Id);
        }
        catch (System.Exception ex)
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
}
