using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    public class SetContainerAsPublicResponse : IResponse
    {
        public HttpStatusCode Status
        {
            get; set;
        }

        public WebHeaderCollection Headers
        {
            get; set;
        }
    }
}