///
/// See COPYING file for licensing information
///

using System.Net;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponseFactory<T>
    {
        T Create(CloudFilesRequest request);
    }

    /// <summary>
    /// ResponseFactory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseFactory<T> : IResponseFactory<T> where T : IResponse, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public T Create(CloudFilesRequest request)
        {
            if (request is IRequestWithContentBody)
                throw new InvalidResponseTypeException(
                    "The request type is of IRequestWithContentBody. Content body is expected with this request. ");

            HttpWebRequest httpWebRequest = request.GetRequest();

            HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();

            WebHeaderCollection headerCollection = response.Headers;
            HttpStatusCode statusCode = response.StatusCode;

            response.Close();
            return new T
                       {
                           Headers = headerCollection,
                           Status = statusCode
                       };
        }
    }
}