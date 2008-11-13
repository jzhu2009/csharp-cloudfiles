///
/// See COPYING file for licensing information
///

using System;

namespace com.mosso.cloudfiles.exceptions
{
    /// <summary>
    /// This exception is thrown when the provided name for the storage item exceeds the maximum allowed by cloudfiles
    /// </summary>
    public class StorageItemNameLengthException : Exception
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public StorageItemNameLengthException()
        {
        }

        /// <summary>
        /// A constructor for explaining the reason for failure
        /// </summary>
        /// <param name="message">The message describing the failure</param>
        public StorageItemNameLengthException(String message) : base(message)
        {
        }
    }
}