namespace EdiSharp.Core.DTO;

public record EdiParseRequest(FileStream fileStream, ParseOptions options);