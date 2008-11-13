using System;

namespace com.mosso.cloudfiles.exceptions
{
    public class ContainerAlreadyPublicException : Exception
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public ContainerAlreadyPublicException()
        {
        }
        /// <summary>
        /// A constructor for explaining the nature of the exception
        /// </summary>
        /// <param name="msg">A message describing the failure</param>
        public ContainerAlreadyPublicException(string msg)
            : base(msg)
        {
        }
    }
}