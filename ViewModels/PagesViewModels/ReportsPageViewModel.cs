using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using desktop.Models.Entities;
using desktop.Support.Api;
using desktop.Views.Pages;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace desktop.ViewModels.PagesViewModels;

public partial class ReportsPageViewModel: ViewModelBase
{
    private readonly ContentControl _currentPage;

    public ReportsPageViewModel(ContentControl currentPage)
    {
        _currentPage = currentPage;
    }

    private string GetPath()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\Reports";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    }

    [RelayCommand]
    private async Task CreateStorageRemainsReportCommand()
    {
        try
        {
            var path = GetPath();
            var fileName = "\\Отчёт по остаткам на складе.pfd";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string pathFilePdf = path + "\\" + fileName + ".pdf";
            var pdfWriter = new PdfWriter(pathFilePdf);
            var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument);

            var font = PdfFontFactory.CreateFont("Times.ttc", PdfEncodings.IDENTITY_H);
            document.SetFont(font);
            document.Add(new Paragraph("Отчёт по остаткам на складе").SetFontSize(18));
            document.Add(new Paragraph($"Дата создания отчета: {DateTime.Now:dd.MM.yyyy}").SetFontSize(12));

            var table = new Table(7);
            table.AddHeaderCell("Название продукта");
            table.AddHeaderCell("Артикул");
            table.AddHeaderCell("Категория");
            table.AddHeaderCell("Количество");
            table.AddHeaderCell("Цена");
            table.AddHeaderCell("Расположение");
            table.AddHeaderCell("Зарезервированное количество");

            var products = await ApiHelper.GetAll<List<Product>>("products");
            foreach (var product in products)
            {
                table.AddCell(product.Name);
                table.AddCell(product.Article);
                table.AddCell(product.Category.Name);
                table.AddCell(product.Quantity.ToString());
                table.AddCell(product.Price.ToString());
                table.AddCell(product.Location);
                table.AddCell(product.ReservedQuantity.ToString());
            }

            document.Add(table);
            document.Close();

            await Task.Delay(2000);
            if (File.Exists(pathFilePdf))
                Process.Start(new ProcessStartInfo(pathFilePdf) { UseShellExecute = true });
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
    private async Task CreateCompletedOrdersReportCommand()
    {
        try
        {
            var path = GetPath();
            var fileName = "\\Отчёт по выполненным заказам.pfd";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string pathFilePdf = path + "\\" + fileName + ".pdf";
            var pdfWriter = new PdfWriter(pathFilePdf);
            var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument);

            var font = PdfFontFactory.CreateFont("Times.ttc", PdfEncodings.IDENTITY_H);
            document.SetFont(font);
            document.Add(new Paragraph("Отчёт по выполненным заказам").SetFontSize(18));
            document.Add(new Paragraph($"Дата создания отчета: {DateTime.Now:dd.MM.yyyy}").SetFontSize(12));

            var table = new Table(7);
            table.AddHeaderCell("Номер заказа");
            table.AddHeaderCell("Компания");
            table.AddHeaderCell("Контактное лицо");
            table.AddHeaderCell("Дата заказа");
            table.AddHeaderCell("Артикул");
            table.AddHeaderCell("Количество");
            table.AddHeaderCell("Итоговая сумма");

            var orders = await ApiHelper.GetAll<List<Order>>("orders");
            foreach (var order in orders.Where(o => o.Status == OrderStatus.Completed))
            {
                table.AddCell(order.Id.ToString());
                table.AddCell(order.Client.CompanyName);
                table.AddCell(order.Client.ContactPerson);
                table.AddCell(order.Date.ToString("dd.MM.yyyy"));
                table.AddCell(order.Product.Article);
                table.AddCell(order.Quantity.ToString(""));
                table.AddCell(order.TotalPrice.ToString() + " .руб");
            }

            document.Add(table);
            document.Close();

            await Task.Delay(2000);
            if (File.Exists(pathFilePdf))
                Process.Start(new ProcessStartInfo(pathFilePdf) { UseShellExecute = true });
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
    private async Task CreateRevenueReportCommand()
    {
        try
        {
            var path = GetPath();
            var fileName = "\\Отчёт по доходам от продаж.pfd";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string pathFilePdf = path + "\\" + fileName + ".pdf";
            var pdfWriter = new PdfWriter(pathFilePdf);
            var pdfDocument = new PdfDocument(pdfWriter);
            var document = new Document(pdfDocument);

            var font = PdfFontFactory.CreateFont("Times.ttc", PdfEncodings.IDENTITY_H);
            document.SetFont(font);
            document.Add(new Paragraph("Отчёт по доходам от продаж").SetFontSize(18));
            document.Add(new Paragraph($"Дата создания отчета: {DateTime.Now:dd.MM.yyyy}").SetFontSize(12));

            var table = new Table(8);
            table.AddHeaderCell("Номер заказа");
            table.AddHeaderCell("Дата заказа");
            table.AddHeaderCell("Название продукта");
            table.AddHeaderCell("Цена");
            table.AddHeaderCell("Количество");
            table.AddHeaderCell("Итоговая сумма");

            var orders = await ApiHelper.GetAll<List<Order>>("orders");
            foreach (var order in orders.Where(o => o.Status == OrderStatus.Completed))
            {
                table.AddCell(order.Id.ToString());
                table.AddCell(order.Date.ToString("dd.MM.yyyy"));
                table.AddCell(order.Product.Name);
                table.AddCell(order.Product.Price.ToString() + " .руб");
                table.AddCell(order.Quantity.ToString());
                table.AddCell(order.TotalPrice.ToString() + " .руб");
            }

            document.Add(table);
            document.Close();

            await Task.Delay(2000);
            if (File.Exists(pathFilePdf))
                Process.Start(new ProcessStartInfo(pathFilePdf) { UseShellExecute = true });
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
