namespace PmsApi.DTO;
using Task = PmsApi.Models.Task;
public record AttachmentWithTaskDto(

    int Id,

        string FileName,

        byte[] FileData,

        int TaskId//,
                  // Task Task

    );


