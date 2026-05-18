using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace EdiSharp.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
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
        Console.WriteLine("Picking File");
    }
    
    [RelayCommand]
    public async Task Parse() 
    {
        Console.WriteLine("Hello");
    }

}
