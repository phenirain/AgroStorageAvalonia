using Avalonia.Controls;

namespace desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AuthWindow w = new AuthWindow();
        w.Show();
        this.Close();
    }
}