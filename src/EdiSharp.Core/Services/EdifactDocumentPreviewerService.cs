using EdiSharp.Core.Enums;
using EdiSharp.Core.Models;
using EdiSharp.Core.ServiceContracts;
using System.Text;

namespace EdiSharp.Core.Services;

public class EdifactDocumentPreviewerService : IDocumentPreviewerService
{
    public InputType InputType => InputType.EDIFACT;

    public string GetRawDocumentPreview(byte[] fileBytes, Encoding encoding, EdifactDelimiters delimiters)
    {
        var sb = new StringBuilder();
        var text = encoding.GetString(fileBytes);

        int lineNumer = 1;
        var lines = text.Split(delimiters.SegmentTerminator);

        for (int i = 0; i < lines.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                sb.Append($"{lineNumer:000}: ")
                  .Append(lines[i].Trim())
                  .Append(delimiters.SegmentTerminator)
                  .AppendLine();

                lineNumer++;
            }
        }

        return sb.ToString();
    }
}
