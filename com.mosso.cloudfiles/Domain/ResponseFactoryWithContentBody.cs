///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
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
            var httpWebRequest = request.GetRequest();
//            OutputRequestInformation(httpWebRequest);
            
            httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();

            var headerCollection = httpResponse.Headers;
            var statusCode = httpResponse.StatusCode;
            var responseStream = httpResponse.GetResponseStream();

            T response = new T
                             {
                                 Headers = headerCollection,
                                 Status = statusCode,
                                 ContentStream = responseStream
                             };

            return response;
        }

//        private void OutputRequestInformation(HttpWebRequest request)
//        {
//            Console.WriteLine(request.Method +" "+ request.RequestUri);
//            foreach(var key in request.Headers.AllKeys)
//            {
//                Console.WriteLine(key + ": " + request.Headers[key]);
//            }
//        }
    }
}