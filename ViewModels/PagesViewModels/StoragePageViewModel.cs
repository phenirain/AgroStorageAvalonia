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

    [ObservableProperty] private ObservableCollection<ProductCategory> _categories;
    [ObservableProperty] private ObservableCollection<Product> _products;
    private List<Product> _allProducts;
    [ObservableProperty] private Product? _selectedProduct;
    [ObservableProperty] private ProductCategory? _selectedCategory;
    [ObservableProperty]
    private CreateProductRequest _createProduct = new CreateProductRequest();
    [ObservableProperty] private ProductCategory? _createProductCategory;
    [ObservableProperty]
    private UpdateProductRequest _updateProduct = new UpdateProductRequest();

    [ObservableProperty] private ProductCategory? _UpdateProductCategory;


    public StoragePageViewModel(ContentControl currentPage)
    {
        _currentPage = currentPage;
        _ = LoadProducts();
        _ = LoadCategories();
    }

    #region Loading Data

    private async Task LoadProducts()
    {
        Products = await ApiHelper.GetAll<ObservableCollection<Product>>("products");
        _allProducts = Products.ToList();
    }

    private async Task LoadCategories()
    {
        Categories = await ApiHelper.GetAll<ObservableCollection<ProductCategory>>("products/categories");
    }

    #endregion
    
    #region ChangeHandlers

    partial void OnCreateProductCategoryChanged(ProductCategory? value)
    {
        if (value is not null)
            CreateProduct.CategoryId = value.Id;
    }

    partial void OnUpdateProductCategoryChanged(ProductCategory? value)
    {
        if (value is not null)
            UpdateProduct.CategoryId = value.Id;
    }

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
            UpdateProductCategory = Categories.First(c => c.Id == value.Category.Id);
            UpdateProduct = new UpdateProductRequest()
            {
                Id = value.Id,
                Name = value.Name,
                Article = value.Article,
                CategoryId = value.Category.Id,
                Quantity = value.Quantity,
                Price = value.Price,
                Location = value.Location,
                ReservedQuantity = value.ReservedQuantity
            };
        }
    }
    
    #endregion
    
    private bool CanSave =>
        CreateProduct.CategoryId != 0
        && !string.IsNullOrEmpty(CreateProduct.Name)
        && !string.IsNullOrEmpty(CreateProduct.Article)
        && CreateProduct.CategoryId > 0
        && CreateProduct.Price > 0;


    [RelayCommand]
    private async Task Save()
    {
        try
        {
            if (!CanSave)
                return;
            var product = await ApiHelper.Post<Product, CreateProductRequest>(CreateProduct, "products");
            Products.Add(product);
            Products = new ObservableCollection<Product>(Products);
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
        UpdateProduct.CategoryId != 0
        && !string.IsNullOrEmpty(UpdateProduct.Name)
        && !string.IsNullOrEmpty(UpdateProduct.Article)
        && UpdateProduct.CategoryId > 0
        && UpdateProduct.Price > 0;

    [RelayCommand]
    private async Task Update()
    {
        try
        {
            if (!CanUpdate)
                return;
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

    private bool CanDelete => SelectedProduct is not null;

    [RelayCommand]
    private async Task Delete()
    {
        try
        {
            if (!CanDelete)
                return;
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
