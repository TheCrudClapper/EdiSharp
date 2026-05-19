using EdiSharp.Core.DTO;
using EdiSharp.Core.Interfaces;

namespace EdiSharp.Core.Implementations;

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
