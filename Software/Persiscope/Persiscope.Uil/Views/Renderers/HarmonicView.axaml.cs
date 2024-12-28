using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvlScope.Lib;
using System.Globalization;
using System.Linq;
using AvlScope.Common;
using static System.Net.Mime.MediaTypeNames;
using Avalonia.Controls.Documents;
using Avalonia.Threading;
using System.Threading.Tasks;
using AvlScope.Ui;
using AvlScope.Uil.ViewModels.Renderers;

namespace AvlScope.Uil.Views.Renderers;

public partial class HarmonicView : UserControl
{

    public Avalonia.Controls.Image GetImage()
    {
        return this.FindControl<Avalonia.Controls.Image>("image");
    }



    public int CyclesToShow = 2;//how many full cycles in scope UI, mor of it, more tooth in UI
    public int NumberOfCyclesToRender = 4;//how many full cycles in scope UI?, more of it denser lines in UI
    public int MinPointsToRender = 1000;//number of samples to show on UI, more samples denser graph


    public HarmonicView()
        : base()
    {
        //InitializeComponent();
        //DataContext = Context = new HarmonicViewModel() { Parent = this };

        //NeedSignalPropertyCalculation = true;

        //AvaloniaXamlLoader.Load(this);

        //NeedSamplesResort = false;
        //NeedSignalPropertyCalculation = false;
    }

    HarmonicViewModel Context;
    public void Activate()
    {
        //RuntimeVariables.DisableSignalPropertyCalculation = false;
        //this.NeedSamplesResort = false;
        //this.NeedSignalPropertyCalculation = false;
    }

    public void DeActivate()
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

    public void Init()
    {
    }


    public void Reset()
    {
        Context.RenderReset();
    }

    private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var img = sender as Border;

        var sz = img.DesiredSize;

        var w = (int)img.Bounds.Width;
        var h = (int)img.Bounds.Height;

        Task.Run(() =>
        {
            Context.UpdateImageSize(w, h);
        });//Context.UpdateImageSize must call from another thread except main thread
    }

    public int[] GetActiveChannels()
    {
        return new int[] { 0, 1, 2 };
    }

    public void RenderFrame(DataRepositorySnapshot shot)
    {
        //Context.RenderShot(shot);
        Dispatcher.UIThread.Invoke(() => GetImage().InvalidateVisual());
    }

    public void RenderFrame(UiDataRepositorySnapshot shot)
    {
        //Context.RenderShot(shot);
        Dispatcher.UIThread.Invoke(() => GetImage().InvalidateVisual());
    }
}
