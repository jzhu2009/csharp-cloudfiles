///
/// See COPYING file for licensing information
/// 

namespace com.mosso.cloudfiles.domain
{
    /// <summary>
    /// Container
    /// </summary>
    public class Container
    {
        public string CdnUri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        public Container(string containerName)
        {
            Name = containerName;
            ObjectCount = 0;
            ByteCount = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public long ByteCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ObjectCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
    }
}