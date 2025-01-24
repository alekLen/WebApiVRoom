using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Ocsp;
using System;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubtitleController : Controller
    {
        private ISubtitleService _subService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly string _containerName;

        public SubtitleController(ISubtitleService subService,BlobServiceClient blobclient,
             IBlobStorageService blobStorageService, IConfiguration configuration)
        {
            _subService = subService;
            _blobServiceClient = blobclient;
            _containerName = configuration.GetConnectionString("ContainerName");
            _blobStorageService = blobStorageService;   
        }
        [HttpGet("getsubtitles/{videoid}")]
        public async Task<ActionResult<List<SubtitleDTO>>> GetSubtitlesByVideo(int videoid)
        {
            var subs = await _subService.GetSubtitlesByVideo(videoid);

            return new ObjectResult(subs);
        }

        [HttpGet("getsubtitlefile/{puthtofile}")]
        public async Task<IActionResult> GetSubtitleFile(string puthtofile)
        {
            try
            {
                var decodedUrl = Uri.UnescapeDataString(puthtofile);
                string fileName = Path.GetFileName(decodedUrl);
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                if (!await blobClient.ExistsAsync())
                {
                    return NotFound($"Файл не найден: {fileName}");
                }
                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0; 

                return File(stream, "text/vtt", Path.GetFileName(fileName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при загрузке файла: {ex.Message}");
            }
        }



        [HttpGet("getpublishsubtitles/{videoid}")]
        public async Task<ActionResult<List<SubtitleDTO>>> GetPublishedSubtitlesByVideo(int videoid)
        {
            var subs = await _subService.GetPublishedSubtitlesByVideo(videoid);
           
            return new ObjectResult(subs);
        }

        [HttpGet("getnotpublishsubtitles/{videoid}")]
        public async Task<ActionResult<List<SubtitleDTO>>> GetNotPublishedSubtitlesByVideo(int videoid)
        {
            var subs = await _subService.GetNotPublishedSubtitlesByVideo(videoid);

            return new ObjectResult(subs);
        }
        [HttpDelete("delete/{subtitleid}")]
        public async Task<ActionResult<List<SubtitleDTO>>> DeleteSubtitle(int subtitleid)
        {
             await _subService.DeleteSubtitle(subtitleid);

            return Ok();
        }


        [HttpPost("add")]
        public async Task<ActionResult> AddSubtitle(IFormFile fileVTT, [FromForm] SubtitleDTO sub)
        {
            
            await _subService.AddSubtitle(sub, fileVTT);

            return Ok();
        }

        [HttpPut("update")]
        public async Task<ActionResult<SubtitleDTO>> UpdateSubtitle(IFormFile fileVTT, string code, string name,
            string videoId, string publish,string id)
        {
            
            SubtitleDTO s = await _subService.GetSubtitle(int.Parse(id));
            if (s == null)
            {
                return NotFound();
            }
            bool p = false;
            if (publish == "yes")
                p = true;
            var subtitleDTO = new SubtitleDTO
            {
                Id= int.Parse(id),
                LanguageName = name,
                LanguageCode = code,
                VideoId = int.Parse(videoId),
                IsPublished = p
            };

            SubtitleDTO sub_new = await _subService.UpdateSubtitle(subtitleDTO, fileVTT);

            return Ok(sub_new);
        }
    }
}
