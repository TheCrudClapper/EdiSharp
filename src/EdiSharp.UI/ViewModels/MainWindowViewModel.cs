using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EdiSharp.Core.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EdiSharp.UI.ViewModels;

public partial class MainWindowViewModel(Func<TopLevel?> topLevelAccessor)
    : ViewModelBase
{

    #region Fields

    [ObservableProperty]
    private bool _validate = false;

    [ObservableProperty]
    private bool _showRawSegments = false;

    [ObservableProperty]
    private bool _isJsonChecked = false;

    public bool IsErrorVisible 
        => Error is not null;

    [ObservableProperty]
    private bool _isXmlChecked = false;

    [ObservableProperty]
    private string? _error;

    [ObservableProperty]
    private string _fileName = "Choose EDIFACT File";

    public Stream? Stream { get; set; }

    private InputType _inputType;
    #endregion

    partial void OnErrorChanged(string? value)
       => OnPropertyChanged(nameof(IsErrorVisible));

    [RelayCommand]
    public async Task PickFile()
    {
        var topLevel = topLevelAccessor();
        if (topLevel is null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
        });

        if (files.Count == 0)
            return;

        var file = files[0];

        var extension = Path.GetExtension(file.Name);
        if (extension != ".edi" 
            && extension != ".edifact" 
            && extension != ".x12" 
            && extension != ".txt")
        {
            Error = "Wrong file extension. Supported types: .edi, .edifact, .x12";
            return;
        }

        Stream = await file.OpenReadAsync();

        var detectedType = await DetermineInputType(Stream);

        if (detectedType is null) 
        {
            Error = "Provided file is not valid EDIFACT or X12 file";
            return;
        }

        _inputType = detectedType.Value;
        Error = null;
        FileName = file.Name;
    }

    [RelayCommand]
    public async Task Parse()
    {
        Console.WriteLine("Parsing file...");
    }


    private static async Task<InputType?> DetermineInputType(Stream stream) 
    {
        var reader = new StreamReader(stream, leaveOpen: true);

        stream.Position = 0;

        var firstLine = await reader.ReadLineAsync();

        stream.Position = 0;

        if (string.IsNullOrWhiteSpace(firstLine))
            return null;

        if (firstLine.StartsWith("ISA"))
            return InputType.X12;

        if (firstLine.StartsWith("UNA")
            || firstLine.StartsWith("UNB"))
            return InputType.EDIFACT;

        return null;
    }
}

