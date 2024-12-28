using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvlScope.Lib;
using AvlScope.Ui.Renderers;
using AvlScope.Ui.Renderers.Ekg;
using AvlScope.Ui.Renderers.Harmonic;
using System.Threading;
using System.Threading.Tasks;

namespace AvlScope.Ui;

public partial class EkgRenderer : BaseFpsRender
{
    public Avalonia.Controls.Image GetImage()
    {
        return this.FindControl<Avalonia.Controls.Image>("image");
    }

    public EkgRenderer() : base()
    {
        DataContext = Context = new EkgContextClass() { Parent = this };

        NeedSignalPropertyCalculation = true;

        AvaloniaXamlLoader.Load(this);

        this.NeedSamplesResort = false;
        this.NeedSignalPropertyCalculation = false;
    }

    EkgContextClass Context;

    public override void Activate()
    {
        //RuntimeVariables.DisableSignalPropertyCalculation = true;

        
    }

    public override void DeActivate()
    {

    }

    public override int[] GetActiveChannels()
    {
        return new int[] { 0, 1, 2 };
    }

    public override void Init()
    {

    }

    public void RenderFrame(DataRepositorySnapshot shot)
    {
        //Context.RenderShot(shot);
        Dispatcher.UIThread.Invoke(() => this.GetImage().InvalidateVisual());
    }

    public override void RenderFrame(UiDataRepositorySnapshot shot)
    {
        Context.RenderShot(shot);
        Dispatcher.UIThread.Invoke(() => this.GetImage().InvalidateVisual());
    }

    public override void Reset()
    {
        //throw new System.NotImplementedException();
    }

    private void image_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var img = sender as Border;

        var sz = img.DesiredSize;

        var w = (int)img.Bounds.Width;
        var h = (int)img.Bounds.Height;

        Task.Run(() => {
            Context.UpdateImageSize(w, h);
        });//Context.UpdateImageSize must call from another thread except main thread
    }

}