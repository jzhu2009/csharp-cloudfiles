using System.IO;

namespace com.mosso.cloudfiles.unit.tests.mocks
{
    public class MockResponse : Response
    {
        public MockStream Stream = new MockStream();

        public Stream GetResponseStream()
        {
            return Stream;
        }
    }
}