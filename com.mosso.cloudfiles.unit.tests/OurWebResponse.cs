using System.IO;
using System.Net;

namespace com.mosso.cloudfiles.unit.tests
{
    internal class OurWebResponse : Response
    {
        private readonly WebResponse response;

        public OurWebResponse(WebResponse response)
        {
            this.response = response;
        }

        public Stream GetResponseStream()
        {
            return response.GetResponseStream();
        }
    }
}