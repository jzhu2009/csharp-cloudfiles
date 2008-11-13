using System.IO;

namespace com.mosso.cloudfiles.unit.tests.mocks
{
    public class MockStream : MemoryStream
    {
        public byte[] StreamData;

        public bool WasClosed;

        public override void Close()
        {
            if (!WasClosed)
            {
                StreamData = new byte[Length];
                Position = 0;
                Read(StreamData, 0, (int) Length);
                WasClosed = true;
            }

            base.Close();
        }
    }
}