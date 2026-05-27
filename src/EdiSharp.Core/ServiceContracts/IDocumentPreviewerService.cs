using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using System.Text;

namespace EdiSharp.Core.ServiceContracts;
public interface IDocumentPreviewerService
{
    InputType InputType { get; }
    string GetRawDocumentPreview(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters);
}
