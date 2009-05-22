///
/// See COPYING file for licensing information
///

using System;
using System.Net;
using System.Text;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.utils;

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
        public ResponseFactory()
        {
            Log.EnsureInitialized();
        }

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
            Log.Debug(this, OutputRequestInformation(httpWebRequest));


            HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
            Log.Debug(this, OutputResponseInformation(response));

            var headerCollection = response.Headers;
            var statusCode = response.StatusCode;

            response.Close();
            return new T
                       {
                           Headers = headerCollection,
                           Status = statusCode
                       };
        }

        private string OutputRequestInformation(HttpWebRequest request)
        {
            StringBuilder output = new StringBuilder();
            output.Append("\n");
            output.Append("REQUEST");
            output.Append("\n");
            output.Append("Request URL: ");
            output.Append(request.RequestUri);
            output.Append("\n");
            output.Append("method: ");
            output.Append(request.Method);
            output.Append("\n");
            output.Append("Headers: ");
            output.Append("\n");
            foreach (var key in request.Headers.AllKeys)
            {
                output.Append(key);
                output.Append(": ");
                output.Append(request.Headers[key]);
                output.Append("\n");
            }

            return output.ToString();
        }

        private string OutputResponseInformation(HttpWebResponse response)
        {
            StringBuilder output = new StringBuilder();
            output.Append("\n");
            output.Append("RESPONSE:");
            output.Append("\n");
            output.Append("method: ");
            output.Append(response.Method);
            output.Append("\n");
            output.Append("Status Code: ");
            output.Append(response.StatusCode);
            output.Append("\n");
            output.Append("Status Description: ");
            output.Append(response.StatusDescription);
            output.Append("\n");
            output.Append("Headers: ");
            output.Append("\n");
            foreach (var key in response.Headers.AllKeys)
            {
                output.Append(key);
                output.Append(": ");
                output.Append(response.Headers[key]);
                output.Append("\n");
            }

            return output.ToString();
        }
    }
}