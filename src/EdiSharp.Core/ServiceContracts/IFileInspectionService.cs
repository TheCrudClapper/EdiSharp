using EdiSharp.Core.DTO;
using EdiSharp.Domain.ResultTypes;

namespace EdiSharp.Core.ServiceContracts;

public interface IFileInspectionService
{
    Result<FileInspectionResult> Inspect(byte[] fileBytes);
}
