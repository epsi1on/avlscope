using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace AvaloniaApplicationTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

			this.DataContext = this.Context = new ParentClass();

			Context.Init();

			new Thread(RenderLoop).Start();
        }

		ParentClass Context;

        private void RenderLoop()
		{
			var cnt = 0;

			while (true)
			{
				//Thread.Sleep(1);
				Context.Label = (cnt++).ToString();

				var lc = Context.Bmp.Lock();

				var ctx = new BitmapContext(lc, ReadWriteMode.ReadWrite);

				ctx.SetPixeli(cnt, int.MaxValue);


				lc.Dispose();

				Dispatcher.UIThread.Invoke(new Action(() => { img.InvalidateVisual(); }));
            }
		}
    }

    public class ParentClass:INotifyPropertyChanged
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

			if (this.PropertyChanged != null)
				foreach (var propertyName in propertyNames)
					this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion


		#region Label Property and field

		[Obfuscation(Exclude = true, ApplyToMembers = false)]
		public string Label
		{
			get { return _Label; }
			set
			{
				if (AreEqualObjects(_Label, value))
					return;

				var _fieldOldValue = _Label;

				_Label = value;

				ParentClass.OnLabelChanged(this, new PropertyValueChangedEventArgs<string>(_fieldOldValue, value));

				this.OnPropertyChanged("Label");
			}
		}

		private string _Label;

		public EventHandler<PropertyValueChangedEventArgs<string>> LabelChanged;

		public static void OnLabelChanged(object sender, PropertyValueChangedEventArgs<string> e)
		{
			var obj = sender as ParentClass;

			if (obj.LabelChanged != null)
				obj.LabelChanged(obj, e);
		}

		#endregion

		#region Bmp Property and field

		[Obfuscation(Exclude = true, ApplyToMembers = false)]
		public WriteableBitmap Bmp
		{
			get { return _Bmp; }
			set
			{
				if (AreEqualObjects(_Bmp, value))
					return;

				var _fieldOldValue = _Bmp;

				_Bmp = value;

				ParentClass.OnBmpChanged(this, new PropertyValueChangedEventArgs<WriteableBitmap>(_fieldOldValue, value));

				this.OnPropertyChanged("Bmp");
			}
		}

		private WriteableBitmap _Bmp;

		public EventHandler<PropertyValueChangedEventArgs<WriteableBitmap>> BmpChanged;

		public static void OnBmpChanged(object sender, PropertyValueChangedEventArgs<WriteableBitmap> e)
		{
			var obj = sender as ParentClass;

			if (obj.BmpChanged != null)
				obj.BmpChanged(obj, e);
		}

		#endregion



		public void Init()
		{
			var dpi = new Vector(100, 100);
			var sz = PixelSize.FromSizeWithDpi(new Size(300, 300), 96);


            this.Bmp = new WriteableBitmap(
				sz,
				dpi,
				Avalonia.Platform.PixelFormats.Rgb32,
				Avalonia.Platform.AlphaFormat.Opaque);

        }

	}

}