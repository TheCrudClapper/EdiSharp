using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using System.Text;

namespace EdiSharp.Core.ServiceContracts;
public interface IDocumentPreviewerService
{
    EdiStandard InputType { get; }
    string GetRawDocumentPreview(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters);
}
