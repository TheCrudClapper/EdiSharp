using EdiSharp.Core.DTO;
using EdiSharp.Core.Interfaces;

namespace EdiSharp.Core.Implementations;

public class EdiProcessingService : IEdiProcessingService
{
    public async Task ProcessAsync(EdiParseRequest request)
    {
        await Task.Delay(10000);
    }
}
