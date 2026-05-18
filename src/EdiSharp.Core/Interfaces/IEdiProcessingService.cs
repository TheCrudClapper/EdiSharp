using EdiSharp.Core.DTO;

namespace EdiSharp.Core.Interfaces;
public interface IEdiProcessingService
{
    Task ProcessAsync(EdiParseRequest request);
}
