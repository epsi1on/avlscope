using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Persiscope.Uil.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    public MainView(ViewModels.MainViewModel vm)
    {
        InitializeComponent();
    }
}