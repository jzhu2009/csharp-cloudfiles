///
/// See COPYING file for licensing information
///

using System.IO;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// IRequestWithContentBody
    /// </summary>
    public interface IRequestWithContentBody : IRequest
    {
        long ContentLength { get; }
        void ReadFileIntoRequest(Stream httpWebRequestFileStream);
        string FileUri { get; }
    }
}