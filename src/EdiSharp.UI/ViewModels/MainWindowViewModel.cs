using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace EdiSharp.UI.ViewModels;

public partial class MainWindowViewModel(Func<TopLevel?> topLevelAccessor) : ViewModelBase
{
    
    //MVVM Fields

    [ObservableProperty]
    private bool _validate = false;

    [ObservableProperty]
    private bool _showRawSegments = false;

    [ObservableProperty]
    private bool _isJsonChecked = false;

    [ObservableProperty]
    private bool _isXmlChecked = false;

    [ObservableProperty]
    private string _fileName = "Choose EDIFACT File";

    [RelayCommand]
    public async Task PickFile()
    {
        var topLevel = topLevelAccessor();
        if (topLevel is null)
            return;

        var file = await topLevel.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions()
        {
            AllowMultiple = false,
        });
    }
    
    [RelayCommand]
    public async Task Parse() 
    {
        Console.WriteLine("Hello");
    }

}
