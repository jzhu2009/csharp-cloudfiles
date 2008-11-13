///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.IO;
using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// A class wrapping the response from the get containers request
    /// </summary>
    public class GetContainersResponse : IResponseWithContentBody
    {
        private Stream contentStream;
        private int numEntities;
        private readonly List<string> contentBody;

        /// <summary>
        /// The default constructor for creating an instance of GetContainersResponse
        /// </summary>
        public GetContainersResponse()
        {
            contentBody = new List<string>();
        }

        /// <summary>
        /// A property representing a list of containers parsed from the body of the response
        /// </summary>
        public List<string> ContentBody
        {
            get { return contentBody; }
        }

        /// <summary>
        /// A property representing the number of containers in the response body
        /// </summary>
        public int NumEntities
        {
            get { return numEntities; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType
        {
            get { return Headers[Constants.CONTENT_TYPE_HEADER]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentLength
        {
            get { return Headers[Constants.CONTENT_LENGTH_HEADER]; }
        }

        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the get containers request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// A property representing the stream returned from the response
        /// </summary>
        public Stream ContentStream
        {
            get { return contentStream; }
            set
            {
                contentStream = value;
                ReadStream();
            }
        }

        /// <summary>
        /// This method must be called once the stream has been processed to release the resources associated with the request
        /// </summary>
        public void Dispose()
        {
            if (contentStream != null)
                contentStream.Close();
        }

        private void ReadStream()
        {
            string[] streamLines = new StreamReader(contentStream).ReadToEnd().Split('\n');
            numEntities = 0;
            if (Status == HttpStatusCode.OK)
            {
                numEntities = streamLines.Length;

                //Because all HTTP requests end with \n\n the split at the end was appending an additional empty string container to the list
                //which of course doesn't exist
                foreach (string s in streamLines)
                {
                    if (s.Length > 0)
                        contentBody.Add(s);
                }
                contentStream.Close();
            }
        }
    }
}