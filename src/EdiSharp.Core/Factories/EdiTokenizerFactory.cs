using EdiSharp.Core.Enums;
using EdiSharp.Core.Interfaces;

namespace EdiSharp.Core.Factories;

public class EdiTokenizerFactory : IEdiTokenizerFactory
{
    private readonly Dictionary<InputType, IEdiTokenizer> _map;
    public EdiTokenizerFactory(IEnumerable<IEdiTokenizer> tokenizers)
    {
        _map = tokenizers.ToDictionary(t => t.InputType);
    }

    public IEdiTokenizer Create(InputType inputType)
    {
        return _map[inputType];
    }
}
