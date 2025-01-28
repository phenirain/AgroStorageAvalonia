using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using desktop.ViewModels;

namespace desktop.Views;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        DataContext = new AuthWindowViewModel();
    }
}