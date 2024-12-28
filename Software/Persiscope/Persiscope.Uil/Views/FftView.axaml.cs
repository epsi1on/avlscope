using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Persiscope.Uil.Views.Plot;

namespace Persiscope.Uil;

public partial class FftView : BaseFpsRenderView
{
    public FftView():
         base()
    {
        AvaloniaXamlLoader.Load(this);
    }
}