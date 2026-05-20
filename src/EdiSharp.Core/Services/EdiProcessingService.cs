using EdiSharp.Core.Abstractions;
using EdiSharp.Core.DTO;
using EdiSharp.Core.ServiceContracts;

namespace EdiSharp.Core.Services;

public class EdiProcessingService : IEdiProcessingService
{
    private readonly IEdiTokenizerFactory _tokenizerFactory;
    public EdiProcessingService(IEdiTokenizerFactory tokenizerFactory)
    {
        _tokenizerFactory = tokenizerFactory;
    }

    public async Task ProcessAsync(EdiParseRequest request)
    {
        var tokenizer = _tokenizerFactory.Create(request.options.InputType);

        var tokenizedEdi = tokenizer.Tokenize(request.fileBytes);
    }
}
