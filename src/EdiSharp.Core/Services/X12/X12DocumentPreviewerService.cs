using EdiSharp.Core.Enums;
using EdiSharp.Core.Models.Delimiters;
using EdiSharp.Core.ServiceContracts;
using System.Text;

namespace EdiSharp.Core.Services.X12;

public class X12DocumentPreviewerService : IDocumentPreviewerService
{
    public EdiStandard InputType => EdiStandard.X12;

    public string GetRawDocumentPreview(byte[] fileBytes, Encoding encoding, EdiDelimiters delimiters)
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
