using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/processing")]
public class ProcessingController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IQueueService _queueService;
    private readonly IEncodedMsgRetrivalService _encodedMsgRetrivalSerivce;

    public ProcessingController(IBlobStorageService blobStorageService, IQueueService queueService, IEncodedMsgRetrivalService encodedMsgRetrivalService)
    {
        _blobStorageService = blobStorageService;
        _queueService = queueService;
        _encodedMsgRetrivalSerivce = encodedMsgRetrivalService;
    }

    [HttpPost("encode")]
    public async Task<IActionResult> Encode([FromBody] SaveFileRequest request)
    {
        await _blobStorageService.SaveContentToBlobAsync(request.FileName, request.FileContent);

        await _queueService.AddContentToQueue(request.FileName);

        return Ok("File saved and queued");
    }

    [HttpGet("download/{nazwa}")]
    public async Task<IActionResult> GetEncodedMsg(string nazwa)
    {
        var content = await _encodedMsgRetrivalSerivce.GetEncodedMessageFromQueueAsync(nazwa);

        return Ok(content);
    }
}