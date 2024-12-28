﻿using Avalonia.Controls.Templates;
using Avalonia.Controls;
using Persiscope.Uil.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persiscope.Uil.Views;
using Persiscope.Uil.ViewModels.Plot;
using Persiscope.Uil.Views.Plot;

namespace Persiscope.Uil
{
    public class ViewLocator : IDataTemplate
    {
        private readonly Dictionary<Type, Func<Control?>> _locator = new();

        public ViewLocator()
        {
            RegisterViewFactory<MainViewModel, MainView>();
            RegisterViewFactory<ConnectViewModel, ConnectionView>();
            RegisterViewFactory<AboutViewModel, AboutView>();
            RegisterViewFactory<SettingsViewModel, SettingsView>();
            RegisterViewFactory<EkgViewModel, EkgView>();
            RegisterViewFactory<HarmonicViewModel, HarmonicView>();
            RegisterViewFactory<FftViewModel, FftView>();

            /** /
            RegisterViewFactory<HomePageViewModel, HomePageView>();
            RegisterViewFactory<ButtonPageViewModel, ButtonPageView>();
            RegisterViewFactory<TextPageViewModel, TextPageView>();
            RegisterViewFactory<ValueSelectionPageViewModel, ValueSelectionPageView>();
            RegisterViewFactory<ImagePageViewModel, ImagePageView>();
            RegisterViewFactory<GridPageViewModel, GridPageView>();
            RegisterViewFactory<DragAndDropPageViewModel, DragAndDropPageView>();
            RegisterViewFactory<CustomSplashScreenViewModel, CustomSplashScreenView>();
            RegisterViewFactory<LoginPageViewModel, LoginPageView>();
            RegisterViewFactory<SecretViewModel, SecretView>();
            RegisterViewFactory<ChartsPageViewModel, ChartsPageView>();
            /**/
        }

        public Control Build(object? data)
        {
            if (data is null)
            {
                return new TextBlock { Text = "No VM provided" };
            }

            _locator.TryGetValue(data.GetType(), out var factory);

            return factory?.Invoke() ?? new TextBlock { Text = $"VM Not Registered: {data.GetType()}" };
        }

        public bool Match(object? data)
        {
            return data is ObservableObject;
        }

        private void RegisterViewFactory<TViewModel, TView>()
            where TViewModel : class
            where TView : Control
            => _locator.Add(
                typeof(TViewModel),
                Design.IsDesignMode
                    ? Activator.CreateInstance<TView>
                    : Ioc.Default.GetService<TView>);
    }
}
