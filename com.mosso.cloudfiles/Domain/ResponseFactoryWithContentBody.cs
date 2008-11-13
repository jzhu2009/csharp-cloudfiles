///
/// See COPYING file for licensing information
///

using System.IO;
using System.Net;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;

namespace com.mosso.cloudfiles.domain
{
    /// <summary>
    /// ResponseFactoryWithContentBody
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseFactoryWithContentBody<T> : IResponseFactory<T> where T : IResponseWithContentBody, new()
    {
        private HttpWebResponse httpResponse;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public T Create(CloudFilesRequest request)
        {
            HttpWebRequest httpWebRequest = request.GetRequest();
            httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();

            WebHeaderCollection headerCollection = httpResponse.Headers;
            HttpStatusCode statusCode = httpResponse.StatusCode;
            Stream responseStream = httpResponse.GetResponseStream();

            T response = new T
                             {
                                 Headers = headerCollection,
                                 Status = statusCode,
                                 ContentStream = responseStream
                             };

            return response;
        }
    }
}