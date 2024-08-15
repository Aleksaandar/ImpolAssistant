using IMPOLAssistant.KernelMemory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.KernelMemory.DataFormats.Office;
using Microsoft.KernelMemory.DataFormats.Pdf;

namespace IMPOLAssistant.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KernelMemoryController : ControllerBase
    {
        private readonly IKernelMemoryService _kernelMemoryService;

        public KernelMemoryController(IKernelMemoryService kernelMemoryService)
        {
            _kernelMemoryService = kernelMemoryService;
        }

        [HttpPost("import-document")]
        public async Task<IActionResult> ImportDocument([FromForm] string filePath, [FromForm] string documentId)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return BadRequest("Valid  file path must be provided.");
            }

            if (string.IsNullOrEmpty(documentId))
            {
                return BadRequest("Document ID must be provided.");
            }

            await _kernelMemoryService.ImportDocumentAsync(filePath, documentId);

            return Ok(" document imported successfully.");
        }

        [HttpPost("import-webpage")]
        public async Task<IActionResult> ImportWebPage([FromBody] ImportWebPageRequest request)
        {
            if (string.IsNullOrEmpty(request.Url) || string.IsNullOrEmpty(request.DocId))
            {
                return BadRequest("URL and DocId must be provided.");
            }

            await _kernelMemoryService.ImportWebPageAsync(request.Url, request.DocId);
            return Ok("Web page imported successfully.");
        }
        [HttpPost("importExcel")]
        public async Task<IActionResult> ImportExcel(IFormFile file, [FromQuery] string docId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
#pragma warning disable KMEXP00 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var text = await new MsExcelDecoder().DecodeAsync("msexcelfile.xlsx");
#pragma warning restore KMEXP00 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            Console.WriteLine(text);

            await _kernelMemoryService.ImportExcelAsync(filePath, docId);

            return Ok("Excel file imported successfully.");
        }




        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            if (string.IsNullOrEmpty(request.Question))
            {
                return BadRequest("Question must be provided.");
            }

            var result = await _kernelMemoryService.AskAsync(request.Question);
            return Ok(result);
        }

        public class ImportWebPageRequest
        {
            public string Url { get; set; }
            public string DocId { get; set; }
        }

        public class AskRequest
        {
            public string Question { get; set; }
        }

    }
}
