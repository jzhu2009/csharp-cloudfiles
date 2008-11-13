using System.Collections.Generic;
using System.Text;

namespace com.mosso.cloudfs.docs
{
    public class ExampleGenerator
    {
        private List<Example> examples;

        public ExampleGenerator()
        {
            examples = new List<Example>();
            AddStaticExamples();
        }

        private void AddStaticExamples()
        {
            examples.Add(new Example
                             {
                                 Content = "alektorphobic, the .NET Client API, makes it easy to work with CloudFS using the Microsoft .NET Framework."
                             });

            examples.Add(new Example
                             {
                                 Content = "#1: The first step is to include the com.mosso.cloudfs dll as a reference in your project and then add a using statement to your file."
                             });

            examples.Add(new Example
                             {
                                 Content = "#2: With the library referenced you can now establish a connection to CloudFS using the Connection class.  You must pass this class an instance of the UserCredentials class to handle connectivity information for your account; if an AuthenticationFailedException is thrown, verify that the account information was entered accurately."
                             });
            examples.Add(new Example
                             {
                                 Content = "#3: Once you have established a connection to CloudFS, you can now interact with the server using the API.  You can use the GetContainers method to display the containers within the current account.  Since this account has no containers, we were returned an empty list.  We can now create a new container using the CreateContainer method."
                             });
            examples.Add(new Example
                             {
                                 Content = "#4: You can get a count of the number of containers you have under your account and also the total bytes used by using the GetAccountInformation call off of the connection instance."
                             });
            examples.Add(new Example
                             {
                                 Content = "#5: Once a container has been created, you can upload files into a container by utilizing the Connection class. Specifically, calling the PutStorageItem method will allow you to pass in the path to a file, or optionally, a stream, and subsequently upload that file to CloudFS."
                             });
            examples.Add(new Example
                             {
                                 Content = "#6: Once you have a container with storage items in it, you can choose to delete one of those items by calling Connection.DeleteStorageItem on your connection instance."
                             });
            examples.Add(new Example
                             {
                                 Content = "#7: To facilitate searching and identification, meta information can be applied to storage items that exist in containers on CloudFS. Simply call Connection.SetStorageItemMetaInformation."
                             });
            examples.Add(new Example
                             {
                                 Content = "#8: Once you have applied meta data to a storage item, it can be retrieved by calling Connection.GetStorageItemMetaInformation."
                             });
            examples.Add(new Example
                             {
                                 Content = "#9: Additionally, you can retrieve items from an existing, non-empty container by calling Connection.GetStorageItem."
                             });
            examples.Add(new Example
                             {
                                 Content = new ProgramListing(new[]
                                                                       {
                                                                           "1: using com.mosso.cloudfs",
                                                                           "2: Connection connection = new Connection(new UserCredentials(\"username\",\"password\"));",
                                                                           "3: List<string> containers = connection.GetContainers();",
                                                                           "3: connection.CreateContainer(\"docs\");",
                                                                           "3: connection.CreateContainer(\"docs\");",
                                                                           "5: AccountInformation accountInformation = connection.GetAccountInformation();",
                                                                           "5: int containerCount = accountInformation.ContainerCount",
                                                                           "5: long bytesUsed = accountInformation.BytesUsed",
                                                                           "6: connection.DeleteStorageItem(\"docs\", \"my_doc\");",
                                                                           "7: Dictionary<string, string> metaInfo = new Dictionary<string, string>( {\"fruit\", \"apple\"}, {\"color\", \"red\")",
                                                                           "7: connection.SetStorageItemMetaInformation(\"my_docs\", \"doc\", metaInformation);",
                                                                           "8: StorageItem storageItem = connection.GetStorageItem(\"my_docs\", \"doc\");",
                                                                           "9: connection.GetStorageItem(\"docs\", \"my_doc\");"
                                                                       }).ToString()
                             });
        }

        public string GenerateExampeles()
        {
            StringBuilder output = new StringBuilder();
            output.Append("<sect3><title>Examples</title>");
            foreach (Example example in examples)
            {
                output.Append(example.ToString());
            }
            output.Append("</sect3>");
            return output.ToString();
        }
    }
}