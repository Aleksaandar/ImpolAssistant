using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPOLAssistant.KernelMemory
{
    public interface IKernelMemoryService
    {
        Task ImportDocumentAsync(string filePath, string documentId);
        Task<string> AskAsync(string question);
        Task ImportWebPageAsync(string url, string docId);
        Task ImportExcelAsync(string filePath, string docId);


    }
}
