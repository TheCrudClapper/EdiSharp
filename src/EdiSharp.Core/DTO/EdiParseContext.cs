namespace EdiSharp.Core.DTO;

public record EdiParseContext(byte[] fileBytes, ParseOptions options);