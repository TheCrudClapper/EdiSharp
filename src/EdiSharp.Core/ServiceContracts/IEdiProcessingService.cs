using EdiSharp.Core.DTO;
using EdiSharp.Domain.ResultTypes;

namespace EdiSharp.Core.ServiceContracts;

public interface IEdiProcessingService
{
    Task<Result> ProcessAsync(EdiParseRequest request);
}
