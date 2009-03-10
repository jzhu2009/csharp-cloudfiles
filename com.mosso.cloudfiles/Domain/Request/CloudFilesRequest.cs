///
/// See COPYING file for licensing information
///

using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// Wraps requests to optionally handle proxy credentials and ssl
    /// </summary>
    public class CloudFilesRequest
    {
        private readonly IRequest request;
        private readonly ProxyCredentials proxyCredentials;

        /// <summary>
        /// Constructor without proxy credentials provided
        /// </summary>
        /// <param name="request">The request being sent to the server</param>
        public CloudFilesRequest(IRequest request) : this(request, null)
        {
        }

        /// <summary>
        /// Constructor with proxy credentials provided
        /// </summary>
        /// <param name="request">The request being sent to the server</param>
        /// <param name="proxyCredentials">Proxy credentials</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        public CloudFilesRequest(IRequest request, ProxyCredentials proxyCredentials)
        {
            if (request == null) throw new ArgumentNullException();

            this.request = request;
            this.proxyCredentials = proxyCredentials;
        }

        /// <summary>
        /// RequestType
        /// </summary>
        /// <returns>the type of the request</returns>
        public Type RequestType
        {
            get { return request.GetType(); }
        }

        /// <summary>
        /// GetRequest
        /// </summary>
        /// <returns>a HttpWebRequest object that has all the information to make a request against CloudFiles</returns>
        public HttpWebRequest GetRequest()
        {
            if (request.Uri.Scheme.ToLower().Equals("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = OnCertificateValidation;
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(request.Uri);
            if (request.Headers != null) httpWebRequest.Headers.Add(request.Headers);

            httpWebRequest.Method = request.Method;
            httpWebRequest.Timeout = Constants.CONNECTION_TIMEOUT;
            httpWebRequest.UserAgent = Constants.USER_AGENT;

            HandleIsModifiedSinceHeaderRequestFieldFor(httpWebRequest);
            HandleRangeHeader(httpWebRequest);
            HandleRequestBodyFor(httpWebRequest);
            HandleProxyCredentialsFor(httpWebRequest);

            return httpWebRequest;
        }

        private void HandleRangeHeader(HttpWebRequest webrequest)
        {
            if (!(request is IRangedRequest)) return;
            IRangedRequest rangedRequest = (IRangedRequest) request;
            if (rangedRequest.RangeFrom != 0 && rangedRequest.RangeTo == 0)
                webrequest.AddRange("bytes", rangedRequest.RangeFrom);
            else if (rangedRequest.RangeFrom == 0 && rangedRequest.RangeTo != 0)
                webrequest.AddRange("bytes", rangedRequest.RangeTo);
            else if (rangedRequest.RangeFrom != 0 && rangedRequest.RangeTo != 0)
                webrequest.AddRange("bytes", rangedRequest.RangeFrom, rangedRequest.RangeTo);
        }

        private void HandleIsModifiedSinceHeaderRequestFieldFor(HttpWebRequest webrequest)
        {
            if (!(request is IModifiedSinceRequest)) return;
            webrequest.IfModifiedSince = ((IModifiedSinceRequest)request).ModifiedSince;
        }

        private void HandleProxyCredentialsFor(HttpWebRequest httpWebRequest)
        {
            if (proxyCredentials == null) return;
            
            WebProxy loProxy = new WebProxy(proxyCredentials.ProxyAddress, true);

            if (proxyCredentials.ProxyUsername.Length > 0)
                loProxy.Credentials = new NetworkCredential(proxyCredentials.ProxyUsername, proxyCredentials.ProxyPassword, proxyCredentials.ProxyDomain);
            httpWebRequest.Proxy = loProxy;
        }

        private void HandleRequestBodyFor(HttpWebRequest httpWebRequest)
        {
            if (!(request is IRequestWithContentBody)) return;

            IRequestWithContentBody requestWithContentBody = (IRequestWithContentBody) request;
            httpWebRequest.ContentLength = requestWithContentBody.ContentLength;
            httpWebRequest.AllowWriteStreamBuffering = false;

            var requestMimeType = request.ContentType;
            httpWebRequest.ContentType = String.IsNullOrEmpty(requestMimeType) 
                ? "application/octet-stream" : requestMimeType;

            var stream = httpWebRequest.GetRequestStream();
            requestWithContentBody.ReadFileIntoRequest(stream);
        }

        /// <summary>
        /// OnCertificateValidation
        /// </summary>
        /// <param name="sender">the caller of this callback</param>
        /// <param name="certificate">the X509 certificate instance</param>
        /// <param name="chain">the security chain</param>
        /// <param name="sslPolicyErrors">and errors that occurred duing the ssl process</param>
        /// <returns>true if no ssl policy errors, false if ssl policy errors</returns>
        private static bool OnCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return (sslPolicyErrors == SslPolicyErrors.None);
        }
    }
}