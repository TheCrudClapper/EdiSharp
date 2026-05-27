using EdiSharp.Core.DTO;
using EdiSharp.Core.Exceptions;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.Domain.ResultTypes;

namespace EdiSharp.Core.Services;

public class EdiProcessingService : IEdiProcessingService
{
    private readonly IEdiTokenizerFactory _tokenizerFactory;
    private readonly IEdiMessageSplitterFactory _splitterFactory;
    public EdiProcessingService(IEdiTokenizerFactory tokenizerFactory, IEdiMessageSplitterFactory splitterFactory)
    {
        _tokenizerFactory = tokenizerFactory;
        _splitterFactory = splitterFactory;
    }

    public async Task<Result> ProcessAsync(EdiParseRequest request)
    {
        var tokenizer = _tokenizerFactory.TryCreate(request.options.InputType)
            ?? throw new EdiInstantiationException($"No tokenizer for {request.options.InputType} type");

        var tokenizedEdi = tokenizer.Tokenize(request.fileBytes, request.options.Encoding, request.options.Delimiters);

        var messageSplitter = _splitterFactory.TryCreate(request.options.InputType)
            ?? throw new EdiInstantiationException($"No message splitter for {request.options.InputType} type");

        var splittedEdi = messageSplitter.Split(tokenizedEdi, request.options.Encoding);

        throw new NotImplementedException();
    }
}
