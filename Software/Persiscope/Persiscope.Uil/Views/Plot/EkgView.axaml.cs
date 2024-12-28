using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Persiscope.Uil.Views.Plot;

public partial class EkgView : BaseFpsRenderView
{
    public EkgView() :
        base()
    {
        AvaloniaXamlLoader.Load(this);
    }

}