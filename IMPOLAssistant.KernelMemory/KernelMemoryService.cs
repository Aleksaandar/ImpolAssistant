using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
