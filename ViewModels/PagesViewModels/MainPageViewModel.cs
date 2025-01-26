using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using desktop.Models.Entities;
using desktop.Support;
using desktop.Support.Api;

namespace desktop.ViewModels.PagesViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    public List<string> DeliveryStatuses { get; set; } = ProgramHelper.GetEnumMemberValues<DeliveryStatus>();
    [ObservableProperty] private string? _selectedDeliveryStatus;
    [ObservableProperty] private ObservableCollection<Product> _products;
    [ObservableProperty] private ObservableCollection<Delivery> _deliveries;
    private List<Delivery> deliveries;
    public List<Delivery> ScheduledDeliveries { get; set; }
    public List<Delivery> ActiveDeliveries { get; set; }
    public List<Delivery> CompletedDeliveries { get; set; }

    public MainPageViewModel()
    {
        // _ = LoadDeliveries();
        // _ = LoadOrders();
        // _ = LoadProducts();
    }

    #region Loading Data

    private async Task LoadDeliveries()
    {
        Deliveries = await ApiHelper.GetAll<ObservableCollection<Delivery>>("deliveries");
        deliveries = Deliveries.ToList();
        ScheduledDeliveries = Deliveries.Where(d => d.Status == DeliveryStatus.Scheduled).ToList();
        ActiveDeliveries = Deliveries.Where(d => d.Status == DeliveryStatus.OnTheWay).ToList();
        CompletedDeliveries = Deliveries.Where(d => d.Status == DeliveryStatus.Completed).ToList();
    }


    private async Task LoadProducts()
    {
        Products = await ApiHelper.GetAll<ObservableCollection<Product>>("products");
    }

    #endregion

    partial void OnSelectedDeliveryStatusChanged(string? value)
    {
        if (value is not null && string.IsNullOrEmpty(value))
        {
            var status = ProgramHelper.GetEnumValueFromEnumMember<DeliveryStatus>(value);
            if (status is not null)
                Deliveries = new ObservableCollection<Delivery>(deliveries.Where(o => o.Status == status));
        }
        else
        {
            Deliveries = new ObservableCollection<Delivery>(deliveries);
        }
    }
}
