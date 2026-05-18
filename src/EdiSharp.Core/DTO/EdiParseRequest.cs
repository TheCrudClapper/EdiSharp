namespace EdiSharp.Core.DTO;

public record EdiParseRequest(byte[] fileBytes, ParseOptions options);