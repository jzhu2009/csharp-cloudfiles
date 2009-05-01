
The .NET API for Mosso CloudFiles (http://www.mosso.com/cloudfiles.jsp)

#############################
# Compiling
#############################

To compile this file go to this directory in the command prompt and type:

build.bat

#############################
# Using
#############################

Just reference the com.mosso.cloudfiles.dll file (either from downloaded bin zip or from you compilinging the source)
in your project and you can start using Mosso CloudFiles

#############################
# Logging
#############################

Logging is done with the log4net.dll and log4net.config file that are included in the source/downloads.
You just need to edit the log4net.config file so that you get the desired logging output and format.  
Currently the log is going to log/com.mosso.cloudfiles.log, whereever you have the dll referenced.

i.e., 
<file value="logs/com.mosso.cloudfiles.log" />   (in the log4net.config file)

Please reference the log4net documentation on how to edit that config file (http://logging.apache.org/log4net/release/config-examples.html)

#############################
# Forums
#############################

Please visit the Mosso Forums (https://manage.mosso.com/forum/).  Once you are logged in, scroll to cloud files category/grouping and then the .NET thread.

#############################
# Code Examples
#############################

// types are explicitly used in this example.  var keyword could also be used

// Connect to CloudFiles
UserCredentials userCredentials = new UserCredentials("username", "api key");
IConnection connection = new Connection(userCredentials);

// Get the account information
AccountInformation accountInformation = connection.GetAccountInformation();

// Get the account information as JSON
string jsonReturnValue = connection.GetAccountInformationJson();

// Get the account information as XML
XmlDocument xmlReturnValue = connection.GetAccountInformationXml();

// Create new container
connection.CreateContainer("container name");

// Get container information
Container container = connection.GetContainerInformation("container name");

// Get container information as JSON
string jsonResponse = connection.GetContainerInformationJson("container name");

// Get container information as XML
XmlDocument xmlResponse = connection.GetContainerInformationXml("container name");

// Put item in container with metadata
Dictionary<string, string> metadata = new Dictionary<string, string>();
metadata.Add("key1", "value1");
metadata.Add("key2", "value2");
metadata.Add("key3", "value3");
connection.PutStorageItem("container name", "C:\Local\File\Path\file.txt", metadata);

// Get all the containers for the account
List<string> containers = connection.GetContainers();

// Put item in container without metadata
connection.PutStorageItem("container name", "C:\Local\File\Path\file.txt");

// Put item in container from stream with metadata
Dictionary{string, string} metadata = new Dictionary{string, string}();
metadata.Add("key1", "value1");// Put item in container from stream
metadata.Add("key2", "value2");FileInfo file = new FileInfo("C:\Local\File\Path\file.txt");
metadata.Add("key3", "value3");connection.PutStorageItem("container name", file.Open(FileMode.Open), "RemoteFileName.txt");
FileInfo file = new FileInfo("C:\Local\File\Path\file.txt");
connection.PutStorageItem("container name", file.Open(FileMode.Open), "RemoteFileName.txt", metadata);

// Make path explicitly with auto-creation of "directory" structure
connection.MakePath("/dir1/dir2/dir3/dir4/file.txt");

// List all the items in a container
List<string> containerItemList = connection.GetContainerItemList("container name");

// Shortening the search results by using parameter dictionary
Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>();
parameters.Add(GetItemListParameters.Limit, 2);
parameters.Add(GetItemListParameters.Marker, 1);
parameters.Add(GetItemListParameters.Prefix, "a");
List<string> containerItemList = connection.GetContainerItemList("container name", parameters);

// Get item from container
StorageItem storageItem = connection.GetStorageItem("container name", "RemoteStorageItem.txt");


// Get item from container with request Header fields 
Dictionary<RequestHeaderFields, string> requestHeaderFields = Dictionary<RequestHeaderFields, string>();
string dummy_etag = "5c66108b7543c6f16145e25df9849f7f";
requestHeaderFields.Add(RequestHeaderFields.IfMatch, dummy_etag);
requestHeaderFields.Add(RequestHeaderFields.IfNoneMatch, dummy_etag);
requestHeaderFields.Add(RequestHeaderFields.IfModifiedSince, DateTime.Now.AddDays(6).ToString());
requestHeaderFields.Add(RequestHeaderFields.IfUnmodifiedSince, DateTime.Now.AddDays(-6).ToString());
requestHeaderFields.Add(RequestHeaderFields.Range, "0-5");
StorageItem storageItem = connection.GetStorageItem("container name", "RemoteStorageItem.txt", requestHeaderFields);

// Get item from container and put straight into local file
StorageItem storageItem = connection.GetStorageItem("container name", "RemoteStorageItem.txt", "C:\Local\File\Path\file.txt");

// Get item from container and put straight into local file with request Header fields
Dictionary<RequestHeaderFields, string> requestHeaderFields = Dictionary<RequestHeaderFields, string>();
string dummy_etag = "5c66108b7543c6f16145e25df9849f7f";
requestHeaderFields.Add(RequestHeaderFields.IfMatch, dummy_etag);
requestHeaderFields.Add(RequestHeaderFields.IfNoneMatch, dummy_etag);
requestHeaderFields.Add(RequestHeaderFields.IfModifiedSince, DateTime.Now.AddDays(6).ToString());
requestHeaderFields.Add(RequestHeaderFields.IfUnmodifiedSince, DateTime.Now.AddDays(-6).ToString());
requestHeaderFields.Add(RequestHeaderFields.Range, "0-5");
StorageItem storageItem = connection.GetStorageItem("container name", "RemoteFileName.txt", "C:\Local\File\Path\file.txt", requestHeaderFields);

// Set meta data information for an item in a container
Dictionary<string, string> metadata = new Dictionary<string, string>();
metadata.Add("key1", "value1");
metadata.Add("key2", "value2");
metadata.Add("key3", "value3");
connection.SetStorageItemMetaInformation("container name", "C:\Local\File\Path\file.txt", metadata);

// Get item information
StorageItem storageItem = connection.GetStorageItemInformation("container name", "RemoteStorageItem.txt");

// Get a list of the public containers (on the CDN)
List<string> containers = connection.GetPublicContainers();

// Mark a container as public (available on the CDN)
Uri containerPublicUrl = connection.MarkContainerAsPublic("container name");

// Get public container information
Container container = connection.GetPublicContainerInformation("container name")

// Mark a container as private (remove it from the CDN)
connection.MarkContainerAsPrivate("container name");

// Delete item from container
connection.DeleteStorageItem("container name", "RemoteStorageItem.txt");

// Delete container
connection.DeleteContainer("container name");