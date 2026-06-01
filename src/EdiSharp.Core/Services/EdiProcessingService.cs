using EdiSharp.Core.DTO;
using EdiSharp.Core.Exceptions;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.ServiceContracts;
using EdiSharp.Domain.ResultTypes;

namespace EdiSharp.Core.Services;

public class EdiProcessingService : IEdiProcessingService
{
    private readonly IEdiTokenizerFactory _tokenizerFactory;
    private readonly IEdiInterchangeBuilderFactory _interchangeBuilderFactory;
    public EdiProcessingService(IEdiTokenizerFactory tokenizerFactory, IEdiInterchangeBuilderFactory splitterFactory)
    {
        _tokenizerFactory = tokenizerFactory;
        _interchangeBuilderFactory = splitterFactory;
    }

    public async Task<Result> ProcessAsync(EdiParseContext context)
    {
        var tokenizer = _tokenizerFactory.TryCreate(context.options.InputType)
            ?? throw new EdiInstantiationException($"No tokenizer for {context.options.InputType} type");

        var tokenizedEdi = tokenizer.Tokenize(context.fileBytes, context.options.Encoding, context.options.Delimiters);

        var interchangeBuilder = _interchangeBuilderFactory.TryCreate(context.options.InputType)
            ?? throw new EdiInstantiationException($"No intechange builder for {context.options.InputType} type");

        var splittedEdi = interchangeBuilder.Build(tokenizedEdi, context.options.Encoding);

        throw new NotImplementedException();
    }
}
