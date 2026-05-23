using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EdiSharp.Core.DTO;
using EdiSharp.Core.Enums;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace EdiSharp.UI.ViewModels;

public partial class MainWindowViewModel(
    Func<TopLevel?> topLevelAccessor,
    IEdiProcessingService service,
    IFileInspectionService fileInspectionService)
    : ViewModelBase
{

    #region Fields
    private const string DefaultFileText = "Choose EDIFACT File";
    private const string DefaultFileDetailsText = "Unknown";

    [ObservableProperty]
    private bool _validate = false;

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
    private string _ediVersion = DefaultFileDetailsText;

    [ObservableProperty]
    private string _inputTypeText = DefaultFileDetailsText;

    [ObservableProperty]
    private int _segmentCount = 0;

    [ObservableProperty]
    private string _encodingName = DefaultFileDetailsText;

    [ObservableProperty]
    private string? _rawDocument;

    [ObservableProperty]
    private ObservableCollection<StatusMessageViewModel> _statusMessages = new();

    //Selected file bytes
    private byte[]? _fileBytes;
    private FileInspectionResult? FileInspectionResult { get; set; }

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
            PushMessage(Error, true);
            return;
        }

        try
        {
            _fileBytes = await File.ReadAllBytesAsync(file.Path.LocalPath);
        }
        catch
        {
            Error = "Failed to read selected file";
            PushMessage(Error, true);
            return;
        }

        var result = fileInspectionService.Inspect(_fileBytes);

        if (result.IsFailure)
        {
            Error = result.Error.Description;
            PushMessage(Error, true);
            return;
        }

        FileInspectionResult = result.Value;
        RawDocument = result.Value.RawDocument;
        FileName = file.Name;
        SegmentCount = result.Value.SegmentCount;
        EncodingName = result.Value.Encoding.EncodingName;
        EdiVersion = result.Value.Version;
        Error = null;
        InputTypeText = InputTypeToStringConverter.ToStringInputType(result.Value.InputType);
    }

    [RelayCommand]
    public async Task Parse()
    {
        OutputType? outputType =
            IsJsonChecked ? OutputType.JSON :
            IsXmlChecked ? OutputType.XML :
            null;

        if (_fileBytes is null || outputType is null || FileInspectionResult is null)
            return;

        var request = new EdiParseRequest(
            _fileBytes,
            new ParseOptions()
            {
                Validate = Validate,
                Delimiters = FileInspectionResult.Delimiters,
                Encoding = FileInspectionResult.Encoding,
                InputType = FileInspectionResult.InputType,
                OutputType = outputType.GetValueOrDefault()
            });

        await service.ProcessAsync(request);
    }

    [RelayCommand]
    private void DiscardSelectedFile()
    {
        _fileBytes = null;
        FileName = DefaultFileText;
        EncodingName = DefaultFileDetailsText;
        EdiVersion = DefaultFileDetailsText;
        InputTypeText = DefaultFileDetailsText;
        RawDocument = null;
        SegmentCount = 0;
        FileInspectionResult = null;
    }

    private void PushMessage(string message, bool isError) 
    {
        StatusMessages.Insert(0, new StatusMessageViewModel
        {
            IsError = isError,
            Message = message,
            TimeStamp = DateTime.Now.TimeOfDay
        });
    }

    [RelayCommand]
    public void FlushMessages() 
    {
        StatusMessages.Clear();
    }

    [RelayCommand]
    public void DeleteError()
    {
        Error = null;
    }
}

