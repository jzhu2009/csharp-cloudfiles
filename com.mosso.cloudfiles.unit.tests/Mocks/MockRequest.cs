using System.IO;

namespace com.mosso.cloudfiles.unit.tests.mocks
{
    public class MockRequest : Request
    {
        public MockStream Stream = new MockStream();

        public MockResponse Response = new MockResponse();

        public int requestContentLength;

        public int ContentLength
        {
            set { requestContentLength = value; }
        }

        public Stream GetRequestStream()
        {
            return Stream;
        }

        public Response GetResponse()
        {
            return Response;
        }
    }
}