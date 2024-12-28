using Avalonia.Media.Imaging;
using Avalonia.Media;
using AvlScope.Lib;
using System.ComponentModel;
using System.Reflection;
using System;
using AvlScope.Common;
using SkiaSharp;
using AvlScope.Ui.Bitmap;
using AvlScope.Uil.Views.Plot;
using System.Threading;
using Avalonia.Threading;
using Avalonia.Rendering;
using System.Linq;
using AvlScope.Ui;

namespace AvlScope.Uil.ViewModels.Renderers;

public class HarmonicViewModel : INotifyPropertyChanged
{
    #region INotifyPropertyChanged members and helpers

    public event PropertyChangedEventHandler PropertyChanged;

    protected static bool AreEqualObjects(object obj1, object obj2)
    {
        var obj1Null = ReferenceEquals(obj1, null);
        var obj2Null = ReferenceEquals(obj2, null);

        if (obj1Null && obj2Null)
            return true;

        if (obj1Null || obj2Null)
            return false;

        if (obj1.GetType() != obj2.GetType())
            return false;

        if (ReferenceEquals(obj1, obj2))
            return true;

        return obj1.Equals(obj2);
    }

    protected void OnPropertyChanged(params string[] propertyNames)
    {
        if (propertyNames == null)
            return;

        if (PropertyChanged != null)
            foreach (var propertyName in propertyNames)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Bitmap Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public WriteableBitmap Bitmap
    {
        get { return _Bitmap; }
        set
        {
            if (AreEqualObjects(_Bitmap, value))
                return;

            var _fieldOldValue = _Bitmap;

            _Bitmap = value;

            OnBitmapChanged(this, new PropertyValueChangedEventArgs<WriteableBitmap>(_fieldOldValue, value));

            OnPropertyChanged("Bitmap");
        }
    }

    private WriteableBitmap _Bitmap;

    public EventHandler<PropertyValueChangedEventArgs<WriteableBitmap>> BitmapChanged;

    public static void OnBitmapChanged(object sender, PropertyValueChangedEventArgs<WriteableBitmap> e)
    {
        var obj = sender as HarmonicViewModel;

        if (obj.BitmapChanged != null)
            obj.BitmapChanged(obj, e);
    }

    #endregion

    #region BitmapContext Property and field

    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public BitmapContext? BitmapContext
    {
        get { return _BitmapContext; }
        set
        {
            if (AreEqualObjects(_BitmapContext, value))
                return;

            var _fieldOldValue = _BitmapContext;

            _BitmapContext = value;

            OnBitmapContextChanged(this, new PropertyValueChangedEventArgs<BitmapContext?>(_fieldOldValue, value));

            OnPropertyChanged("BitmapContext");
        }
    }

    private BitmapContext? _BitmapContext;

    public EventHandler<PropertyValueChangedEventArgs<BitmapContext?>> BitmapContextChanged;

    public static void OnBitmapContextChanged(object sender, PropertyValueChangedEventArgs<BitmapContext?> e)
    {
        var obj = sender as HarmonicViewModel;

        if (obj.BitmapContextChanged != null)
            obj.BitmapContextChanged(obj, e);
    }

    #endregion


    public HarmonicView Parent;

    public void RenderReset()
    {
        if (BitmapContext == null)
        {
            return;
        }


        throw new Exception();

        //BitmapContext.Value.Clear();
    }

    static Random random = new Random();

    static SKColor[] cl = new SKColor[2] { SKColors.Blue, SKColors.Red };

    int couner = 0;


    public void UpdateImageSize(int w, int h)
    {
        /*
        Parent.RenderMutex.WaitOne();

        try
        {
            Bitmap = BitmapFactory.New(w, h);
            BitmapContext = BitmapFactory.GetBitmapContext(Bitmap);
        }
        finally
        {
            Parent.RenderMutex.ReleaseMutex();
        }

        */
    }


}