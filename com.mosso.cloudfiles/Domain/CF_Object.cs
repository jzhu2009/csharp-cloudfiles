using System;
using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain
{
    public interface IObject
    {
        string Name { get; }
        Uri PublicUrl { get; set; }
        Dictionary<string, string> Metadata { get; set; }
        long ContentLength { get; }
        string ETag { get; }
        string ContentType { get; }
        Uri CDNManagementUrl { get; set; }
        string AuthToken { get; set; }
        Uri StorageUrl { get; set; }
        string StorageToken { get; set; }
        UserCredentials UserCredentials { get; set; }
        string ContainerName { get; set; }
    }

    public class CF_Object : IObject
    {
        private readonly string objectName;
        protected Uri publicUrl;
        protected long contentLength;
        protected string etag;
        protected string contentType;
        protected Dictionary<string, string> metadata;

        public CF_Object(string objectName) : this(objectName, new Dictionary<string, string>()){}

        public CF_Object(string objectName, Dictionary<string, string> metadata)
        {
            this.objectName = objectName;
            this.metadata = metadata;
        }

        public string Name
        {
            get { return objectName; }
        }

        public long ContentLength
        {
            get
            {
                CloudFilesHeadObject();
                return contentLength;
            }
        }

        public string ETag
        {
            get
            {
                CloudFilesHeadObject(); 
                return etag;
            }
        }

        public string ContentType
        {
            get
            {
                CloudFilesHeadObject(); 
                return contentType;
            }
        }

        public Uri PublicUrl
        {
            get { return new Uri(publicUrl + Name); }
            set { publicUrl = (value == null ? value : value); }
        }

        public Dictionary<string, string> Metadata
        {
            get { return metadata; }
            set
            {
                CloudFilesPostObject(value);
                metadata = value;
            }
        }

        public Uri CDNManagementUrl { get; set; }
        public string AuthToken { get; set; }
        public Uri StorageUrl { get; set; }
        public string StorageToken { get; set; }
        public string ContainerName { get; set; }
        public UserCredentials UserCredentials { get; set; }

        protected virtual void CloudFilesHeadObject()
        {
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation(StorageUrl.ToString(), ContainerName, objectName, StorageToken);
            try
            {
                GetStorageItemInformationResponse getStorageItemResponse = new ResponseFactory<GetStorageItemInformationResponse>().Create(new CloudFilesRequest(getStorageItemInformation, UserCredentials.ProxyCredentials));
                contentLength = long.Parse(getStorageItemResponse.ContentLength);
                contentType = getStorageItemResponse.ContentType;
                etag = getStorageItemResponse.ETag;
                metadata = getStorageItemResponse.Metadata;
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object " + objectName + " does not exist inside container: " + ContainerName);
            }
        }

        protected virtual void CloudFilesPostObject(Dictionary<string, string> metadata)
        {
            SetStorageItemMetaInformation setStorageItemInformation = new SetStorageItemMetaInformation(StorageUrl.ToString(), ContainerName, objectName, metadata, StorageToken);
            try
            {
                new ResponseFactory<SetStorageItemMetaInformationResponse>().Create(new CloudFilesRequest(setStorageItemInformation, UserCredentials.ProxyCredentials));
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");
            }
        }
    }
}