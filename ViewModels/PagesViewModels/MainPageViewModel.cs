using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using desktop.Models.Entities;
using desktop.Support;
using desktop.Support.Api;

namespace desktop.ViewModels.PagesViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public List<string> DeliveryStatuses { get; set; } = ProgramHelper.GetEnumMemberValues<DeliveryStatus>();
    [ObservableProperty] private string? _selectedDeliveryStatus;
    [ObservableProperty] private ObservableCollection<Product> _products;
    [ObservableProperty] private ObservableCollection<Delivery> _deliveries;
    private List<Delivery> deliveries;
    [ObservableProperty] private ObservableCollection<Delivery> _scheduledDeliveries;
    [ObservableProperty] private ObservableCollection<Delivery> _activeDeliveries;
    [ObservableProperty] private ObservableCollection<Delivery> _completedDeliveries;

    public MainPageViewModel()
    {
        _ = LoadDeliveries();
        _ = LoadProducts();
    }

    #region Loading Data

    private async Task LoadDeliveries()
    {
        Deliveries = await ApiHelper.GetAll<ObservableCollection<Delivery>>("deliveries");
        deliveries = Deliveries.ToList();
        ScheduledDeliveries = new ObservableCollection<Delivery>(Deliveries.Where(d => d.Status == DeliveryStatus.Scheduled).ToList());
        ActiveDeliveries = new ObservableCollection<Delivery>(Deliveries.Where(d => d.Status == DeliveryStatus.OnTheWay).ToList());
        CompletedDeliveries = new ObservableCollection<Delivery>(Deliveries.Where(d => d.Status == DeliveryStatus.Completed).ToList());
    }


    private async Task LoadProducts()
    {
        Products = await ApiHelper.GetAll<ObservableCollection<Product>>("products");
        var a = Products;
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
