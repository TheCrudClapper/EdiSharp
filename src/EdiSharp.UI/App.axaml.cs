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
using EdiSharp.Core.MessageSplitters;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.Core.Services;
using EdiSharp.Core.Services.Edifact;
using EdiSharp.Core.Services.X12;
using EdiSharp.Core.Tokenizers;
using EdiSharp.Core.VersionExtractors;
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

            //Others
            services.AddSingleton<IEdiEncodingDetector, EdifactEncodingDetector>();
            services.AddSingleton<IEdiEncodingDetector, X12EncodingDetector>();
            services.AddSingleton<IEdiDelimiterDetector, EdifactDelimitersDetector>();
            services.AddSingleton<IEdiVersionExtractor, EdifactVersionExtractor>();
            services.AddSingleton<IEdiDelimiterDetector, X12DelimiterDetector>();
            services.AddSingleton<IEdiVersionExtractor, X12VersionExtractor>();

            //Factories
            services.AddSingleton<IEdiTokenizerFactory, EdiTokenizerFactory>();
            services.AddSingleton<IEdiDelimiterDetectorFactory, EdiDelimiterDetectorFactory>();
            services.AddSingleton<IEdiVersionExtractorFactory, EdiVersionExtractorFactory>();
            services.AddSingleton<IDocumentPreviewerServiceFactory, DocumentPreviewerServiceFactory>();
            services.AddSingleton<IEdiEncodingDetectorFactory, EdiEncodingDetectorFactory>();
            services.AddSingleton<IEdiInterchangeBuilderFactory, EdiMessageSplitterFactory>();

            //Services
            services.AddSingleton<IFileInspectionService, FileInspectionService>();
            services.AddSingleton<IEdiProcessingService, EdiProcessingService>();
            services.AddSingleton<IDocumentPreviewerService, X12DocumentPreviewerService>();
            services.AddSingleton<IDocumentPreviewerService, EdifactDocumentPreviewerService>();

            //Tokenizers
            services.AddSingleton<IEdiTokenizer, EdifactTokenizer>();
            services.AddSingleton<IEdiTokenizer, X12Tokenizer>();

            //Message Splitters
            services.AddSingleton<IEdiInterchangeBuilder, EdifactInterchangeBuilder>();
            services.AddSingleton<IEdiInterchangeBuilder, X12InterchangeBuilder>();
            
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
                    DataContext = new MainWindowViewModel(provider.GetRequiredService<Func<TopLevel?>>(), provider.GetRequiredService<IEdiProcessingService>(), provider.GetRequiredService<IFileInspectionService>(), provider.GetRequiredService<IDocumentPreviewerServiceFactory>()),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}