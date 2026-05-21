using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EdiSharp.Core.Abstractions;
using EdiSharp.Core.DelimiterDetectors;
using EdiSharp.Core.EncodingDetectors;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.Factories.Implementations;
using EdiSharp.Core.Interfaces;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.Core.Services;
using EdiSharp.Core.Tokenizers;
using EdiSharp.UI.ViewModels;
using EdiSharp.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            services.AddSingleton<IEdiProcessingService, EdiProcessingService>();
            services.AddSingleton<IEdiTokenizerFactory, EdiTokenizerFactory>();
            services.AddSingleton<IEdiTokenizer, EdifactTokenizer>();
            services.AddSingleton<IFileInspectionService, FileInspectionService>();
            services.AddSingleton<IEdiTokenizer, X12Tokenizer>();
            services.AddSingleton<IEdiEncodingDetectorFactory, EdiEncodingDetectorFactory>();
            services.AddSingleton<IEdiEncodingDetector, EdifactEncodingDetector>();
            services.AddSingleton<IEdiEncodingDetector, X12EncodingDetector>();
            services.AddSingleton<IEdiDelimiterDetector, EdifactDelimitersDetector>();
            services.AddSingleton<IEdiDelimiterDetectorFactory, EdiDelimiterDetectorFactory>();

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
                    DataContext = new MainWindowViewModel(provider.GetRequiredService<Func<TopLevel?>>(), provider.GetRequiredService<IEdiProcessingService>(), provider.GetRequiredService<IFileInspectionService>()),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}