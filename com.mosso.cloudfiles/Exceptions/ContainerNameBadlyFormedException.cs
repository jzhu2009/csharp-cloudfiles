///
/// See COPYING file for licensing information
///

using System;

namespace com.mosso.cloudfiles.exceptions
{
    /// <summary>
    /// Thrown when the container name has an invalid character
    /// </summary>
    public class ContainerNameBadlyFormedException : Exception
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public ContainerNameBadlyFormedException()
        {
        }

        /// <summary>
        /// A constructor for describing that the container name has an invalid character
        /// </summary>
        /// <param name="message">A message indicating that the container name has an invalid character.</param>
        public ContainerNameBadlyFormedException(String message)
            : base(message)
        {
        }
    }
}