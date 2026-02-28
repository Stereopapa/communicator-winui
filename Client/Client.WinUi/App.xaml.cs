using Client.Core.Services;
using Client.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Client.WUI.Services;



namespace Client
{

    public partial class App : Application
    {
        private Window? _window;
        public static IServiceProvider Services { get; private set; }

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
        }
        
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ConnectViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<IServerConnectionService, ServerConnectionService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMainThreadDispatcher, MainThreadDispatcher>();

            Services = services.BuildServiceProvider();
        }
    }
}
