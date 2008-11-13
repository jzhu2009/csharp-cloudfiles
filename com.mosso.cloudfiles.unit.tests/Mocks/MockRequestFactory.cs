namespace com.mosso.cloudfiles.unit.tests.mocks
{
    public class MockRequestFactory : RequestFactory
    {
        public string RequestedUri;

        public MockRequest Request = new MockRequest();

        private byte[] data;

        public bool HasResponseDataSet
        {
            get { return Request.Response.Stream.Length > 0; }
        }

        public void SetResponseData(byte[] responseData)
        {
            data = responseData;
            ConfigureResponseStream(responseData);
        }

        private void ConfigureResponseStream(byte[] responseData)
        {
            Request.Response.Stream.SetLength(0);
            Request.Response.Stream.Write(responseData, 0, responseData.Length);
            Request.Response.Stream.Position = 0;
        }

        public Request Create(string uri)
        {
            RequestedUri += uri;
            return Request;
        }

        public void Reset()
        {
            Request = new MockRequest();
            ConfigureResponseStream(data);
        }
    }
}