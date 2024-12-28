using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvlScope.Lib;
using AvlScope.Ui.Renderers;
using System.Globalization;
using System.Linq;
using AvlScope.Common;
using static System.Net.Mime.MediaTypeNames;
using Avalonia.Controls.Documents;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace AvlScope.Ui.Renderers.Harmonic;

public partial class HarmonicRenderer : BaseFpsRender
{

    public Avalonia.Controls.Image GetImage()
    {
        return this.FindControl<Avalonia.Controls.Image>("image");
    }

    

    public int CyclesToShow = 2;//how many full cycles in scope UI, mor of it, more tooth in UI
    public int NumberOfCyclesToRender = 4;//how many full cycles in scope UI?, more of it denser lines in UI
    public int MinPointsToRender = 1000;//number of samples to show on UI, more samples denser graph


    public HarmonicRenderer()
        : base()
    {
        //InitializeComponent();
        DataContext = Context = new HarmonicContextClass() { Parent = this };

        NeedSignalPropertyCalculation = true;

        AvaloniaXamlLoader.Load(this);

        this.NeedSamplesResort = false;
        this.NeedSignalPropertyCalculation = false;
    }

    HarmonicContextClass Context;
    public override void Activate()
    {
        //RuntimeVariables.DisableSignalPropertyCalculation = false;
        //this.NeedSamplesResort = false;
        //this.NeedSignalPropertyCalculation = false;
    }

    public override void DeActivate()
    {
    }

    private void HarmonicRenderer_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var img = sender as Border;

        var sz = img.DesiredSize;

        var w = (int)img.Bounds.Width;
        var h = (int)img.Bounds.Height;

        Context.UpdateImageSize(w, h);
    }

    public override void Init()
    {
    }


    public override void Reset()
    {
        Context.RenderReset();
    }

    private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var img = sender as Border;

        var sz = img.DesiredSize;

        var w = (int)img.Bounds.Width;
        var h = (int)img.Bounds.Height;

        Task.Run(() => {
            Context.UpdateImageSize(w, h);
        });//Context.UpdateImageSize must call from another thread except main thread
    }

    public override int[] GetActiveChannels()
    {
        return new int[] { 0, 1, 2 };
    }

    public void RenderFrame(DataRepositorySnapshot shot)
    {
        Context.RenderShot(shot);
        Dispatcher.UIThread.Invoke(() => this.GetImage().InvalidateVisual());
    }

    public override void RenderFrame(UiDataRepositorySnapshot shot)
    {
        Context.RenderShot(shot);
        Dispatcher.UIThread.Invoke(() => this.GetImage().InvalidateVisual());
    }
}
