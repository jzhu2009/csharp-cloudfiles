using System.Collections.Generic;
using System.IO;
using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    public class GetSerializedResponse : IResponseWithContentBody
    {
        private readonly List<string> contentBody;
        private Stream contentStream;

        /// <summary>
        /// The default constructor for creating this response
        /// </summary>
        public GetSerializedResponse()
        {
            contentBody = new List<string>();
        }

        /// <summary>
        /// A property representing the status of the request from cloudfiles
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from each request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// This method must be called once the stream has been processed to release the resources associated with the request
        /// </summary>
        public void Dispose()
        {
            if (contentStream != null)
                contentStream.Close();
        }

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
        /// A property containing the list of names of objects in the requested container
        /// </summary>
        public List<string> ContentBody
        {
            get { return contentBody; }
        }

        /// <summary>
        /// A property representing the MIME type of the content in the body of the response
        /// </summary>
        public string ContentType
        {
            get { return Headers[Constants.CONTENT_TYPE_HEADER]; }
        }

        /// <summary>
        /// A property representing the length of the content in the body of the response
        /// </summary>
        public string ContentLength
        {
            get { return Headers[Constants.CONTENT_LENGTH_HEADER]; }
        }

        private void ReadStream()
        {
            string[] streamLines = new StreamReader(contentStream).ReadToEnd().Split('\n');
            if (Status == HttpStatusCode.OK)
            {
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