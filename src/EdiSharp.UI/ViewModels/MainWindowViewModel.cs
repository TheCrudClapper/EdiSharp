using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EdiSharp.Core.DTO;
using EdiSharp.Core.Enums;
using EdiSharp.Core.ServiceContracts;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EdiSharp.UI.ViewModels;

public partial class MainWindowViewModel(Func<TopLevel?> topLevelAccessor, IEdiProcessingService service)
    : ViewModelBase
{

    #region Fields
    private const string DefaultFileText = "Choose EDIFACT File";

    [ObservableProperty]
    private bool _validate = false;

    [ObservableProperty]
    private bool _showRawSegments = false;

    [ObservableProperty]
    private bool _isJsonChecked = false;

    public bool IsDiscardButtonVisible 
        => RawDocument is not null;
    public bool IsErrorVisible 
        => Error is not null;

    [ObservableProperty]
    private bool _isXmlChecked = false;

    [ObservableProperty]
    private string? _error;

    [ObservableProperty]
    private string _fileName = DefaultFileText;

    [ObservableProperty]
    private string? _rawDocument;

    //Selected file bytes
    private byte[]? _fileBytes;

    private InputType? _inputType;
    #endregion

    partial void OnErrorChanged(string? value)
       => OnPropertyChanged(nameof(IsErrorVisible));

    partial void OnRawDocumentChanged(string? value)
        => OnPropertyChanged(nameof(IsDiscardButtonVisible));

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

        try
        {
            _fileBytes = await File.ReadAllBytesAsync(file.Path.LocalPath);
        }
        catch
        {
            Error = "Failed to read selected file";
            return;
        }

        var detectedType = DetermineInputType(_fileBytes);

        if (detectedType is null) 
        {
            Error = "Provided file is not valid EDIFACT or X12 file";
            return;
        }

        _inputType = detectedType.Value;
        Error = null;
        FileName = file.Name;

        BuildRawFilePreview(_fileBytes);
    }

    [RelayCommand]
    public async Task Parse()
    {
        OutputType? outputType =
            IsJsonChecked ? OutputType.JSON :
            IsXmlChecked ? OutputType.XML :
            null;

        if (_fileBytes is null || _inputType is null || outputType is null)
            return;

        var request = new EdiParseRequest(
            _fileBytes,
            new ParseOptions()
            {
                Validate = Validate,
                InputType = _inputType.GetValueOrDefault(),
                ShowRawSegments = ShowRawSegments,
                OutputType = outputType.GetValueOrDefault()
            });

        await service.ProcessAsync(request);
    }

    private void BuildRawFilePreview(byte[] fileBytes) 
    {
        var sb = new StringBuilder();
        var text = Encoding.UTF8.GetString(fileBytes);

        var lines = text.Split('\n');
        for(int i = 0; i < lines.Length; i++) 
        {
            sb.AppendLine($"{i + 1:000}: {lines[i].TrimEnd('\r')}");
        }

        RawDocument = sb.ToString();
    }

    [RelayCommand]
    private void DiscardSelectedFile() 
    {
        _fileBytes = null;
        FileName = DefaultFileText;
        _inputType = null;
        RawDocument = null;
    }

    private static InputType? DetermineInputType(byte[] fileBytes) 
    {
        var text = Encoding.UTF8.GetString(fileBytes);

        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
            return null;
        
        var firstLine = lines[0].Trim();

        if (firstLine.StartsWith("ISA", StringComparison.Ordinal))
            return InputType.X12;

        if (firstLine.StartsWith("UNA", StringComparison.Ordinal) || firstLine.StartsWith("UNB", StringComparison.Ordinal))
            return InputType.EDIFACT;

        return null;
    }
}

