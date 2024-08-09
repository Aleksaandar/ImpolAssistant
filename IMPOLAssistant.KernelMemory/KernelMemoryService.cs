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

        public async Task ImportDocumentAsync(string filePath, string documentId)
        {
            await _kernelMemory.ImportDocumentAsync(filePath, documentId);
        }

        public async Task<string> AskAsync(string question)
        {
            var result = await _kernelMemory.AskAsync(question);
            return result.Result;
        }
    }
}
