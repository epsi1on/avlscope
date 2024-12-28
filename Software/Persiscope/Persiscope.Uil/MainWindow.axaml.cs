using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Persiscope.Uil.ViewModels;

namespace Persiscope.Uil;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        this.DataContext = new MainWindowModel();

#if DEBUG
        //if (attachDevTools)
        {
            //this.AttachDevTools();
        }
#endif
    }


}