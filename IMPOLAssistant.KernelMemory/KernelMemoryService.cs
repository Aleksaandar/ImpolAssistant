using Microsoft.KernelMemory;
using OfficeOpenXml;


namespace IMPOLAssistant.KernelMemory
{
    public class KernelMemoryService : IKernelMemoryService
    {
        private readonly IKernelMemory _kernelMemory;

        public KernelMemoryService(IKernelMemory kernelMemory)
        {
            _kernelMemory = kernelMemory;
        }

        public Task ImportDocumentAsync(string filePath, string documentId)
        {
            return _kernelMemory.ImportDocumentAsync(filePath, documentId);
        }

        public Task ImportWebPageAsync(string url, string docId)
        {
            return _kernelMemory.ImportWebPageAsync(url, docId);
        }

        public async Task<string> AskAsync(string question)
        {
            var result = await _kernelMemory.AskAsync(question);
            return result.Result;
        }
        public async Task ImportExcelAsync(string filePath, string docId)
        {
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0]; 

            for (int row = 1; row <= worksheet.Dimension.Rows; row++)
            {
                var content = "";
                for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                {
                    content += worksheet.Cells[row, col].Text + " ";
                }

                
                await _kernelMemory.ImportDocumentAsync(content, $"{docId}_row{row}");
            }
        }
    }
}
