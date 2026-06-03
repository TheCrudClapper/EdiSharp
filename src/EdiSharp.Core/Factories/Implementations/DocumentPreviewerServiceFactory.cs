using EdiSharp.Core.Enums;
using EdiSharp.Core.Factories.Abstractions;
using EdiSharp.Core.ServiceContracts;

namespace EdiSharp.Core.Factories.Implementations;

public class DocumentPreviewerServiceFactory : IDocumentPreviewerServiceFactory
{
    private readonly Dictionary<EdiStandard, IDocumentPreviewerService> _map;
    public DocumentPreviewerServiceFactory(IEnumerable<IDocumentPreviewerService> previewers)
    {
        _map = previewers.ToDictionary(p => p.InputType);
    }
    public IDocumentPreviewerService? TryCreate(EdiStandard input)
        => _map.TryGetValue(input, out var service) ? service : null;
}
