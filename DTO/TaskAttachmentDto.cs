
namespace PmsApi.DTO;

public record TaskAttachmentDto(

    int Id,

    string FileName,

    byte[]? FileData,

    int? TaskId


);
