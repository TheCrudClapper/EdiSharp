using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;

namespace EdiSharp.Core.Factories.Implementations;

public class EdiTokenizerFactory : IEdiTokenizerFactory
{
    private readonly Dictionary<EdiStandard, IEdiTokenizer> _map;
    public EdiTokenizerFactory(IEnumerable<IEdiTokenizer> tokenizers)
    {
        _map = tokenizers.ToDictionary(t => t.InputType);
    }

    public IEdiTokenizer? TryCreate(EdiStandard inputType)
    {
        if (_map.TryGetValue(inputType, out var tokenizer))
            return tokenizer;

        return null;
    }
}
