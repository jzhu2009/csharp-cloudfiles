using System.IO;

namespace com.mosso.cloudfiles.unit.tests
{
    public interface RequestFactory
    {
        Request Create(string uri);
    }

    public interface Request
    {
        Stream GetRequestStream();
        Response GetResponse();
    }

    public interface Response
    {
        Stream GetResponseStream();
    }
}