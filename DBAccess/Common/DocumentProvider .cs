using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DBAccess.Common
{
    public class DocumentProvider 
    {
        public Task<byte[]> GetDocumentAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return File.ReadAllBytesAsync(Path.Combine(AppContext.BaseDirectory, Path.Combine("Data", "Documents", name)), cancellationToken);
        }
    }
}
