using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EdiSharp.Core.DTO;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.UI.Enums;
using EdiSharp.UI.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace EdiSharp.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    #region Fields & Constructors
    [ObservableProperty]
    private UIState _state = UIState.Idle;

    [ObservableProperty]
    private bool _validate = false;

    [ObservableProperty]
    private bool _isJsonChecked = false;

    [ObservableProperty]
    private bool _isXmlChecked = false;

    [ObservableProperty]
    private string? _error;

    [ObservableProperty]
    private DocumentContext? _context;

    [ObservableProperty]
    private DocumentViewModel? _document;

    [ObservableProperty]
    private ObservableCollection<StatusMessageViewModel> _statusMessages = new();
    private readonly Func<TopLevel?> _topLevelAccessor;
    private readonly IEdiProcessingService _service;
    private readonly IFileInspectionService _fileInspectionService;
    private readonly IDocumentPreviewerServiceFactory _previewFactory;

    public MainWindowViewModel(
        Func<TopLevel?> topLevelAccessor,
        IEdiProcessingService service,
        IFileInspectionService fileInspectionService,
        IDocumentPreviewerServiceFactory previewFactory)
    {
        _topLevelAccessor = topLevelAccessor;
        _service = service;
        _fileInspectionService = fileInspectionService;
        _previewFactory = previewFactory;
        _document = DocumentViewModel.GetInitalState();
    }

    public bool IsBusy =>
        State is UIState.LoadingFile or UIState.Inspecting or UIState.Parsing;

    public bool IsDiscardButtonVisible =>
        Context is not null;

    public bool IsErrorVisible =>
        !string.IsNullOrWhiteSpace(Error);

    public bool CanParse =>
        State == UIState.ReadyToParse;

    #endregion

    private void SetState(UIState state)
    {
        State = state;
        OnPropertyChanged(nameof(IsBusy));
        OnPropertyChanged(nameof(IsDiscardButtonVisible));
        OnPropertyChanged(nameof(IsErrorVisible));
        OnPropertyChanged(nameof(CanParse));
    }

    [RelayCommand]
    public async Task PickFile()
    {
        var topLevel = _topLevelAccessor();
        if (topLevel is null)
            return;

        SetState(UIState.LoadingFile);

        try
        {
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false
            });

            if (files.Count == 0)
            {
                SetState(UIState.Idle);
                return;
            }

            var file = files[0];

            var ext = Path.GetExtension(file.Name);
            if (ext is not ".edi" and not ".edifact" and not ".x12" and not ".txt")
            {
                RaiseError("Unsupported file extension");
                return;
            }

            var bytes = await File.ReadAllBytesAsync(file.Path.LocalPath);

            SetState(UIState.Inspecting);

            var inspectionResult = _fileInspectionService.Inspect(bytes);

            if (inspectionResult.IsFailure)
            {
                RaiseError(inspectionResult.Error.Description);
                return;
            }

            var inspection = inspectionResult.Value;

            var previewer = _previewFactory.TryCreate(inspection.InputType);
            if (previewer is null)
            {
                RaiseError("Preview service not available");
                return;
            }

            var preview = previewer.GetRawDocumentPreview(bytes, inspection.Encoding, inspection.Delimiters);

            Context = new DocumentContext
            {
                Bytes = bytes,
                Inspection = inspection,
            };

            Document = new DocumentViewModel
            {
                FileName = file.Name,
                RawPreview = preview,
                EdiStandard = inspection.InputType.ToString(),
                EncodingName = inspection.Encoding.EncodingName,
                SegmentCount = inspection.SegmentCount
            };

            Error = null;
            SetState(UIState.ReadyToParse);
            PushMessage("File loaded successfully", false);
            
        }
        catch (Exception ex)
        {
            RaiseError(ex.Message);
        }
    }

    [RelayCommand]
    public async Task Parse()
    {
        if (Context is null)
            return;

        OutputStandard? outputType =
            IsJsonChecked ? OutputStandard.JSON :
            IsXmlChecked ? OutputStandard.XML :
            null;

        if (outputType is null)
            return;

        try
        {
            var request = new EdiParseContext(
            Context.Bytes,
            new ParseOptions()
            {
                Validate = Validate,
                Delimiters = Context.Inspection.Delimiters,
                Encoding = Context.Inspection.Encoding,
                InputType = Context.Inspection.InputType,
                OutputType = outputType.Value
            });


            await _service.ProcessAsync(request);

            SetState(UIState.ReadyToParse);
        }
        catch (Exception ex)
        {
            RaiseError(ex.Message);
        }

    }

    [RelayCommand]
    private void DiscardSelectedFile()
    {
        Context = null;
        Error = null;
        Document = DocumentViewModel.GetInitalState();
        SetState(UIState.Idle);
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

    private void RaiseError(string message)
    {
        Error = message;
        PushMessage(message, true);
        SetState(UIState.Error);
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
        OnPropertyChanged(nameof(IsErrorVisible));
    }
}

