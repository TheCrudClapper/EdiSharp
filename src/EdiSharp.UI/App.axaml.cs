using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EdiSharp.UI.ViewModels;
using EdiSharp.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EdiSharp.UI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();

            //Top level
            services.AddSingleton<Func<TopLevel?>>(x => () =>
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime topDesktop)
                    return TopLevel.GetTopLevel(topDesktop.MainWindow);

                return null;
            });


            var provider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(provider.GetRequiredService<Func<TopLevel?>>()),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}