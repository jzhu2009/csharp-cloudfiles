using System;
using System.Collections.Generic;

namespace com.mosso.cloudfiles.domain
{
    public interface IObject
    {
        string Name { get; }
        Uri PublicUrl { get; set; }
        Dictionary<string, string> MetaTags { get; set; }
    }

    public class CF_Object : IObject
    {
        private readonly string objectName;
        protected Uri publicUrl;

        public CF_Object(string objectName)
        {
            this.objectName = objectName;
            MetaTags = new Dictionary<string, string>();
        }

        public string Name
        {
            get { return objectName; }
        }

        public Uri PublicUrl
        {
            get { return new Uri(publicUrl + Name); }
            set { publicUrl = (value == null ? value : value); }
        }

        public Dictionary<string, string> MetaTags { get; set; }
    }
}