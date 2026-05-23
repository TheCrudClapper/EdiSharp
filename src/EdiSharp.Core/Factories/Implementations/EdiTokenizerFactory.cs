using EdiSharp.Core.Abstractions;
using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;

namespace EdiSharp.Core.Factories.Implementations;

public class EdiTokenizerFactory : IEdiTokenizerFactory
{
    private readonly Dictionary<InputType, IEdiTokenizer> _map;
    public EdiTokenizerFactory(IEnumerable<IEdiTokenizer> tokenizers)
    {
        _map = tokenizers.ToDictionary(t => t.InputType);
    }

    public IEdiTokenizer? TryCreate(InputType inputType)
    {
        if (_map.TryGetValue(inputType, out var tokenizer))
            return tokenizer;

        return null;
    }
}
