///
/// See COPYING file for licensing information
///

using System;
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

            var httpWebRequest = request.GetRequest();
//            OutputRequestInformation(httpWebRequest);


            HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();

            var headerCollection = response.Headers;
            var statusCode = response.StatusCode;

            response.Close();
            return new T
                       {
                           Headers = headerCollection,
                           Status = statusCode
                       };
        }

//        private void OutputRequestInformation(HttpWebRequest request)
//        {
//            Console.WriteLine(request.Method + " " + request.RequestUri);
//            foreach (var key in request.Headers.AllKeys)
//            {
//                Console.WriteLine(key + ": " + request.Headers[key]);
//            }
//        }
    }
}