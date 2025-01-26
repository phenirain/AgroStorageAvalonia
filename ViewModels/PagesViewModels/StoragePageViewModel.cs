using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using desktop.DTOs.Products;
using desktop.Models.Entities;
using desktop.Support.Api;
using desktop.Views.Pages;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels.PagesViewModels;

public partial class StoragePageViewModel: ViewModelBase
{
    private readonly ContentControl _currentPage;

    public List<ProductCategory> Categories { get; set; }
    [ObservableProperty] private ObservableCollection<Product> _products;
    private List<Product> _allProducts;
    [ObservableProperty] private Product? _selectedProduct;
    [ObservableProperty] private ProductCategory? _selectedCategory;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private CreateProductRequest _createProduct = new CreateProductRequest();
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    private UpdateProductRequest _updateProduct = new UpdateProductRequest();


    public StoragePageViewModel(ContentControl currentPage)
    {
        _currentPage = currentPage;
        // _ = LoadProducts();
        // _ = LoadCategories();
    }

    #region Loading Data

    private async Task LoadProducts()
    {
        Products = await ApiHelper.GetAll<ObservableCollection<Product>>("products");
        _allProducts = Products.ToList();
    }

    private async Task LoadCategories()
    {
        Categories = await ApiHelper.GetAll<List<ProductCategory>>("products/categories");
    }

    #endregion

    partial void OnSelectedCategoryChanged(ProductCategory? value)
    {
        if (value is not null)
        {
            Products = new ObservableCollection<Product>(_allProducts.Where(p => p.Category.Id == value.Id));
        }
        else
        {
            Products = new ObservableCollection<Product>(_allProducts);
        }
    }

    partial void OnSelectedProductChanged(Product? value)
    {
        if (value is not null)
        {
            UpdateProduct.Id = value.Id;
            UpdateProduct.Name = value.Name;
            UpdateProduct.Article = value.Article;
            UpdateProduct.CategoryId = value.Category.Id;
            UpdateProduct.Quantity = value.Quantity;
            UpdateProduct.Price = value.Price;
            UpdateProduct.Location = value.Location;
            UpdateProduct.ReservedQuantity = value.ReservedQuantity;
        }
    }

    private bool CanSave() =>
        CreateProduct.CategoryId != 0
        && !string.IsNullOrEmpty(CreateProduct.Name)
        && !string.IsNullOrEmpty(CreateProduct.Article)
        && CreateProduct.CategoryId > 0
        && CreateProduct.Price > 0;


    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task Save()
    {
        try
        {
            var product = await ApiHelper.Post<Product, CreateProductRequest>(CreateProduct, "products");
            Products.Add(product);
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
        UpdateProduct.CategoryId != 0
        && !string.IsNullOrEmpty(UpdateProduct.Name)
        && !string.IsNullOrEmpty(UpdateProduct.Article)
        && UpdateProduct.CategoryId > 0
        && UpdateProduct.Price > 0;

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task Update()
    {
        try
        {
            await ApiHelper.Put(UpdateProduct, $"products", UpdateProduct.Id);
            var product = _allProducts.First(p => p.Id == UpdateProduct.Id);
            product.Name = UpdateProduct.Name;
            product.Article = UpdateProduct.Article;
            product.Category = Categories.First(c => c.Id == UpdateProduct.CategoryId);
            product.Quantity = UpdateProduct.Quantity;
            product.Price = UpdateProduct.Price;
            product.Location = UpdateProduct.Location;
            product.ReservedQuantity = UpdateProduct.ReservedQuantity;
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

    private bool CanDelete() => SelectedProduct is not null;

    [RelayCommand]
    private async Task Delete()
    {
        try
        {
            await ApiHelper.Delete($"products", SelectedProduct.Id);
            Products.Remove(SelectedProduct);
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
