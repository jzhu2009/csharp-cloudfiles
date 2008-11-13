///
/// See COPYING file for licensing information
///

using System;

namespace com.mosso.cloudfiles.exceptions
{
    /// <summary>
    /// Thrown when the length of the container name exceeds the maximum allowed by cloudfiles
    /// </summary>
    public class ContainerNameLengthException : Exception
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public ContainerNameLengthException()
        {
        }

        /// <summary>
        /// A constructor for describing the maximum length allowed for container names
        /// </summary>
        /// <param name="message">A message indicating the explicit maximum length allowed.</param>
        public ContainerNameLengthException(String message) : base(message)
        {
        }
    }
}