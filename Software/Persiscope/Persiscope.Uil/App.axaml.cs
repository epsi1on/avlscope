using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
//using Persiscope.Uil.Services;
using Persiscope.Uil.ViewModels;
//using Persiscope.Uil.ViewModels.SplitViewPane;
using Persiscope.Uil.Views;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
//using Persiscope.Uil.Services;
//using Persiscope.Uil.ViewModels.SplitViewPane;
//using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Persiscope.Uil.Views.Plot;
using Persiscope.Uil.ViewModels.Plot;
//using Microsoft.Extensions.DependencyInjection;



namespace Persiscope.Uil
{
    public partial class App : Application
    {

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            var tt = this.ActualThemeVariant;

        }

        public override void OnFrameworkInitializationCompleted()
        {
            var locator = new ViewLocator();
            DataTemplates.Add(locator);

            var services = new ServiceCollection();
            ConfigureViewModels(services);
            ConfigureViews(services);
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            // Typed-clients
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0#typed-clients
            //services.AddHttpClient<ILoginService, LoginService>(httpClient => httpClient.BaseAddress = new Uri("https://dummyjson.com/"));

            var provider = services.BuildServiceProvider();

            Ioc.Default.ConfigureServices(provider);

            var vm = Ioc.Default.GetRequiredService<MainViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView { DataContext = vm };
            }

            base.OnFrameworkInitializationCompleted();
        }

        [Singleton(typeof(MainViewModel))]
        [Singleton(typeof(ConnectViewModel))]
        [Singleton(typeof(AboutViewModel))]
        [Singleton(typeof(SettingsViewModel))]
        [Singleton(typeof(EkgViewModel))]
        [Singleton(typeof(HarmonicViewModel))]
        [Singleton(typeof(FftViewModel))]
        /*
        [Transient(typeof(ButtonPageViewModel))]
        [Transient(typeof(TextPageViewModel))]
        [Transient(typeof(ValueSelectionPageViewModel))]
        [Transient(typeof(ImagePageViewModel))]
        [Singleton(typeof(GridPageViewModel))]
        [Singleton(typeof(DragAndDropPageViewModel))]
        [Singleton(typeof(CustomSplashScreenViewModel))]
        [Singleton(typeof(LoginPageViewModel))]
        [Singleton(typeof(SecretViewModel))]
        [Transient(typeof(ChartsPageViewModel))]
        */
        [SuppressMessage("CommunityToolkit.Extensions.DependencyInjection.SourceGenerators.InvalidServiceRegistrationAnalyzer", "TKEXDI0004:Duplicate service type registration")]
        internal static partial void ConfigureViewModels(IServiceCollection services);


        [Singleton(typeof(MainView))]
        [Singleton(typeof(ConnectionView))]
        [Singleton(typeof(AboutView))]
        [Singleton(typeof(SettingsView))]
        [Singleton(typeof(EkgView))]
        [Singleton(typeof(HarmonicView))]
        [Singleton(typeof(FftView))]
        /*
        [Transient(typeof(ButtonPageView))]
        [Transient(typeof(TextPageView))]
        [Transient(typeof(ValueSelectionPageView))]
        [Transient(typeof(ImagePageView))]
        [Transient(typeof(GridPageView))]
        [Transient(typeof(DragAndDropPageView))]
        [Transient(typeof(CustomSplashScreenView))]
        [Transient(typeof(LoginPageView))]
        [Transient(typeof(SecretView))]
        [Transient(typeof(ChartsPageView))]
        */
        internal static partial void ConfigureViews(IServiceCollection services);
    }
}
