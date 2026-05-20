using EdiSharp.Core.DTO;

namespace EdiSharp.Core.ServiceContracts;
public interface IEdiProcessingService
{
    Task ProcessAsync(EdiParseRequest request);
}
