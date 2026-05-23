using EdiSharp.Core.DTO;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.Domain.ResultTypes;

namespace EdiSharp.Core.Services;

public class EdiProcessingService : IEdiProcessingService
{
    private readonly IEdiTokenizerFactory _tokenizerFactory;
    public EdiProcessingService(IEdiTokenizerFactory tokenizerFactory)
    {
        _tokenizerFactory = tokenizerFactory;
    }

    public async Task<Result> ProcessAsync(EdiParseRequest request)
    {
        var tokenizer = _tokenizerFactory.TryCreate(request.options.InputType);
        if(tokenizer is null)
            return Result.Failure(Error.Create("Tokenizer failed to be instantiated"));

        var tokenizedEdi = tokenizer.Tokenize(request.fileBytes, request.options.Encoding, request.options.Delimiters);

        throw new NotImplementedException();
    }
}
