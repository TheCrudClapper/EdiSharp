using System.Text;

namespace EdiSharp.Core.Tokenizers;

public static class DelimitedEscapedSplitter
{
    public static List<string> Split(string raw, char separator, char escape)
    {
        bool isEscaping = false;
        var sb = new StringBuilder();
        List<string> splits = [];

        foreach (char c in raw)
        {
            if (isEscaping)
            {
                sb.Append(c);
                isEscaping = false;
                continue;
            }

            if (c == escape)
            {
                isEscaping = true;
                continue;
            }

            if (c == separator)
            {
                splits.Add(sb.ToString());
                sb.Clear();
                continue;
            }

            sb.Append(c);
        }

        splits.Add(sb.ToString());
        
        return splits;
    }
}
