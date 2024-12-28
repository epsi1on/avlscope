using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Persiscope.Uil.Views.Plot;

public partial class HarmonicView : BaseFpsRenderView
{
    public HarmonicView():     base()
    {
        AvaloniaXamlLoader.Load(this);
    }

}